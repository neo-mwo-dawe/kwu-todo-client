// =============================================================
//  EditTodoForm.cs — TODO "편집" 폼 (디자이너 분리형)
//  · 컨트롤 선언/배치 → EditTodoForm.Designer.cs
//  · 동작/이벤트     → EditTodoForm.cs (이 파일)
//  · PUT /todos/{id} → TodoApi.PutAsync 사용
// =============================================================
// [신규] 원본의 단일 TodoForm 클래스를 Add/Edit 두 폼으로 분리한 것 중
//        "편집" 전용 폼. 기존 TodoItem 값으로 폼 초기화 후 서버에 PUT 요청.
// [변경] 다크모드 지원 — 생성자에 isDark 인자 추가, ApplyTheme() 으로 색상 적용

using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KwuTodoAI
{
    public partial class EditTodoForm : Form
    {
        private readonly HttpClient _http;
        private readonly TodoItem   _original;
        private readonly bool       _isDark;

        /// <summary>저장 성공 시 갱신된 TodoItem. 취소 시 null.</summary>
        public TodoItem? Result { get; private set; }

        public EditTodoForm(HttpClient http, TodoItem existing, bool isDark = false)
        {
            _http     = http;
            _original = existing;
            _isDark   = isDark;
            InitializeComponent();
            ApplyTheme();     // [변경] 다크/라이트 테마 적용
            PopulateFields(existing);
            HookEvents();
        }

        // ── 다크/라이트 테마 적용 ────────────────────────────────
        // [신규] 다크모드일 때 모든 컨트롤의 색상을 어두운 톤으로 전환.
        //   라이트 모드는 디자이너에 지정된 흰 배경/검정 글자 그대로 사용.
        private void ApplyTheme()
        {
            if (!_isDark) return;   // 라이트는 디자이너 기본값 사용

            Color bg       = Color.FromArgb(15, 18, 30);
            Color inputBg  = Color.FromArgb(32, 38, 60);
            Color text     = Color.FromArgb(232, 238, 255);
            Color subtext  = Color.FromArgb(170, 185, 220);
            Color accent   = Color.FromArgb(82, 130, 255);
            Color cancelBg = Color.FromArgb(45, 52, 78);

            BackColor = bg;

            // 헤더 — ACCENT 색상 유지
            lblHeader.ForeColor = accent;

            // 섹션 캡션
            lblTitleCap.ForeColor = text;
            lblDueCap.ForeColor   = text;
            lblPriCap.ForeColor   = text;
            lblCatCap.ForeColor   = text;
            lblTeamCap.ForeColor  = text;
            chkNoDue.ForeColor    = subtext;

            // 입력
            txtTitle.BackColor    = inputBg;  txtTitle.ForeColor    = text;
            cboCategory.BackColor = inputBg;  cboCategory.ForeColor = text;

            // 라디오/체크박스
            rdoHigh.ForeColor     = text;     rdoHigh.BackColor     = bg;
            rdoMedium.ForeColor   = text;     rdoMedium.BackColor   = bg;
            rdoLow.ForeColor      = text;     rdoLow.BackColor      = bg;
            chkTeamWork.ForeColor = text;     chkTeamWork.BackColor = bg;

            // 취소 버튼
            btnCancel.BackColor = cancelBg;
            btnCancel.ForeColor = text;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(70, 80, 110);
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

        // ── 기존 값으로 폼 채우기 ────────────────────────────────
        private void PopulateFields(TodoItem item)
        {
            txtTitle.Text = item.Title;

            if (!string.IsNullOrEmpty(item.DueDate) && DateTime.TryParse(item.DueDate, out var due))
            {
                dtpDueDate.Value   = due;
                dtpDueDate.Enabled = true;
                chkNoDue.Checked   = false;
            }
            else
            {
                chkNoDue.Checked   = true;
                dtpDueDate.Enabled = false;
            }

            switch (item.Priority)
            {
                case "high": rdoHigh.Checked   = true; break;
                case "low":  rdoLow.Checked    = true; break;
                default:     rdoMedium.Checked = true; break;
            }

            cboCategory.SelectedIndex = item.Category switch
            {
                "행정" => 1,
                "장학" => 2,
                "기타" => 3,
                _      => 0,
            };

            chkTeamWork.Checked = item.IsTeamWork;
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

            var body = new
            {
                title        = txtTitle.Text.Trim(),
                priority,
                category,
                due_date     = dueDate,
                source_event = string.IsNullOrWhiteSpace(_original.Reason) ? (string?)null : _original.Reason,
                is_done      = _original.IsCompleted,
            };

            bool ok = await TodoApi.PutAsync(_http, $"/todos/{_original.Id}", body, ShowError);

            if (ok)
            {
                _original.Title      = txtTitle.Text.Trim();
                _original.Priority   = priority;
                _original.Category   = category;
                _original.DueDate    = dueDate ?? "";
                _original.IsTeamWork = chkTeamWork.Checked;
                _original.RecalculateDDay();
                Result = _original;

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.Text    = "저장";
            }
        }

        private void ShowError(string msg)
        {
            if (InvokeRequired) { Invoke(() => ShowError(msg)); return; }
            lblStatus.Text = "⚠ " + msg;
            btnSave.Enabled = true;
            btnSave.Text    = "저장";
        }
    }
}
