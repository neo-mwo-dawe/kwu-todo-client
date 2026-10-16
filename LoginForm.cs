// =============================================================
//  LoginForm.cs — 로그인 폼 (디자이너 분리형)
//  · 컨트롤 선언/배치 → LoginForm.Designer.cs
//  · 동작/이벤트     → LoginForm.cs (이 파일)
// =============================================================
//
// ─────────────────────────────────────────────────────────────
// [변경 사항] — 원본 (지원의 develop 브랜치) 대비
//   · 원본은 단일 클래스에서 BuildUI() 로 코드 기반 UI 구성
//   → partial class 로 분리, InitializeComponent() 는
//     LoginForm.Designer.cs 로 이동 (Visual Studio 디자이너 편집 가능)
//   · DoLogin() 에서 lblErr 초기화 보강
//   · 카드 중앙 정렬을 위한 CenterCard() 도입 (Load/Resize 시 호출)
//   · 테마 토글 시 btnLogin 색상도 함께 갱신
//   · 다크모드 SUBTEXT 색상 가독성 강화
// ─────────────────────────────────────────────────────────────

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KwuTodoAI
{
    /// <summary>
    /// 로그인 화면. 학번/비밀번호 입력 후 MainForm 으로 진입한다.
    /// (실제 인증 로직은 없고, 빈 값만 막는다 — 추후 서버 인증 연동 가능.)
    /// </summary>
    public partial class LoginForm : Form
    {
        // 현재 적용 중인 테마가 다크 모드인지 여부.
        // 로그인 시 MainForm 에 동일한 값을 넘겨 일관된 테마로 시작하게 한다.
        private bool _isDark = false;

        // ── 테마 색상 ─────────────────────────────────────────────
        // C# expression-bodied 속성으로 정의해 _isDark 가 바뀌면
        // 다음 접근 때 자동으로 새 색이 계산되도록 한다.
        // (배경 / 카드 / 강조 / 본문 텍스트 / 보조 텍스트 / 입력 배경)
        private Color BG      => _isDark ? Color.FromArgb(18, 22, 36)    : Color.FromArgb(235, 240, 255);
        private Color CARD    => _isDark ? Color.FromArgb(28, 33, 52)    : Color.White;
        private Color ACCENT  => Color.FromArgb(82, 130, 255);              // 강조색 (테마 무관)
        private Color TEXT    => _isDark ? Color.FromArgb(220, 228, 255) : Color.FromArgb(30, 35, 60);
        // [변경] 다크 SUBTEXT 색 밝게 조정 — (130,145,180) → (160,175,210), 가독성 ↑
        private Color SUBTEXT => _isDark ? Color.FromArgb(160, 175, 210) : Color.FromArgb(110, 120, 155);
        private Color INPUTBG => _isDark ? Color.FromArgb(38, 44, 68)    : Color.FromArgb(248, 250, 255);

        /// <summary>생성자 — 디자이너 컨트롤 초기화 후 이벤트 연결과 테마 적용.</summary>
        public LoginForm()
        {
            InitializeComponent();   // Designer.cs 의 컨트롤 트리 구축
            HookEvents();            // 사용자 입력/창 크기 변경 이벤트 연결
            ApplyTheme();            // 초기 테마(라이트) 색상 적용
        }

        /// <summary>
        /// 각 컨트롤의 이벤트 핸들러 연결을 한 곳에 모은다.
        /// 디자이너 파일에 직접 이벤트를 거는 대신 코드-비하인드에서 처리.
        /// </summary>
        private void HookEvents()
        {
            // 🌙/☀ 토글 — 클릭마다 다크/라이트 모드 전환
            btnTheme.Click += (s, e) =>
            {
                _isDark = !_isDark;
                btnTheme.Text = _isDark ? "☀" : "🌙";
                ApplyTheme();
            };

            // 로그인 버튼 (async — KLAS 서버 호출이 수 초 걸릴 수 있음)
            btnLogin.Click += async (s, e) => await DoLoginAsync();

            // 학번 입력칸에서 Enter → 비밀번호 칸으로 포커스 이동
            txtId.KeyDown  += (s, e) => { if (e.KeyCode == Keys.Enter) txtPw.Focus(); };

            // 비밀번호 입력칸에서 Enter → 즉시 로그인 시도
            txtPw.KeyDown  += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    await DoLoginAsync();
                }
            };

            // [변경] 신규 — 창 크기가 바뀌거나 처음 표시될 때 카드(pnlCard)를 중앙에 재정렬
            Resize += (s, e) => CenterCard();
            Load   += (s, e) => CenterCard();
        }

        /// <summary>
        /// 폼 한가운데에 입력 카드를 위치시킨다.
        /// FormBorderStyle = FixedSingle 이라 사용자가 폼 크기를 바꿀 일은 거의 없지만,
        /// DPI 변경/창 이동 시에도 안전하게 중앙 정렬이 유지되도록 처리.
        /// </summary>
        private void CenterCard()
        {
            int x = (ClientSize.Width  - pnlCard.Width)  / 2;
            int y = (ClientSize.Height - pnlCard.Height) / 2 - 10; // 살짝 위로 올려 시각적 균형
            pnlCard.Location = new Point(Math.Max(0, x), Math.Max(50, y));
        }

        /// <summary>
        /// KLAS 로그인 처리 (성호 통합).
        /// 빈 값 검증 → ApiClient.LoginAsync 호출 → 성공 시 MainForm 진입.
        /// </summary>
        private async Task DoLoginAsync()
        {
            lblErr.Text = "";   // 이전 오류 메시지 초기화

            if (string.IsNullOrWhiteSpace(txtId.Text))
            { lblErr.Text = "⚠ 학번을 입력해주세요."; txtId.Focus(); return; }

            if (string.IsNullOrWhiteSpace(txtPw.Text))
            { lblErr.Text = "⚠ 비밀번호를 입력해주세요."; txtPw.Focus(); return; }

            // 로그인 진행 중 UI 잠금
            btnLogin.Enabled = false;
            txtId.Enabled    = false;
            txtPw.Enabled    = false;
            string originalText = btnLogin.Text;
            btnLogin.Text = "로그인 중...";
            lblErr.ForeColor = SUBTEXT;
            lblErr.Text = "🔄 KLAS 인증 중입니다. 잠시만 기다려주세요...";

            LoginResult result;
            try
            {
                result = await ApiClient.Instance.LoginAsync(txtId.Text.Trim(), txtPw.Text);
            }
            finally
            {
                btnLogin.Enabled = true;
                txtId.Enabled    = true;
                txtPw.Enabled    = true;
                btnLogin.Text    = originalText;
            }

            if (!result.Success)
            {
                lblErr.ForeColor = Color.FromArgb(220, 60, 60);
                lblErr.Text = string.IsNullOrEmpty(result.Message)
                    ? "⚠ 로그인에 실패했습니다."
                    : "⚠ " + result.Message;
                txtPw.Focus();
                txtPw.SelectAll();
                return;
            }

            // 로그인 성공 → MainForm 진입
            var main = new MainForm(_isDark);
            main.Show();
            Hide();
            main.FormClosed += (s, e) => Close();
        }

        /// <summary>
        /// 현재 _isDark 값에 맞춰 모든 컨트롤의 색상을 일괄 갱신.
        /// 다크 ↔ 라이트 전환 시 호출.
        /// </summary>
        private void ApplyTheme()
        {
            BackColor          = BG;
            pnlCard.BackColor  = CARD;
            lblLogo.ForeColor  = ACCENT;
            lblSub.ForeColor   = SUBTEXT;
            lblId.ForeColor    = TEXT;
            lblPw.ForeColor    = TEXT;
            txtId.BackColor    = INPUTBG;
            txtId.ForeColor    = TEXT;
            txtPw.BackColor    = INPUTBG;
            txtPw.ForeColor    = TEXT;
            btnTheme.BackColor = BG;
            btnTheme.ForeColor = TEXT;
            // [변경] 원본은 btnLogin 색을 ApplyTheme 에서 갱신하지 않았음 → 명시적 갱신 추가
            btnLogin.BackColor = ACCENT;
            btnLogin.ForeColor = Color.White;
            Invalidate();  // 강제 다시 그리기 (잔상 방지)
        }
    }
}
