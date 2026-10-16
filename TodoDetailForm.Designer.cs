// =============================================================
//  TodoDetailForm.Designer.cs — Visual Studio 디자이너 자동 생성 영역
//  · 도구상자에서 컨트롤을 추가하거나 속성 편집 시 이 파일이 자동 갱신됨
//  · 동작/이벤트는 TodoDetailForm.cs 에 작성
// =============================================================
// [신규] 원본 MainForm.ShowDetail() 코드형 임시 Form 을 별도 디자이너 폼으로 분리.
//        헤더(배지·제목·D-Day) + 메타 패널 + 본문 GroupBox + 푸터(저장/닫기) 구조.

namespace KwuTodoAI
{
    partial class TodoDetailForm
    {
        private System.ComponentModel.IContainer components = null!;

        // ── 컨테이너 패널 ──────────────────────────────────────────
        private System.Windows.Forms.Panel    pnlHead     = null!;
        private System.Windows.Forms.Panel    pnlMeta     = null!;
        private System.Windows.Forms.Panel    pnlBodyArea = null!;
        private System.Windows.Forms.Panel    pnlFoot     = null!;
        private System.Windows.Forms.GroupBox gbxBody     = null!;

        // ── 헤더 영역 컨트롤 ──────────────────────────────────────
        private System.Windows.Forms.Label    lblPriBadge = null!;
        private System.Windows.Forms.Label    lblDDay     = null!;
        private System.Windows.Forms.TextBox  txtTitle    = null!;

        // ── 메타 정보 ──────────────────────────────────────────────
        private System.Windows.Forms.Label    lblMeta     = null!;

        // ── 본문 ───────────────────────────────────────────────────
        private System.Windows.Forms.TextBox  txtBody     = null!;
        private System.Windows.Forms.Button   btnSave     = null!;
        private System.Windows.Forms.Button   btnClose    = null!;

        // ── 도구상자 컴포넌트 (비주얼 디자이너에서 추가 가능) ─────
        private System.Windows.Forms.ToolTip       toolTip       = null!;
        private System.Windows.Forms.ErrorProvider errorProvider = null!;
        private System.Windows.Forms.HelpProvider  helpProvider  = null!;
        private System.Windows.Forms.ImageList     imageList     = null!;
        private System.Windows.Forms.ContextMenuStrip ctxBody    = null!;
        private System.Windows.Forms.ToolStripMenuItem ctxCopy   = null!;
        private System.Windows.Forms.ToolStripMenuItem ctxPaste  = null!;
        private System.Windows.Forms.ToolStripMenuItem ctxClear  = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        #region 디자이너 자동 생성 코드

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHead = new Panel();
            lblPriBadge = new Label();
            lblDDay = new Label();
            txtTitle = new TextBox();
            pnlMeta = new Panel();
            lblMeta = new Label();
            pnlBodyArea = new Panel();
            gbxBody = new GroupBox();
            txtBody = new TextBox();
            ctxBody = new ContextMenuStrip(components);
            ctxCopy = new ToolStripMenuItem();
            ctxPaste = new ToolStripMenuItem();
            ctxClear = new ToolStripMenuItem();
            pnlFoot = new Panel();
            lblStatus = new Label();
            btnSave = new Button();
            btnClose = new Button();
            toolTip = new ToolTip(components);
            errorProvider = new ErrorProvider(components);
            helpProvider = new HelpProvider();
            imageList = new ImageList(components);
            pnlHead.SuspendLayout();
            pnlMeta.SuspendLayout();
            pnlBodyArea.SuspendLayout();
            gbxBody.SuspendLayout();
            ctxBody.SuspendLayout();
            pnlFoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlHead
            // 
            pnlHead.Controls.Add(lblPriBadge);
            pnlHead.Controls.Add(lblDDay);
            pnlHead.Controls.Add(txtTitle);
            pnlHead.Dock = DockStyle.Top;
            pnlHead.Location = new Point(0, 0);
            pnlHead.Name = "pnlHead";
            pnlHead.Size = new Size(700, 108);
            pnlHead.TabIndex = 0;
            // 
            // lblPriBadge
            // 
            lblPriBadge.AutoSize = true;
            lblPriBadge.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblPriBadge.Location = new Point(20, 20);
            lblPriBadge.Name = "lblPriBadge";
            lblPriBadge.Padding = new Padding(6, 3, 6, 3);
            lblPriBadge.Size = new Size(78, 27);
            lblPriBadge.TabIndex = 0;
            lblPriBadge.Text = "  보통  ";
            // 
            // lblDDay
            // 
            lblDDay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDDay.AutoSize = true;
            lblDDay.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
            lblDDay.Location = new Point(1100, 18);
            lblDDay.Name = "lblDDay";
            lblDDay.Size = new Size(51, 30);
            lblDDay.TabIndex = 1;
            lblDDay.Text = "D-0";
            // 
            // txtTitle
            // 
            txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTitle.BorderStyle = BorderStyle.FixedSingle;
            txtTitle.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            helpProvider.SetHelpString(txtTitle, "TODO 의 제목입니다.");
            txtTitle.Location = new Point(20, 56);
            txtTitle.MaxLength = 200;
            txtTitle.Name = "txtTitle";
            helpProvider.SetShowHelp(txtTitle, true);
            txtTitle.Size = new Size(650, 39);
            txtTitle.TabIndex = 2;
            toolTip.SetToolTip(txtTitle, "TODO 제목을 편집할 수 있습니다");
            // 
            // pnlMeta
            // 
            pnlMeta.Controls.Add(lblMeta);
            pnlMeta.Dock = DockStyle.Top;
            pnlMeta.Location = new Point(0, 108);
            pnlMeta.Name = "pnlMeta";
            pnlMeta.Padding = new Padding(20, 10, 20, 10);
            pnlMeta.Size = new Size(700, 90);
            pnlMeta.TabIndex = 1;
            // 
            // lblMeta
            // 
            lblMeta.Dock = DockStyle.Fill;
            lblMeta.Font = new Font("맑은 고딕", 10F);
            lblMeta.Location = new Point(20, 10);
            lblMeta.Name = "lblMeta";
            lblMeta.Size = new Size(660, 70);
            lblMeta.TabIndex = 0;
            lblMeta.TextAlign = ContentAlignment.MiddleLeft;
            lblMeta.UseMnemonic = false;
            // 
            // pnlBodyArea
            // 
            pnlBodyArea.Controls.Add(gbxBody);
            pnlBodyArea.Dock = DockStyle.Fill;
            pnlBodyArea.Location = new Point(0, 198);
            pnlBodyArea.Name = "pnlBodyArea";
            pnlBodyArea.Padding = new Padding(20, 8, 20, 8);
            pnlBodyArea.Size = new Size(700, 276);
            pnlBodyArea.TabIndex = 2;
            // 
            // gbxBody
            // 
            gbxBody.Controls.Add(txtBody);
            gbxBody.Dock = DockStyle.Fill;
            gbxBody.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            gbxBody.Location = new Point(20, 8);
            gbxBody.Name = "gbxBody";
            gbxBody.Padding = new Padding(10, 8, 10, 8);
            gbxBody.Size = new Size(660, 260);
            gbxBody.TabIndex = 0;
            gbxBody.TabStop = false;
            gbxBody.Text = "  상세 내용 (편집 가능)  ";
            // 
            // txtBody
            // 
            txtBody.BorderStyle = BorderStyle.None;
            txtBody.ContextMenuStrip = ctxBody;
            txtBody.Dock = DockStyle.Fill;
            txtBody.Font = new Font("맑은 고딕", 10F);
            helpProvider.SetHelpString(txtBody, "이 영역에서 판단 이유와 실행 항목을 자유롭게 수정할 수 있습니다.");
            txtBody.HideSelection = false;
            txtBody.Location = new Point(10, 28);
            txtBody.Multiline = true;
            txtBody.Name = "txtBody";
            txtBody.ScrollBars = ScrollBars.Vertical;
            helpProvider.SetShowHelp(txtBody, true);
            txtBody.Size = new Size(640, 224);
            txtBody.TabIndex = 0;
            toolTip.SetToolTip(txtBody, "이유와 실행 항목을 자유롭게 편집하세요");
            // 
            // ctxBody
            // 
            ctxBody.ImageScalingSize = new Size(20, 20);
            ctxBody.Items.AddRange(new ToolStripItem[] { ctxCopy, ctxPaste, ctxClear });
            ctxBody.Name = "ctxBody";
            ctxBody.Size = new Size(253, 76);
            // 
            // ctxCopy
            // 
            ctxCopy.Name = "ctxCopy";
            ctxCopy.ShortcutKeys = Keys.Control | Keys.C;
            ctxCopy.Size = new Size(252, 24);
            ctxCopy.Text = "복사 (Ctrl+C)";
            // 
            // ctxPaste
            // 
            ctxPaste.Name = "ctxPaste";
            ctxPaste.ShortcutKeys = Keys.Control | Keys.V;
            ctxPaste.Size = new Size(252, 24);
            ctxPaste.Text = "붙여넣기 (Ctrl+V)";
            // 
            // ctxClear
            // 
            ctxClear.Name = "ctxClear";
            ctxClear.Size = new Size(252, 24);
            ctxClear.Text = "본문 비우기";
            // 
            // pnlFoot
            // 
            pnlFoot.Controls.Add(lblStatus);
            pnlFoot.Controls.Add(btnSave);
            pnlFoot.Controls.Add(btnClose);
            pnlFoot.Dock = DockStyle.Bottom;
            pnlFoot.Location = new Point(0, 474);
            pnlFoot.Name = "pnlFoot";
            pnlFoot.Padding = new Padding(20, 8, 20, 8);
            pnlFoot.Size = new Size(700, 66);
            pnlFoot.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.Font = new Font("맑은 고딕", 9F);
            lblStatus.ForeColor = Color.FromArgb(220, 60, 60);
            lblStatus.Location = new Point(23, 22);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(210, 22);
            lblStatus.TabIndex = 0;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(82, 130, 255);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(462, 11);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(110, 40);
            btnSave.TabIndex = 0;
            btnSave.Text = "저장";
            toolTip.SetToolTip(btnSave, "변경 사항을 서버에 저장합니다");
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(82, 130, 255);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(578, 11);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 40);
            btnClose.TabIndex = 1;
            btnClose.Text = "닫기";
            toolTip.SetToolTip(btnClose, "저장하지 않고 창을 닫습니다");
            btnClose.UseVisualStyleBackColor = false;
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 8000;
            toolTip.InitialDelay = 400;
            toolTip.ReshowDelay = 100;
            toolTip.ShowAlways = true;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // imageList
            // 
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(16, 16);
            imageList.TransparentColor = Color.Transparent;
            // 
            // TodoDetailForm
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(700, 540);
            Controls.Add(pnlBodyArea);
            Controls.Add(pnlMeta);
            Controls.Add(pnlHead);
            Controls.Add(pnlFoot);
            Font = new Font("맑은 고딕", 9.5F);
            MaximizeBox = false;
            MinimumSize = new Size(560, 480);
            Name = "TodoDetailForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "TODO 상세";
            pnlHead.ResumeLayout(false);
            pnlHead.PerformLayout();
            pnlMeta.ResumeLayout(false);
            pnlBodyArea.ResumeLayout(false);
            gbxBody.ResumeLayout(false);
            gbxBody.PerformLayout();
            ctxBody.ResumeLayout(false);
            pnlFoot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblStatus;
    }
}
