// =============================================================
//  EditTodoForm.Designer.cs — Visual Studio 디자이너 자동 생성 영역
//  · 도구상자에서 컨트롤을 추가하거나 속성 편집 시 이 파일이 자동 갱신됨
//  · 동작/이벤트는 EditTodoForm.cs 에 작성
// =============================================================
// [신규] 원본 TodoForm 을 Add/Edit 두 폼으로 분리하면서 새로 생성된 디자이너 파일.
//        AddTodoForm.Designer.cs 와 동일 레이아웃 (저장/추가 버튼 텍스트만 다름).

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KwuTodoAI
{
    partial class EditTodoForm
    {
        private IContainer components = null!;

        // ── 컨트롤 ─────────────────────────────────────────────────
        private Label          lblHeader   = null!;
        private Label          lblTitleCap = null!;
        private TextBox        txtTitle    = null!;
        private Label          lblDueCap   = null!;
        private DateTimePicker dtpDueDate  = null!;
        private CheckBox       chkNoDue    = null!;
        private Label          lblPriCap   = null!;
        private RadioButton    rdoHigh     = null!;
        private RadioButton    rdoMedium   = null!;
        private RadioButton    rdoLow      = null!;
        private Label          lblCatCap   = null!;
        private ComboBox       cboCategory = null!;
        private Label          lblTeamCap  = null!;
        private CheckBox       chkTeamWork = null!;
        private Button         btnSave     = null!;
        private Button         btnCancel   = null!;
        private Label          lblStatus   = null!;

        // ── 도구상자 컴포넌트 트레이 ───────────────────────────────
        private ToolTip            toolTip            = null!;
        private ErrorProvider      errorProvider      = null!;
        private HelpProvider       helpProvider       = null!;
        private ImageList          imageList          = null!;
        private System.Windows.Forms.Timer timer      = null!;
        private ContextMenuStrip   ctxMenu            = null!;
        private ToolStripMenuItem  ctxRevert          = null!;
        private ColorDialog        colorDialog        = null!;
        private FontDialog         fontDialog         = null!;
        private BindingSource      bindingSource      = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        #region 디자이너 자동 생성 코드

        private void InitializeComponent()
        {
            components  = new Container();

            lblHeader   = new Label();
            lblTitleCap = new Label();
            txtTitle    = new TextBox();
            lblDueCap   = new Label();
            dtpDueDate  = new DateTimePicker();
            chkNoDue    = new CheckBox();
            lblPriCap   = new Label();
            rdoHigh     = new RadioButton();
            rdoMedium   = new RadioButton();
            rdoLow      = new RadioButton();
            lblCatCap   = new Label();
            cboCategory = new ComboBox();
            lblTeamCap  = new Label();
            chkTeamWork = new CheckBox();
            btnSave     = new Button();
            btnCancel   = new Button();
            lblStatus   = new Label();

            toolTip       = new ToolTip(components);
            errorProvider = new ErrorProvider(components);
            helpProvider  = new HelpProvider();
            imageList     = new ImageList(components);
            timer         = new System.Windows.Forms.Timer(components);
            ctxMenu       = new ContextMenuStrip(components);
            ctxRevert     = new ToolStripMenuItem();
            colorDialog   = new ColorDialog();
            fontDialog    = new FontDialog();
            bindingSource = new BindingSource(components);

            ctxMenu.SuspendLayout();
            ((ISupportInitialize)errorProvider).BeginInit();
            ((ISupportInitialize)bindingSource).BeginInit();
            SuspendLayout();

            // ── lblHeader ─────────────────────────────────────────
            lblHeader.AutoSize  = true;
            lblHeader.Font      = new Font("맑은 고딕", 17F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(82, 130, 255);
            lblHeader.Location  = new Point(22, 18);
            lblHeader.Name      = "lblHeader";
            lblHeader.Text      = "TODO 편집";   // [변경] AddTodoForm 과 헤더 텍스트 차별화

            // ── lblTitleCap ───────────────────────────────────────
            lblTitleCap.AutoSize = true;
            lblTitleCap.Font     = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblTitleCap.Location = new Point(24, 70);
            lblTitleCap.Name     = "lblTitleCap";
            lblTitleCap.Text     = "제목";

            // ── txtTitle ──────────────────────────────────────────
            txtTitle.BorderStyle     = BorderStyle.FixedSingle;
            txtTitle.Font            = new Font("맑은 고딕", 10F);
            txtTitle.Location        = new Point(24, 92);
            txtTitle.MaxLength       = 200;
            txtTitle.Name            = "txtTitle";
            txtTitle.PlaceholderText = "할 일을 입력하세요";
            txtTitle.Size            = new Size(326, 30);
            txtTitle.ContextMenuStrip = ctxMenu;

            // ── lblDueCap / dtpDueDate / chkNoDue ─────────────────
            lblDueCap.AutoSize = true;
            lblDueCap.Font     = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblDueCap.Location = new Point(24, 138);
            lblDueCap.Name     = "lblDueCap";
            lblDueCap.Text     = "마감일";

            dtpDueDate.Font         = new Font("맑은 고딕", 10F);
            dtpDueDate.Format       = DateTimePickerFormat.Custom;
            dtpDueDate.CustomFormat = "yyyy-MM-dd";
            dtpDueDate.Location     = new Point(24, 160);
            dtpDueDate.Name         = "dtpDueDate";
            dtpDueDate.Size         = new Size(230, 30);
            dtpDueDate.Value        = System.DateTime.Today.AddDays(7);

            chkNoDue.AutoSize  = true;
            chkNoDue.Font      = new Font("맑은 고딕", 9F);
            chkNoDue.ForeColor = Color.FromArgb(115, 126, 162);
            chkNoDue.Location  = new Point(262, 165);
            chkNoDue.Name      = "chkNoDue";
            chkNoDue.Text      = "없음";

            // ── lblPriCap / rdoHigh / rdoMedium / rdoLow ──────────
            lblPriCap.AutoSize = true;
            lblPriCap.Font     = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblPriCap.Location = new Point(24, 206);
            lblPriCap.Name     = "lblPriCap";
            lblPriCap.Text     = "우선순위";

            rdoHigh.AutoSize = true;
            rdoHigh.Font     = new Font("맑은 고딕", 9.5F);
            rdoHigh.Location = new Point(24, 230);
            rdoHigh.Name     = "rdoHigh";
            rdoHigh.Text     = "높음";

            rdoMedium.AutoSize = true;
            rdoMedium.Checked  = true;
            rdoMedium.Font     = new Font("맑은 고딕", 9.5F);
            rdoMedium.Location = new Point(98, 230);
            rdoMedium.Name     = "rdoMedium";
            rdoMedium.Text     = "보통";

            rdoLow.AutoSize = true;
            rdoLow.Font     = new Font("맑은 고딕", 9.5F);
            rdoLow.Location = new Point(172, 230);
            rdoLow.Name     = "rdoLow";
            rdoLow.Text     = "낮음";

            // ── lblCatCap / cboCategory ───────────────────────────
            lblCatCap.AutoSize = true;
            lblCatCap.Font     = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblCatCap.Location = new Point(24, 270);
            lblCatCap.Name     = "lblCatCap";
            lblCatCap.Text     = "카테고리";

            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Font          = new Font("맑은 고딕", 9.5F);
            cboCategory.Items.AddRange(new object[] { "학업", "행정", "장학", "기타" });
            cboCategory.SelectedIndex = 0;
            cboCategory.Location      = new Point(24, 292);
            cboCategory.Name          = "cboCategory";
            cboCategory.Size          = new Size(326, 28);

            // ── lblTeamCap / chkTeamWork ──────────────────────────
            lblTeamCap.AutoSize = true;
            lblTeamCap.Font     = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblTeamCap.Location = new Point(24, 336);
            lblTeamCap.Name     = "lblTeamCap";
            lblTeamCap.Text     = "팀 과제 여부";

            chkTeamWork.AutoSize = true;
            chkTeamWork.Font     = new Font("맑은 고딕", 9.5F);
            chkTeamWork.Location = new Point(24, 360);
            chkTeamWork.Name     = "chkTeamWork";
            chkTeamWork.Text     = "팀 과제입니다 👥";

            // ── lblStatus ─────────────────────────────────────────
            lblStatus.AutoSize  = false;
            lblStatus.Font      = new Font("맑은 고딕", 9F);
            lblStatus.ForeColor = Color.FromArgb(220, 60, 60);
            lblStatus.Location  = new Point(24, 396);
            lblStatus.Name      = "lblStatus";
            lblStatus.Size      = new Size(326, 22);

            // ── btnSave (저장) ────────────────────────────────────
            btnSave.BackColor = Color.FromArgb(82, 130, 255);
            btnSave.Cursor    = Cursors.Hand;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font      = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location  = new Point(50, 430);
            btnSave.Name      = "btnSave";
            btnSave.Size      = new Size(120, 40);
            btnSave.Text      = "저장";
            btnSave.UseVisualStyleBackColor = false;

            // ── btnCancel ─────────────────────────────────────────
            btnCancel.BackColor = Color.White;
            btnCancel.Cursor    = Cursors.Hand;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 207, 230);
            btnCancel.FlatAppearance.BorderSize  = 1;
            btnCancel.Font      = new Font("맑은 고딕", 10F);
            btnCancel.ForeColor = Color.FromArgb(60, 70, 100);
            btnCancel.Location  = new Point(204, 430);
            btnCancel.Name      = "btnCancel";
            btnCancel.Size      = new Size(120, 40);
            btnCancel.Text      = "취소";
            btnCancel.UseVisualStyleBackColor = false;

            // ──────────────────────────────────────────────────────
            //  도구상자 컴포넌트
            // ──────────────────────────────────────────────────────
            toolTip.AutoPopDelay = 8000;
            toolTip.InitialDelay = 400;
            toolTip.ReshowDelay  = 100;
            toolTip.ShowAlways   = true;
            toolTip.SetToolTip(txtTitle,    "할 일의 제목을 수정하세요 (최대 200자)");
            toolTip.SetToolTip(dtpDueDate,  "마감일을 변경하세요");
            toolTip.SetToolTip(chkNoDue,    "마감일을 해제하려면 체크");
            toolTip.SetToolTip(rdoHigh,     "긴급/중요한 일");
            toolTip.SetToolTip(rdoMedium,   "기본 우선순위");
            toolTip.SetToolTip(rdoLow,      "여유 있는 일");
            toolTip.SetToolTip(chkTeamWork, "팀 과제는 카드에 👥 아이콘으로 표시됩니다");
            toolTip.SetToolTip(btnSave,     "변경 사항을 서버에 저장합니다");
            toolTip.SetToolTip(btnCancel,   "저장하지 않고 닫습니다");

            errorProvider.BlinkStyle       = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;

            helpProvider.SetHelpString(txtTitle, "할 일의 제목");

            imageList.ColorDepth       = ColorDepth.Depth32Bit;
            imageList.ImageSize        = new Size(16, 16);
            imageList.TransparentColor = Color.Transparent;

            timer.Interval = 500;

            ctxRevert.Name = "ctxRevert";
            ctxRevert.Size = new Size(160, 22);
            ctxRevert.Text = "원래 값으로 되돌리기";
            ctxMenu.Items.AddRange(new ToolStripItem[] { ctxRevert });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.Size = new Size(161, 26);

            colorDialog.AnyColor = true;
            fontDialog.Font      = new Font("맑은 고딕", 10F);

            // ──────────────────────────────────────────────────────
            //  EditTodoForm
            // ──────────────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode       = AutoScaleMode.Dpi;
            BackColor           = Color.White;
            ClientSize          = new Size(376, 490);
            Controls.Add(lblHeader);
            Controls.Add(lblTitleCap);
            Controls.Add(txtTitle);
            Controls.Add(lblDueCap);
            Controls.Add(dtpDueDate);
            Controls.Add(chkNoDue);
            Controls.Add(lblPriCap);
            Controls.Add(rdoHigh);
            Controls.Add(rdoMedium);
            Controls.Add(rdoLow);
            Controls.Add(lblCatCap);
            Controls.Add(cboCategory);
            Controls.Add(lblTeamCap);
            Controls.Add(chkTeamWork);
            Controls.Add(lblStatus);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Font            = new Font("맑은 고딕", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            Name            = "EditTodoForm";
            StartPosition   = FormStartPosition.CenterParent;
            Text            = "TODO 편집";

            ctxMenu.ResumeLayout(false);
            ((ISupportInitialize)errorProvider).EndInit();
            ((ISupportInitialize)bindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
