// =============================================================
//  AddTodoForm.cs — TODO "추가" 폼 (디자이너 분리형)
//  · 컨트롤 선언/배치 → AddTodoForm.Designer.cs
//  · 동작/이벤트     → AddTodoForm.cs (이 파일)
//  · POST /todos     → TodoApi.PostAsync 사용
// =============================================================
// [신규] 원본의 단일 TodoForm 클래스를 Add/Edit 두 폼으로 분리한 것 중
//        "추가" 전용 폼. 디자이너 친화적으로 partial class + Designer.cs 구조.

using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KwuTodoAI
{
    public partial class AddTodoForm : Form
    {
        private readonly HttpClient _http;

        /// <summary>저장 성공 시 생성된 TodoItem. 취소 시 null.</summary>
        public TodoItem? Result { get; private set; }

        public AddTodoForm(HttpClient http)
        {
            _http = http;
            InitializeComponent();
            HookEvents();
        }

        private void HookEvents()
        {
            chkNoDue.CheckedChanged += (s, e) => dtpDueDate.Enabled = !chkNoDue.Checked;
            btnSave.Click   += async (s, e) => await OnSaveAsync();
            btnCancel.Click += (s, e) =>
            {
                Result = null;
                DialogResult = DialogResult.Cancel;
                Close();
            };
        }

        // ── 저장 ─────────────────────────────────────────────────
        private async Task OnSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                lblStatus.Text = "⚠ 제목을 입력하세요.";
                txtTitle.Focus();
                return;
            }

            btnSave.Enabled = false;
            btnSave.Text    = "저장 중...";
            lblStatus.Text  = "";

            string  priority = rdoHigh.Checked ? "high" : rdoLow.Checked ? "low" : "medium";
            string  category = cboCategory.SelectedItem?.ToString() ?? "기타";
            string? dueDate  = chkNoDue.Checked ? null : dtpDueDate.Value.ToString("yyyy-MM-dd");
            bool    isTeam   = chkTeamWork.Checked;

            var body = new
            {
                title        = txtTitle.Text.Trim(),
                priority,
                category,
                due_date     = dueDate,
                source_event = (string?)null,
            };

            var created = await TodoApi.PostAsync<TodoItem>(
                _http, "/todos", body, ShowError);

            if (created != null)
            {
                created.IsTeamWork = isTeam;
                created.RecalculateDDay();
                Result = created;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.Text    = "추가";
            }
        }

        private void ShowError(string msg)
        {
            if (InvokeRequired) { Invoke(() => ShowError(msg)); return; }
            lblStatus.Text = "⚠ " + msg;
            btnSave.Enabled = true;
            btnSave.Text    = "추가";
        }
    }
}
