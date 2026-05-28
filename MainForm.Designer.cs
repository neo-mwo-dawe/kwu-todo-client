// =============================================================
//  MainForm.Designer.cs — Visual Studio 디자이너 자동 생성 영역
//  · 도구상자에서 컨트롤을 추가하거나 속성 편집 시 이 파일이 자동 갱신됨
//  · 동작/이벤트는 MainForm.cs 에 작성
//  · 캘린더 셀과 TODO 카드는 런타임에 동적 생성하므로 여기엔 없음
// =============================================================
// [신규] 원본 MainForm.cs 가 단일 클래스로 InitUI() 했던 것을
//        partial 클래스 + Designer.cs 패턴으로 분리한 결과 파일.
//        flpSchedule(오늘의 일정 패널), 도구상자 컴포넌트 다수 포함.

namespace KwuTodoAI
{
    partial class MainForm
    {
        /// <summary>필수 디자이너 변수입니다.</summary>
        private System.ComponentModel.IContainer components = null!;

        // ── 헤더 ───────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader = null!;
        private System.Windows.Forms.Label lblTitle  = null!;
        private System.Windows.Forms.Label lblDate   = null!;
        private System.Windows.Forms.Button btnTheme = null!;

        // ── 본문 컨테이너 ──────────────────────────────────────────
        private System.Windows.Forms.Panel pnlBody = null!;
        private System.Windows.Forms.Panel pnlLeft = null!;
        private System.Windows.Forms.Panel pnlSep  = null!;
        private System.Windows.Forms.Panel pnlRight = null!;

        // ── 왼쪽: 캘린더 & 오늘의 일정 ─────────────────────────────
        private System.Windows.Forms.Label  lblMonth      = null!;
        private System.Windows.Forms.Button btnPrev       = null!;
        private System.Windows.Forms.Button btnNext       = null!;
        private System.Windows.Forms.Panel  pnlCalendar   = null!;
        private System.Windows.Forms.Label  lblSchedTitle = null!;
        private System.Windows.Forms.FlowLayoutPanel flpSchedule = null!;

        // ── 오른쪽: 통계 & TODO 리스트 ─────────────────────────────
        private System.Windows.Forms.Panel  pnlTodoTop   = null!;
        private System.Windows.Forms.Label  lblTodoTitle = null!;
        private System.Windows.Forms.Button btnAdd       = null!;
        private System.Windows.Forms.Button btnRefresh   = null!;
        private System.Windows.Forms.Panel  pnlStats     = null!;
        private System.Windows.Forms.Label  lblSummary   = null!;
        private System.Windows.Forms.Panel  pnlLoading   = null!;
        private System.Windows.Forms.Label  lblLoading   = null!;
        private System.Windows.Forms.FlowLayoutPanel flpTodos = null!;

        // ── 도구상자 비주얼 컴포넌트 트레이 (디자이너 컴포넌트 트레이에 표시) ──
        private System.Windows.Forms.ToolTip              toolTip              = null!;
        private System.Windows.Forms.ErrorProvider        errorProvider        = null!;
        private System.Windows.Forms.HelpProvider         helpProvider         = null!;
        private System.Windows.Forms.ImageList            imageList            = null!;
        private System.Windows.Forms.Timer                timer                = null!;
        private System.Windows.Forms.NotifyIcon           notifyIcon           = null!;
        private System.Windows.Forms.ContextMenuStrip     ctxTodo              = null!;
        private System.Windows.Forms.ToolStripMenuItem    ctxEdit              = null!;
        private System.Windows.Forms.ToolStripMenuItem    ctxDelete            = null!;
        private System.Windows.Forms.ToolStripMenuItem    ctxMarkDone          = null!;
        private System.Windows.Forms.BindingSource        bindingSource        = null!;
        private System.ComponentModel.BackgroundWorker    backgroundWorker     = null!;
        private System.Windows.Forms.ColorDialog          colorDialog          = null!;
        private System.Windows.Forms.FontDialog           fontDialog           = null!;
        private System.Windows.Forms.OpenFileDialog       openFileDialog       = null!;
        private System.Windows.Forms.SaveFileDialog       saveFileDialog       = null!;
        private System.Windows.Forms.FolderBrowserDialog  folderBrowserDialog  = null!;

        /// <summary>사용 중인 모든 리소스를 정리합니다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _http?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 디자이너 자동 생성 코드

        /// <summary>
        ///  디자이너 지원에 필요한 메서드입니다.
        ///  이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlHeader     = new System.Windows.Forms.Panel();
            this.lblTitle      = new System.Windows.Forms.Label();
            this.lblDate       = new System.Windows.Forms.Label();
            this.btnTheme      = new System.Windows.Forms.Button();
            this.pnlBody       = new System.Windows.Forms.Panel();
            this.pnlLeft       = new System.Windows.Forms.Panel();
            this.pnlSep        = new System.Windows.Forms.Panel();
            this.pnlRight      = new System.Windows.Forms.Panel();
            this.lblMonth      = new System.Windows.Forms.Label();
            this.btnPrev       = new System.Windows.Forms.Button();
            this.btnNext       = new System.Windows.Forms.Button();
            this.pnlCalendar   = new System.Windows.Forms.Panel();
            this.lblSchedTitle = new System.Windows.Forms.Label();
            this.flpSchedule   = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlTodoTop    = new System.Windows.Forms.Panel();
            this.lblTodoTitle  = new System.Windows.Forms.Label();
            this.btnAdd        = new System.Windows.Forms.Button();
            this.btnRefresh    = new System.Windows.Forms.Button();
            this.pnlStats      = new System.Windows.Forms.Panel();
            this.lblSummary    = new System.Windows.Forms.Label();
            this.pnlLoading    = new System.Windows.Forms.Panel();
            this.lblLoading    = new System.Windows.Forms.Label();
            this.flpTodos      = new System.Windows.Forms.FlowLayoutPanel();

            // ── 도구상자 컴포넌트 인스턴스화 ─────────────────────
            this.toolTip             = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider       = new System.Windows.Forms.ErrorProvider(this.components);
            this.helpProvider        = new System.Windows.Forms.HelpProvider();
            this.imageList           = new System.Windows.Forms.ImageList(this.components);
            this.timer               = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon          = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxTodo             = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxEdit             = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDelete           = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMarkDone         = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource       = new System.Windows.Forms.BindingSource(this.components);
            this.backgroundWorker    = new System.ComponentModel.BackgroundWorker();
            this.colorDialog         = new System.Windows.Forms.ColorDialog();
            this.fontDialog          = new System.Windows.Forms.FontDialog();
            this.openFileDialog      = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog      = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            this.ctxTodo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.bindingSource).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlTodoTop.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            this.SuspendLayout();

            // ── lblTitle ──────────────────────────────────────────
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font     = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(18, 7);
            this.lblTitle.Name     = "lblTitle";
            this.lblTitle.Text     = "🎓 광운대 AI TODO";

            // ── lblDate ───────────────────────────────────────────
            this.lblDate.AutoSize = true;
            this.lblDate.Font     = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblDate.Location = new System.Drawing.Point(20, 35);
            this.lblDate.Name     = "lblDate";
            this.lblDate.Text     = "";

            // ── btnTheme ──────────────────────────────────────────
            this.btnTheme.Anchor    = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnTheme.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheme.FlatAppearance.BorderSize = 0;
            this.btnTheme.Font      = new System.Drawing.Font("Segoe UI Emoji", 14F);
            this.btnTheme.Location  = new System.Drawing.Point(1034, 11);
            this.btnTheme.Name      = "btnTheme";
            this.btnTheme.Size      = new System.Drawing.Size(36, 36);
            this.btnTheme.Text      = "🌙";
            this.btnTheme.UseVisualStyleBackColor = false;

            // ── pnlHeader ─────────────────────────────────────────
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblDate);
            this.pnlHeader.Controls.Add(this.btnTheme);
            this.pnlHeader.Dock   = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 58;
            this.pnlHeader.Name   = "pnlHeader";

            // ── lblMonth ──────────────────────────────────────────
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font     = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblMonth.Location = new System.Drawing.Point(14, 12);
            this.lblMonth.Name     = "lblMonth";
            this.lblMonth.Text     = "";

            // ── btnPrev / btnNext (이전·다음 달) ──────────────────
            // 화살표(< >) 글자 잘림 방지 — 폰트는 Segoe UI Symbol, AutoEllipsis off
            this.btnPrev.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.Font      = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPrev.Location  = new System.Drawing.Point(218, 8);
            this.btnPrev.Name      = "btnPrev";
            this.btnPrev.Padding   = new System.Windows.Forms.Padding(0);
            this.btnPrev.Size      = new System.Drawing.Size(32, 32);
            this.btnPrev.Text      = "<";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrev.UseVisualStyleBackColor = false;

            this.btnNext.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.Font      = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnNext.Location  = new System.Drawing.Point(254, 8);
            this.btnNext.Name      = "btnNext";
            this.btnNext.Padding   = new System.Windows.Forms.Padding(0);
            this.btnNext.Size      = new System.Drawing.Size(32, 32);
            this.btnNext.Text      = ">";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNext.UseVisualStyleBackColor = false;

            // ── pnlCalendar ───────────────────────────────────────
            this.pnlCalendar.Location = new System.Drawing.Point(10, 50);
            this.pnlCalendar.Name     = "pnlCalendar";
            this.pnlCalendar.Size     = new System.Drawing.Size(282, 270);

            // ── lblSchedTitle ─────────────────────────────────────
            this.lblSchedTitle.AutoSize = true;
            this.lblSchedTitle.Font     = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSchedTitle.Location = new System.Drawing.Point(10, 332);
            this.lblSchedTitle.Name     = "lblSchedTitle";
            this.lblSchedTitle.Text     = "📅 오늘의 일정";

            // ── flpSchedule (선택 날짜의 일정 목록) ───────────────
            this.flpSchedule.AutoScroll    = true;
            this.flpSchedule.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSchedule.Location      = new System.Drawing.Point(10, 358);
            this.flpSchedule.Name          = "flpSchedule";
            this.flpSchedule.Size          = new System.Drawing.Size(282, 220);
            this.flpSchedule.WrapContents  = false;

            // ── pnlLeft ───────────────────────────────────────────
            this.pnlLeft.Controls.Add(this.lblMonth);
            this.pnlLeft.Controls.Add(this.btnPrev);
            this.pnlLeft.Controls.Add(this.btnNext);
            this.pnlLeft.Controls.Add(this.pnlCalendar);
            this.pnlLeft.Controls.Add(this.lblSchedTitle);
            this.pnlLeft.Controls.Add(this.flpSchedule);
            this.pnlLeft.Dock    = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Name    = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(14);
            this.pnlLeft.Width   = 310;

            // ── pnlSep ────────────────────────────────────────────
            this.pnlSep.Dock  = System.Windows.Forms.DockStyle.Left;
            this.pnlSep.Name  = "pnlSep";
            this.pnlSep.Width = 1;

            // ── lblTodoTitle ──────────────────────────────────────
            this.lblTodoTitle.AutoSize = true;
            this.lblTodoTitle.Font     = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTodoTitle.Location = new System.Drawing.Point(0, 10);
            this.lblTodoTitle.Name     = "lblTodoTitle";
            this.lblTodoTitle.Text     = "AI TODO LIST";

            // ── btnAdd ────────────────────────────────────────────
            this.btnAdd.Anchor    = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(55, 175, 115);
            this.btnAdd.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Font      = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location  = new System.Drawing.Point(534, 6);
            this.btnAdd.Name      = "btnAdd";
            this.btnAdd.Size      = new System.Drawing.Size(80, 34);
            this.btnAdd.Text      = "➕ 추가";
            this.btnAdd.UseVisualStyleBackColor = false;

            // ── btnRefresh ────────────────────────────────────────
            this.btnRefresh.Anchor    = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(82, 130, 255);
            this.btnRefresh.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Font      = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location  = new System.Drawing.Point(620, 6);
            this.btnRefresh.Name      = "btnRefresh";
            this.btnRefresh.Size      = new System.Drawing.Size(104, 34);
            this.btnRefresh.Text      = "🔄 AI 생성";
            this.btnRefresh.UseVisualStyleBackColor = false;

            // ── pnlTodoTop ────────────────────────────────────────
            this.pnlTodoTop.Controls.Add(this.lblTodoTitle);
            this.pnlTodoTop.Controls.Add(this.btnAdd);
            this.pnlTodoTop.Controls.Add(this.btnRefresh);
            this.pnlTodoTop.Dock   = System.Windows.Forms.DockStyle.Top;
            this.pnlTodoTop.Height = 46;
            this.pnlTodoTop.Name   = "pnlTodoTop";

            // ── lblSummary ────────────────────────────────────────
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font     = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblSummary.Location = new System.Drawing.Point(2, 8);
            this.lblSummary.Name     = "lblSummary";
            this.lblSummary.Text     = "🔄 AI 생성 버튼을 눌러 TODO를 불러오세요";

            // ── pnlStats ──────────────────────────────────────────
            this.pnlStats.Controls.Add(this.lblSummary);
            this.pnlStats.Dock   = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Height = 32;
            this.pnlStats.Name   = "pnlStats";

            // ── lblLoading ────────────────────────────────────────
            this.lblLoading.AutoSize = true;
            this.lblLoading.Font     = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblLoading.Location = new System.Drawing.Point(0, 0);
            this.lblLoading.Name     = "lblLoading";
            this.lblLoading.Text     = "🤖  AI가 학사 일정을 분석 중입니다...";

            // ── pnlLoading ────────────────────────────────────────
            this.pnlLoading.Controls.Add(this.lblLoading);
            this.pnlLoading.Dock    = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoading.Name    = "pnlLoading";
            this.pnlLoading.Visible = false;

            // ── flpTodos ──────────────────────────────────────────
            this.flpTodos.AutoScroll    = true;
            this.flpTodos.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.flpTodos.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTodos.Name          = "flpTodos";
            this.flpTodos.Padding       = new System.Windows.Forms.Padding(12, 2, 12, 2);
            this.flpTodos.WrapContents  = false;

            // ── pnlRight ──────────────────────────────────────────
            // Dock 겹침 방지: Fill을 먼저 추가하고 Top을 나중에 추가
            this.pnlRight.Controls.Add(this.flpTodos);
            this.pnlRight.Controls.Add(this.pnlLoading);
            this.pnlRight.Controls.Add(this.pnlStats);
            this.pnlRight.Controls.Add(this.pnlTodoTop);
            this.pnlRight.Dock    = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Name    = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(14, 6, 14, 6);

            // ── pnlBody ───────────────────────────────────────────
            // Fill 패널을 가장 먼저 추가 → Left → 본문 영역이 안정적으로 나뉨
            this.pnlBody.Controls.Add(this.pnlRight);
            this.pnlBody.Controls.Add(this.pnlSep);
            this.pnlBody.Controls.Add(this.pnlLeft);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Name = "pnlBody";

            // ──────────────────────────────────────────────────────
            //  도구상자 컴포넌트 (디자이너 컴포넌트 트레이에 표시)
            // ──────────────────────────────────────────────────────
            this.toolTip.AutoPopDelay = 8000;
            this.toolTip.InitialDelay = 400;
            this.toolTip.ReshowDelay  = 100;
            this.toolTip.ShowAlways   = true;
            this.toolTip.SetToolTip(this.btnPrev,    "이전 달");
            this.toolTip.SetToolTip(this.btnNext,    "다음 달");
            this.toolTip.SetToolTip(this.btnAdd,     "새 TODO 추가");
            this.toolTip.SetToolTip(this.btnRefresh, "AI로 TODO 자동 생성");
            this.toolTip.SetToolTip(this.btnTheme,   "다크/라이트 테마 전환");

            this.errorProvider.BlinkStyle       = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;

            this.imageList.ColorDepth       = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize        = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;

            this.timer.Interval = 60000; // 1분마다 호출 가능 (자동 새로고침 용도로 활용)

            this.notifyIcon.Text    = "광운대 AI TODO";
            this.notifyIcon.Visible = false;

            // 컨텍스트 메뉴 (투두 카드 우클릭 시 사용 가능)
            this.ctxEdit.Name = "ctxEdit";
            this.ctxEdit.Size = new System.Drawing.Size(180, 22);
            this.ctxEdit.Text = "수정";
            this.ctxDelete.Name = "ctxDelete";
            this.ctxDelete.Size = new System.Drawing.Size(180, 22);
            this.ctxDelete.Text = "삭제";
            this.ctxMarkDone.Name = "ctxMarkDone";
            this.ctxMarkDone.Size = new System.Drawing.Size(180, 22);
            this.ctxMarkDone.Text = "완료 표시";
            this.ctxTodo.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.ctxEdit, this.ctxMarkDone, new System.Windows.Forms.ToolStripSeparator(), this.ctxDelete,
            });
            this.ctxTodo.Name = "ctxTodo";
            this.ctxTodo.Size = new System.Drawing.Size(181, 76);

            this.colorDialog.AnyColor = true;
            this.colorDialog.FullOpen = true;
            this.fontDialog.Font      = new System.Drawing.Font("맑은 고딕", 10F);
            this.openFileDialog.Filter = "JSON 파일|*.json|모든 파일|*.*";
            this.saveFileDialog.Filter = "JSON 파일|*.json|모든 파일|*.*";
            this.folderBrowserDialog.Description = "내보낼 폴더를 선택하세요";

            this.backgroundWorker.WorkerReportsProgress      = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;

            // ── MainForm ──────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize          = new System.Drawing.Size(1080, 700);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Font           = new System.Drawing.Font("맑은 고딕", 9.5F);
            this.MinimumSize    = new System.Drawing.Size(880, 580);
            this.Name           = "MainForm";
            this.StartPosition  = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text           = "광운대 AI TODO";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlTodoTop.ResumeLayout(false);
            this.pnlTodoTop.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlStats.PerformLayout();
            this.pnlLoading.ResumeLayout(false);
            this.pnlLoading.PerformLayout();
            this.ctxTodo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.bindingSource).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
