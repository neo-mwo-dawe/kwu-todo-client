// =============================================================
//  LoginForm.Designer.cs — Visual Studio 디자이너 자동 생성 영역
//  · 도구상자에서 컨트롤을 추가하거나 속성 편집 시 이 파일이 자동 갱신됨
//  · 동작/이벤트는 LoginForm.cs 에 작성
// =============================================================
// [신규] 원본 LoginForm.cs 가 단일 클래스로 BuildUI() 했던 것을
//        partial 클래스 + Designer.cs 패턴으로 분리한 결과 파일.
//        도구상자 컴포넌트(ToolTip 등) 다수 포함.

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KwuTodoAI
{
    partial class LoginForm
    {
        /// <summary>필수 디자이너 변수입니다.</summary>
        private IContainer components = null!;

        // ── 디자이너 컨트롤들 ─────────────────────────────────────
        private Panel   pnlCard  = null!;
        private Label   lblLogo  = null!;
        private Label   lblSub   = null!;
        private Label   lblId    = null!;
        private Label   lblPw    = null!;
        private Label   lblErr   = null!;
        private TextBox txtId    = null!;
        private TextBox txtPw    = null!;
        private Button  btnLogin = null!;
        private Button  btnTheme = null!;

        // ── 도구상자 비주얼 컴포넌트 트레이 ────────────────────────
        private ToolTip              toolTip              = null!;
        private ErrorProvider        errorProvider        = null!;
        private HelpProvider         helpProvider         = null!;
        private ImageList            imageList            = null!;
        private System.Windows.Forms.Timer    timer       = null!;
        private NotifyIcon           notifyIcon           = null!;
        private ContextMenuStrip     ctxMenu              = null!;
        private ToolStripMenuItem    ctxAbout             = null!;
        private ToolStripMenuItem    ctxExit              = null!;
        private FontDialog           fontDialog           = null!;
        private ColorDialog          colorDialog          = null!;
        private OpenFileDialog       openFileDialog       = null!;
        private SaveFileDialog       saveFileDialog       = null!;
        private FolderBrowserDialog  folderBrowserDialog  = null!;
        private BindingSource        bindingSource        = null!;
        private BackgroundWorker     backgroundWorker     = null!;

        /// <summary>사용 중인 모든 리소스를 정리합니다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        #region 디자이너 자동 생성 코드

        /// <summary>
        ///  디자이너 지원에 필요한 메서드입니다.
        ///  이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();

            pnlCard  = new Panel();
            lblLogo  = new Label();
            lblSub   = new Label();
            lblId    = new Label();
            txtId    = new TextBox();
            lblPw    = new Label();
            txtPw    = new TextBox();
            lblErr   = new Label();
            btnLogin = new Button();
            btnTheme = new Button();

            // 컴포넌트 트레이
            toolTip             = new ToolTip(components);
            errorProvider       = new ErrorProvider(components);
            helpProvider        = new HelpProvider();
            imageList           = new ImageList(components);
            timer               = new System.Windows.Forms.Timer(components);
            notifyIcon          = new NotifyIcon(components);
            ctxMenu             = new ContextMenuStrip(components);
            ctxAbout            = new ToolStripMenuItem();
            ctxExit             = new ToolStripMenuItem();
            fontDialog          = new FontDialog();
            colorDialog         = new ColorDialog();
            openFileDialog      = new OpenFileDialog();
            saveFileDialog      = new SaveFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            bindingSource       = new BindingSource(components);
            backgroundWorker    = new BackgroundWorker();

            pnlCard.SuspendLayout();
            ctxMenu.SuspendLayout();
            ((ISupportInitialize)errorProvider).BeginInit();
            ((ISupportInitialize)bindingSource).BeginInit();
            SuspendLayout();

            //
            // pnlCard
            //
            pnlCard.BackColor = Color.White;
            pnlCard.Controls.Add(lblLogo);
            pnlCard.Controls.Add(lblSub);
            pnlCard.Controls.Add(lblId);
            pnlCard.Controls.Add(txtId);
            pnlCard.Controls.Add(lblPw);
            pnlCard.Controls.Add(txtPw);
            pnlCard.Controls.Add(lblErr);
            pnlCard.Controls.Add(btnLogin);
            pnlCard.Location = new Point(50, 60);
            pnlCard.Name     = "pnlCard";
            pnlCard.Size     = new Size(320, 390);
            pnlCard.TabIndex = 1;
            //
            // lblLogo (카드 가로 중앙 정렬)
            //
            lblLogo.Font      = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblLogo.Location  = new Point(20, 30);
            lblLogo.Name      = "lblLogo";
            lblLogo.Size      = new Size(280, 40);
            lblLogo.TabIndex  = 0;
            lblLogo.Text      = "🎓 KWU TODO";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblSub (카드 가로 중앙 정렬)
            //
            lblSub.Font      = new Font("맑은 고딕", 9F);
            lblSub.Location  = new Point(20, 72);
            lblSub.Name      = "lblSub";
            lblSub.Size      = new Size(280, 20);
            lblSub.TabIndex  = 1;
            lblSub.Text      = "광운대 AI 학사일정 관리 시스템";
            lblSub.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblId
            //
            lblId.AutoSize = true;
            lblId.Font     = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblId.Location = new Point(28, 114);
            lblId.Name     = "lblId";
            lblId.TabIndex = 2;
            lblId.Text     = "학번";
            //
            // txtId
            //
            txtId.BorderStyle     = BorderStyle.FixedSingle;
            txtId.Font            = new Font("맑은 고딕", 10F);
            txtId.Location        = new Point(28, 136);
            txtId.MaxLength       = 50;
            txtId.Name            = "txtId";
            txtId.PlaceholderText = "학번을 입력하세요";
            txtId.Size            = new Size(264, 30);
            txtId.TabIndex        = 3;
            //
            // lblPw
            //
            lblPw.AutoSize = true;
            lblPw.Font     = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblPw.Location = new Point(28, 186);
            lblPw.Name     = "lblPw";
            lblPw.TabIndex = 4;
            lblPw.Text     = "비밀번호";
            //
            // txtPw
            //
            txtPw.BorderStyle     = BorderStyle.FixedSingle;
            txtPw.Font            = new Font("맑은 고딕", 10F);
            txtPw.Location        = new Point(28, 208);
            txtPw.MaxLength       = 50;
            txtPw.Name            = "txtPw";
            txtPw.PasswordChar    = '●';
            txtPw.PlaceholderText = "비밀번호를 입력하세요";
            txtPw.Size            = new Size(264, 30);
            txtPw.TabIndex        = 5;
            //
            // lblErr
            //
            lblErr.AutoSize  = true;
            lblErr.Font      = new Font("맑은 고딕", 8.5F);
            lblErr.ForeColor = Color.FromArgb(220, 60, 60);
            lblErr.Location  = new Point(28, 252);
            lblErr.Name      = "lblErr";
            lblErr.TabIndex  = 6;
            //
            // btnLogin
            //
            btnLogin.BackColor = Color.FromArgb(82, 130, 255);
            btnLogin.Cursor    = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font      = new Font("맑은 고딕", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location  = new Point(28, 278);
            btnLogin.Name      = "btnLogin";
            btnLogin.Size      = new Size(264, 46);
            btnLogin.TabIndex  = 7;
            btnLogin.Text      = "로그인";
            btnLogin.UseVisualStyleBackColor = false;
            //
            // btnTheme
            //
            btnTheme.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTheme.Cursor = Cursors.Hand;
            btnTheme.FlatAppearance.BorderSize = 0;
            btnTheme.FlatStyle = FlatStyle.Flat;
            btnTheme.Font      = new Font("Segoe UI Emoji", 14F);
            btnTheme.Location  = new Point(366, 10);
            btnTheme.Name      = "btnTheme";
            btnTheme.Size      = new Size(36, 36);
            btnTheme.TabIndex  = 0;
            btnTheme.Text      = "🌙";
            btnTheme.UseVisualStyleBackColor = false;

            // ──────────────────────────────────────────────────────
            //  도구상자 컴포넌트 (Designer 컴포넌트 트레이에 표시)
            // ──────────────────────────────────────────────────────
            //
            // toolTip
            //
            toolTip.AutoPopDelay = 8000;
            toolTip.InitialDelay = 400;
            toolTip.ReshowDelay  = 100;
            toolTip.ShowAlways   = true;
            toolTip.SetToolTip(txtId,    "학번 8자리를 입력하세요");
            toolTip.SetToolTip(txtPw,    "비밀번호를 입력 후 Enter");
            toolTip.SetToolTip(btnLogin, "로그인하여 메인 화면으로 이동");
            toolTip.SetToolTip(btnTheme, "다크/라이트 테마 전환");
            //
            // errorProvider
            //
            errorProvider.BlinkStyle       = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // helpProvider
            //
            helpProvider.SetHelpString(txtId, "학번 (예: 2025xxxxxx)");
            helpProvider.SetHelpString(txtPw, "비밀번호");
            //
            // imageList
            //
            imageList.ColorDepth       = ColorDepth.Depth32Bit;
            imageList.ImageSize        = new Size(16, 16);
            imageList.TransparentColor = Color.Transparent;
            //
            // timer  (예: 자동 입력 검증/애니메이션용)
            //
            timer.Interval = 1000;
            //
            // notifyIcon
            //
            notifyIcon.Text = "광운대 AI TODO";
            notifyIcon.Visible = false;
            //
            // ctxMenu
            //
            ctxAbout.Name = "ctxAbout";
            ctxAbout.Size = new Size(180, 22);
            ctxAbout.Text = "프로그램 정보";
            ctxExit.Name = "ctxExit";
            ctxExit.Size = new Size(180, 22);
            ctxExit.Text = "종료";
            ctxMenu.Items.AddRange(new ToolStripItem[] { ctxAbout, ctxExit });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.Size = new Size(181, 48);
            //
            // fontDialog / colorDialog / openFileDialog / saveFileDialog / folderBrowserDialog
            //   (디자이너에서 추가만 해 두고 코드에서 필요할 때 호출)
            //
            fontDialog.Font           = new Font("맑은 고딕", 10F);
            colorDialog.AnyColor      = true;
            colorDialog.FullOpen      = true;
            openFileDialog.Filter     = "텍스트 파일|*.txt|모든 파일|*.*";
            saveFileDialog.Filter     = "텍스트 파일|*.txt|모든 파일|*.*";
            folderBrowserDialog.Description = "폴더를 선택하세요";
            //
            // bindingSource / backgroundWorker
            //
            backgroundWorker.WorkerSupportsCancellation = true;

            //
            // LoginForm
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode       = AutoScaleMode.Dpi;
            ClientSize          = new Size(420, 520);
            Controls.Add(btnTheme);
            Controls.Add(pnlCard);
            DoubleBuffered      = true;
            Font                = new Font("맑은 고딕", 9.5F);
            FormBorderStyle     = FormBorderStyle.FixedSingle;
            MaximizeBox         = false;
            Name                = "LoginForm";
            StartPosition       = FormStartPosition.CenterScreen;
            Text                = "광운대 AI TODO";

            pnlCard.ResumeLayout(false);
            pnlCard.PerformLayout();
            ctxMenu.ResumeLayout(false);
            ((ISupportInitialize)errorProvider).EndInit();
            ((ISupportInitialize)bindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
