// =============================================================
//  AddTodoForm.cs — TODO "추가" 폼 (디자이너 분리형)
//  · 컨트롤 선언/배치 → AddTodoForm.Designer.cs
//  · 동작/이벤트     → AddTodoForm.cs (이 파일)
//  · POST /todos     → TodoApi.PostAsync 사용
// =============================================================
// [신규] 원본의 단일 TodoForm 클래스를 Add/Edit 두 폼으로 분리한 것 중
//        "추가" 전용 폼. 디자이너 친화적으로 partial class + Designer.cs 구조.
// [변경] 다크모드 지원 — 생성자에 isDark 인자 추가, ApplyTheme() 으로 색상 적용

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
        private readonly bool       _isDark;

        /// <summary>저장 성공 시 생성된 TodoItem. 취소 시 null.</summary>
        public TodoItem? Result { get; private set; }

        public AddTodoForm(HttpClient http, bool isDark = false)
        {
            _http   = http;
            _isDark = isDark;
            InitializeComponent();
            ApplyTheme();    // [변경] 다크/라이트 테마 적용
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

        // ── 다크/라이트 테마 적용 ────────────────────────────────
        // [신규] 다크모드일 때 모든 컨트롤의 색상을 어두운 톤으로 전환.
        //   라이트 모드는 디자이너에 지정된 흰 배경/검정 글자 그대로 사용.
        private void ApplyTheme()
        {
            if (!_isDark) return;   // 라이트는 디자이너 기본값 사용

            Color bg       = Color.FromArgb(15, 18, 30);
            Color surface  = Color.FromArgb(24, 28, 46);
            Color inputBg  = Color.FromArgb(32, 38, 60);
            Color text     = Color.FromArgb(232, 238, 255);
            Color subtext  = Color.FromArgb(170, 185, 220);
            Color accent   = Color.FromArgb(82, 130, 255);
            Color cancelBg = Color.FromArgb(45, 52, 78);

            BackColor = bg;

            // 헤더는 ACCENT 그대로 유지 (브랜드 색상)
            lblHeader.ForeColor = accent;

            // 섹션 캡션 라벨
            lblTitleCap.ForeColor = text;
            lblDueCap.ForeColor   = text;
            lblPriCap.ForeColor   = text;
            lblCatCap.ForeColor   = text;
            lblTeamCap.ForeColor  = text;
            chkNoDue.ForeColor    = subtext;

            // 입력 박스
            txtTitle.BackColor    = inputBg;  txtTitle.ForeColor    = text;
            cboCategory.BackColor = inputBg;  cboCategory.ForeColor = text;

            // 라디오/체크박스 (배경 투명 → 폼 배경 비침)
            rdoHigh.ForeColor     = text;     rdoHigh.BackColor     = bg;
            rdoMedium.ForeColor   = text;     rdoMedium.BackColor   = bg;
            rdoLow.ForeColor      = text;     rdoLow.BackColor      = bg;
            chkTeamWork.ForeColor = text;     chkTeamWork.BackColor = bg;

            // 취소 버튼 — 다크 모드에서 어두운 회색 배경 + 밝은 글자
            btnCancel.BackColor = cancelBg;
            btnCancel.ForeColor = text;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(70, 80, 110);

            // btnSave 는 ACCENT 색상 그대로 (라이트/다크 공통)
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
