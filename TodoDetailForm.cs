// =============================================================
//  TodoDetailForm.cs — TODO 상세 / 편집 다이얼로그 (디자이너 분리형)
//  · 컨트롤 선언/배치 → TodoDetailForm.Designer.cs
//  · 동작/이벤트     → TodoDetailForm.cs (이 파일)
//  · 제목 길이가 길면 폼 가로 길이를 더 늘림 (초기 크기보다 작아지지는 않음)
//  · 본문(이유) 직접 편집 가능 → "저장" 버튼으로 서버 PUT
// =============================================================
// [신규] 원본 MainForm.ShowDetail() 안의 코드형 임시 Form 을 별도 폼으로 분리.
//        편집 가능(제목·본문) + 저장 버튼 + 동적 크기 + 다크/라이트 테마 지원.

using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KwuTodoAI
{
    public partial class TodoDetailForm : Form
    {
        private readonly HttpClient _http;
        private readonly TodoItem   _todo;
        private readonly bool       _isDark;

        // 디자이너가 지정한 초기 폼 크기 — AdjustSize는 이 값보다 절대 작아지지 않음
        private Size _initialClientSize;

        /// <summary>저장 성공 시 갱신된 TodoItem. 변경 없음/취소 시 null.</summary>
        public TodoItem? Result { get; private set; }

        public TodoDetailForm(HttpClient http, TodoItem todo, bool isDark = false)
        {
            _http   = http;
            _todo   = todo;
            _isDark = isDark;

            InitializeComponent();
            _initialClientSize = ClientSize;   // 디자이너 지정 크기 보존
            ApplyTheme();
            PopulateFields();
            AdjustSize();
            HookEvents();
        }

        // ── 이벤트 ────────────────────────────────────────────────
        private void HookEvents()
        {
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            btnSave.Click  += async (s, e) => await OnSaveAsync();

            // 본문 컨텍스트 메뉴 항목 동작
            ctxCopy.Click  += (s, e) => { if (txtBody.SelectionLength > 0) txtBody.Copy(); };
            ctxPaste.Click += (s, e) =>
            {
                if (Clipboard.ContainsText()) txtBody.Paste();
            };
            ctxClear.Click += (s, e) => txtBody.Clear();

            // 폼이 열린 직후 포커스를 "닫기" 버튼으로 옮겨
            // 본문 TextBox에 캐럿이 깜빡거리는 것을 방지
            Shown += (s, e) =>
            {
                btnClose.Focus();
                PositionDDay();          // 초기 표시 시점에 D-Day 위치 확정
                PositionFooterButtons(); // 초기 표시 시점에 푸터 버튼 위치 확정
            };

            // ── D-Day 라벨을 헤더 우측에 동적으로 고정 ──
            //   디자이너의 Location/Anchor 값과 무관하게 항상 헤더 폭에 맞춰 재배치한다.
            //   (디자이너에서 라벨을 옮겨두어도 런타임에서는 이 핸들러가 우선 적용됨)
            pnlHead.Resize += (s, e) => PositionDDay();
            Load           += (s, e) => PositionDDay();

            // ── 저장 / 닫기 버튼을 푸터 우측에 동적으로 정렬 ──
            //   창 크기를 줄이거나 늘릴 때 버튼이 항상 우측 끝에서 일정 여백을 유지한다.
            pnlFoot.Resize += (s, e) => PositionFooterButtons();
            Load           += (s, e) => PositionFooterButtons();

            // 제목 변경 시 ErrorProvider 자동 해제
            txtTitle.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtTitle.Text))
                    errorProvider.SetError(txtTitle, "");
            };
        }

        /// <summary>D-Day 라벨을 헤더의 오른쪽 끝에서 24px 여백으로 정렬.</summary>
        private void PositionDDay()
        {
            if (pnlHead == null || lblDDay == null) return;
            int x = pnlHead.Width - lblDDay.PreferredWidth - 24;
            if (x < 0) x = 0;
            lblDDay.Location = new Point(x, 18);
        }

        /// <summary>
        /// 저장/닫기 버튼을 푸터 패널의 오른쪽 끝에 동적으로 정렬.
        ///   · btnClose : 우측 끝에서 20px 여백
        ///   · btnSave  : btnClose 왼쪽 12px 여백
        ///   · y 좌표는 디자이너의 값을 유지
        /// </summary>
        private void PositionFooterButtons()
        {
            if (pnlFoot == null || btnSave == null || btnClose == null) return;

            const int MARGIN_RIGHT = 20;
            const int GAP_BETWEEN  = 12;

            int closeX = pnlFoot.Width - btnClose.Width - MARGIN_RIGHT;
            int saveX  = closeX - btnSave.Width - GAP_BETWEEN;

            // 창이 너무 좁아져도 왼쪽 가장자리에서 잘리지 않게 보호
            if (saveX < 12)
            {
                saveX  = 12;
                closeX = saveX + btnSave.Width + GAP_BETWEEN;
            }

            btnSave.Location  = new Point(saveX,  btnSave.Location.Y);
            btnClose.Location = new Point(closeX, btnClose.Location.Y);
        }

        // ── 데이터 채우기 ─────────────────────────────────────────
        private void PopulateFields()
        {
            Text = "TODO 상세 — " + _todo.Title;

            // 우선순위 배지
            lblPriBadge.Text      = $"  {_todo.PriorityLabel}  ";
            lblPriBadge.BackColor = _todo.PriorityColor;
            lblPriBadge.ForeColor = Color.White;

            lblDDay.Text      = _todo.DDayText;
            lblDDay.ForeColor = _todo.PriorityColor;

            txtTitle.Text = _todo.Title;
            lblMeta.Text =
                $"📅 마감일 :  {_todo.DueDate}\n" +
                $"📂 카테고리:  {_todo.Category}\n" +
                $"📌 유형    :  {_todo.Type}{(_todo.IsTeamWork ? "  (팀 과제)" : "")}";

            // 본문 — 이유 + 실행 항목
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("💡 판단 이유");
            sb.AppendLine(string.IsNullOrWhiteSpace(_todo.Reason) ? "  (없음)" : $"  {_todo.Reason}");
            if (_todo.ActionItems.Any())
            {
                sb.AppendLine();
                sb.AppendLine("✅ 실행 항목");
                foreach (var a in _todo.ActionItems) sb.AppendLine($"  • {a}");
            }
            txtBody.Text = sb.ToString();
        }

        // ── 제목 길이에 따라 폼 가로 길이 확장 (초기 크기 이하로 줄어들지 않음) ──
        private void AdjustSize()
        {
            int titleLen = _todo.Title.Length;
            // 한 글자 ≈ 12px, 여백 + D-Day 영역 ≈ 240px
            int estimateW = 240 + Math.Max(titleLen, 12) * 12;
            int newW = Math.Clamp(estimateW, _initialClientSize.Width, 1100);

            // ClientSize 갱신 — 높이는 초기값 그대로 유지하되 너무 작아지지 않게
            ClientSize = new Size(newW, _initialClientSize.Height);
        }

        // ── 저장 (제목 + 이유 업데이트 → PUT) ────────────────────
        private async Task OnSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                errorProvider.SetError(txtTitle, "제목을 비울 수 없습니다.");
                lblStatus.Text = "⚠ 제목을 비울 수 없습니다.";
                txtTitle.Focus();
                return;
            }

            btnSave.Enabled = false;
            btnSave.Text    = "저장 중...";
            lblStatus.Text  = "";

            // 본문에서 "💡 판단 이유" 다음 줄들만 추출해 Reason으로 저장
            string newReason = ExtractReason(txtBody.Text);
            string newTitle  = txtTitle.Text.Trim();

            var body = new
            {
                title        = newTitle,
                priority     = _todo.Priority,
                category     = _todo.Category,
                due_date     = string.IsNullOrEmpty(_todo.DueDate) ? null : _todo.DueDate,
                source_event = string.IsNullOrWhiteSpace(newReason) ? (string?)null : newReason,
                is_done      = _todo.IsCompleted,
            };

            bool ok = await TodoApi.PutAsync(_http, $"/todos/{_todo.Id}", body, ShowError);

            if (ok)
            {
                _todo.Title  = newTitle;
                _todo.Reason = newReason;
                Result = _todo;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.Text    = "저장";
            }
        }

        /// <summary>편집된 본문 텍스트에서 "💡 판단 이유" 섹션의 내용만 추출.</summary>
        private static string ExtractReason(string body)
        {
            var lines = body.Replace("\r\n", "\n").Split('\n');
            var sb = new System.Text.StringBuilder();
            bool inReason = false;
            foreach (var rawLine in lines)
            {
                var line = rawLine;
                if (line.Contains("💡 판단 이유")) { inReason = true; continue; }
                if (line.Contains("✅ 실행 항목")) { inReason = false; continue; }
                if (inReason)
                {
                    var trimmed = line.TrimStart();
                    if (trimmed.Length == 0) continue;
                    if (sb.Length > 0) sb.Append('\n');
                    sb.Append(trimmed);
                }
            }
            string result = sb.ToString().Trim();
            return result == "(없음)" ? "" : result;
        }

        private void ShowError(string msg)
        {
            if (InvokeRequired) { Invoke(() => ShowError(msg)); return; }
            lblStatus.Text  = "⚠ " + msg;
            btnSave.Enabled = true;
            btnSave.Text    = "저장";
        }

        // ── 다크/라이트 테마 적용 ────────────────────────────────
        private void ApplyTheme()
        {
            Color bg       = _isDark ? Color.FromArgb(15, 18, 30)    : Color.FromArgb(242, 245, 255);
            Color surface  = _isDark ? Color.FromArgb(24, 28, 46)    : Color.White;
            Color text     = _isDark ? Color.FromArgb(232, 238, 255) : Color.FromArgb(25, 30, 60);
            Color subtext  = _isDark ? Color.FromArgb(170, 185, 220) : Color.FromArgb(115, 126, 162);
            Color inputBg  = _isDark ? Color.FromArgb(32, 38, 60)    : Color.FromArgb(248, 250, 255);

            BackColor              = bg;
            pnlHead.BackColor      = surface;
            pnlMeta.BackColor      = bg;
            pnlBodyArea.BackColor  = bg;
            pnlFoot.BackColor      = bg;
            gbxBody.BackColor      = surface;
            gbxBody.ForeColor      = text;

            txtTitle.BackColor = inputBg;     txtTitle.ForeColor = text;
            txtBody.BackColor  = surface;     txtBody.ForeColor  = text;
            lblMeta.ForeColor  = subtext;
        }
    }
}
