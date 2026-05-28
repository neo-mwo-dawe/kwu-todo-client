using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KwuTodoAI
{
    public class MainForm : Form
    {
        private bool      _isDark;
        private DateTime  _currentMonth = DateTime.Today;
        private DateTime? _selectedDate = DateTime.Today;
        private List<TodoItem> _todos   = new();
        private readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(60) };

        private Panel           pnlHeader   = null!, pnlLeft    = null!, pnlRight  = null!;
        private Panel           pnlCalendar = null!, pnlStats   = null!, pnlTodoTop = null!, pnlLoading = null!;
        private FlowLayoutPanel flpTodos    = null!;
        private Label           lblMonth    = null!, lblTodoTitle = null!, lblSummary = null!, lblLoading = null!;
        private Button          btnPrev     = null!, btnNext = null!, btnRefresh = null!, btnTheme = null!;

        private Color BG       => _isDark ? Color.FromArgb(15, 18, 30)    : Color.FromArgb(242, 245, 255);
        private Color SURFACE  => _isDark ? Color.FromArgb(24, 28, 46)    : Color.White;
        private Color SURFACE2 => _isDark ? Color.FromArgb(32, 38, 60)    : Color.FromArgb(245, 248, 255);
        private Color ACCENT   => Color.FromArgb(82, 130, 255);
        private Color TEXT     => _isDark ? Color.FromArgb(218, 226, 255) : Color.FromArgb(25, 30, 60);
        private Color SUBTEXT  => _isDark ? Color.FromArgb(115, 130, 172) : Color.FromArgb(115, 126, 162);
        private Color URGENT   => Color.FromArgb(220, 60, 60);
        private Color HIGH_C   => Color.FromArgb(230, 130, 30);

        public MainForm(bool isDark = false)
        {
            _isDark = isDark;
            InitUI();
            ApplyTheme();
            _ = LoadTodosAsync();
        }

        private void InitUI()
        {
            Text           = "광운대 AI TODO";
            Size           = new Size(1080, 700);
            MinimumSize    = new Size(880, 580);
            StartPosition  = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            Font           = new Font("맑은 고딕", 9.5f);

            // ── 헤더 ──────────────────────────────────────────────
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 58 };
            var pnlBody = new Panel { Dock = DockStyle.Fill };

            var lblTitle = new Label { Text = "🎓 광운대 AI TODO", Font = new Font("맑은 고딕", 14f, FontStyle.Bold), AutoSize = true, Location = new Point(18, 7) };
            var lblDate  = new Label { Text = DateTime.Today.ToString("yyyy년 MM월 dd일 (ddd)"), Font = new Font("맑은 고딕", 8.5f), AutoSize = true, Location = new Point(20, 35) };

            btnTheme = MakeBtn("🌙", 36, 36);
            btnTheme.Font   = new Font("Segoe UI Emoji", 14f);
            btnTheme.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTheme.Click += (s, e) => { _isDark = !_isDark; btnTheme.Text = _isDark ? "☀️" : "🌙"; ApplyTheme(); };
            pnlHeader.Resize += (s, e) => btnTheme.Location = new Point(pnlHeader.Width - 46, 11);
            btnTheme.Location = new Point(1034, 11);

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblDate, btnTheme });

            // ── 왼쪽 캘린더 ───────────────────────────────────────
            pnlLeft = new Panel { Width = 310, Dock = DockStyle.Left, Padding = new Padding(14) };

            lblMonth = new Label { Text = _currentMonth.ToString("yyyy년 MM월"), Font = new Font("맑은 고딕", 11f, FontStyle.Bold), AutoSize = true, Location = new Point(14, 12) };

            btnPrev = MakeBtn("‹", 28, 28); btnPrev.Font = new Font("맑은 고딕", 13f, FontStyle.Bold);
            btnNext = MakeBtn("›", 28, 28); btnNext.Font = new Font("맑은 고딕", 13f, FontStyle.Bold);
            btnPrev.Click += (s, e) => { _currentMonth = _currentMonth.AddMonths(-1); RebuildCalendar(); };
            btnNext.Click += (s, e) => { _currentMonth = _currentMonth.AddMonths(1);  RebuildCalendar(); };
            pnlLeft.Resize += (s, e) => { btnNext.Location = new Point(pnlLeft.Width - 38, 10); btnPrev.Location = new Point(pnlLeft.Width - 70, 10); };
            btnNext.Location = new Point(272, 10); btnPrev.Location = new Point(240, 10);

            pnlCalendar = new Panel { Location = new Point(10, 50), Size = new Size(282, 270) };

            var lblSchedTitle = new Label { Text = "📅 이번 달 일정", Font = new Font("맑은 고딕", 9.5f, FontStyle.Bold), AutoSize = true, Location = new Point(10, 332) };

            pnlLeft.Controls.AddRange(new Control[] { lblMonth, btnPrev, btnNext, pnlCalendar, lblSchedTitle });

            var sep = new Panel { Width = 1, Dock = DockStyle.Left };

            RebuildCalendar();

            // ── 오른쪽 TODO ────────────────────────────────────────
            pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(14, 6, 14, 6) };

            pnlTodoTop = new Panel { Height = 46, Dock = DockStyle.Top };
            lblTodoTitle = new Label { Text = "AI TODO LIST", Font = new Font("맑은 고딕", 12f, FontStyle.Bold), AutoSize = true, Location = new Point(0, 10) };

            btnRefresh = MakeBtn("🔄 AI 생성", 104, 34);
            btnRefresh.Font      = new Font("맑은 고딕", 9f, FontStyle.Bold);
            btnRefresh.BackColor = ACCENT;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Anchor    = AnchorStyles.Right | AnchorStyles.Top;
            btnRefresh.Click    += async (s, e) => await LoadTodosAsync();
            pnlTodoTop.Resize   += (s, e) => btnRefresh.Location = new Point(pnlTodoTop.Width - 112, 6);
            btnRefresh.Location  = new Point(620, 6);

            var btnAdd = MakeBtn("➕ 추가", 80, 34);
            btnAdd.Font      = new Font("맑은 고딕", 9f, FontStyle.Bold);
            btnAdd.BackColor = Color.FromArgb(55, 175, 115);
            btnAdd.ForeColor = Color.White;
            btnAdd.Anchor    = AnchorStyles.Right | AnchorStyles.Top;
            btnAdd.Click    += (s, e) => OpenAddTodoForm();
            pnlTodoTop.Resize += (s, e) => btnAdd.Location = new Point(pnlTodoTop.Width - 200, 6);
            btnAdd.Location   = new Point(534, 6);

            pnlTodoTop.Controls.AddRange(new Control[] { lblTodoTitle, btnAdd, btnRefresh });

            pnlStats = new Panel { Height = 32, Dock = DockStyle.Top };
            lblSummary = new Label { Text = "🔄 AI 생성 버튼을 눌러 TODO를 불러오세요", Font = new Font("맑은 고딕", 8.5f), AutoSize = true, Location = new Point(2, 8) };
            pnlStats.Controls.Add(lblSummary);

            pnlLoading = new Panel { Dock = DockStyle.Fill, Visible = false };
            lblLoading = new Label { Text = "🤖  AI가 학사 일정을 분석 중입니다...", Font = new Font("맑은 고딕", 11f), AutoSize = true };
            pnlLoading.Controls.Add(lblLoading);
            pnlLoading.Resize += (s, e) => lblLoading.Location = new Point(
                (pnlLoading.Width  - lblLoading.PreferredWidth)  / 2,
                (pnlLoading.Height - lblLoading.PreferredHeight) / 2);

            flpTodos = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, Padding = new Padding(12, 2, 12, 2),
            };
            // 창 크기 변경 시 카드 너비 자동 조정
            flpTodos.Resize += (s, e) =>
            {
                int w = GetTodoCardWidth();
                if (w <= 50) return;
                foreach (Control c in flpTodos.Controls)
                    c.Width = w;
            };

            pnlRight.Controls.Add(flpTodos);
            pnlRight.Controls.Add(pnlLoading);
            pnlRight.Controls.Add(pnlStats);
            pnlRight.Controls.Add(pnlTodoTop);

            // Dock 겹침 방지: 헤더는 Form에, 좌/우 본문은 pnlBody 안에만 배치한다.
            // Fill 패널을 먼저 추가하고, Left 패널을 나중에 추가해야 본문 영역이 안정적으로 나뉜다.
            pnlBody.Controls.Add(pnlRight);
            pnlBody.Controls.Add(sep);
            pnlBody.Controls.Add(pnlLeft);
            Controls.Add(pnlBody);
            Controls.Add(pnlHeader);
        }

        // ── 캘린더 ────────────────────────────────────────────────
        private void RebuildCalendar()
        {
            if (lblMonth != null) lblMonth.Text = _currentMonth.ToString("yyyy년 MM월");
            pnlCalendar.Controls.Clear();

            int cellW = 38, cellH = 32;
            string[] dn = { "월", "화", "수", "목", "금", "토", "일" };

            for (int i = 0; i < 7; i++)
                pnlCalendar.Controls.Add(new Label
                {
                    Text = dn[i], Size = new Size(cellW, 20), Location = new Point(i * cellW, 0),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                    ForeColor = i >= 5 ? Color.FromArgb(215, 75, 75) : SUBTEXT,
                });

            var dueDates = _todos
                .Where(t => !string.IsNullOrEmpty(t.DueDate))
                .Select(t => { DateTime.TryParse(t.DueDate, out var d); return d; })
                .ToHashSet();

            DateTime first = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            int startCol   = ((int)first.DayOfWeek + 6) % 7;
            int days       = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);

            for (int d = 1; d <= days; d++)
            {
                var  date    = new DateTime(_currentMonth.Year, _currentMonth.Month, d);
                int  idx     = startCol + d - 1;
                int  col     = idx % 7;
                int  row     = idx / 7;
                bool isToday = date == DateTime.Today;
                bool isSel   = date == _selectedDate;
                bool hasTodo = dueDates.Contains(date);

                var btn = new Button
                {
                    Text = d.ToString(), Size = new Size(cellW - 2, cellH - 2),
                    Location = new Point(col * cellW, 24 + row * cellH),
                    FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Tag = date,
                    Font      = new Font("맑은 고딕", 8.5f, isToday ? FontStyle.Bold : FontStyle.Regular),
                    BackColor = isToday ? ACCENT : isSel ? Color.FromArgb(185, 215, 255) : SURFACE,
                    ForeColor = isToday ? Color.White : col >= 5 ? Color.FromArgb(215, 75, 75) : TEXT,
                };
                btn.FlatAppearance.BorderSize = 0;

                if (hasTodo && !isToday)
                    btn.Paint += (s, pe) =>
                    {
                        pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using var br = new SolidBrush(URGENT);
                        pe.Graphics.FillEllipse(br, btn.Width / 2 - 3, btn.Height - 7, 6, 6);
                    };

                btn.Click += (s, e) => { _selectedDate = (DateTime)((Button)s!).Tag!; RebuildCalendar(); };
                pnlCalendar.Controls.Add(btn);
            }
            ApplyCalendarTheme();
        }

        // ── Python 서버에서 TODO 로드 ─────────────────────────────
        private async Task LoadTodosAsync()
        {
            SetLoading(true);
            try
            {
                // 수정: /todos/rule-based → /todos/generate
                string json = await _http.GetStringAsync("http://localhost:8000/todos/generate");
                var resp = JsonSerializer.Deserialize<TodoResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (resp != null)
                {
                    _todos = resp.Todos;
                    _todos.ForEach(t => t.RecalculateDDay());
                    UpdateStats(ComputeStats(_todos));
                    RebuildTodoList();
                    RebuildCalendar();
                }
            }
            catch
            {
                if (InvokeRequired) { Invoke(() => SetLoading(false)); return; }
                lblSummary.Text      = "⚠  서버 연결 실패 — CMD에서 uvicorn main:app --reload 를 먼저 실행하세요";
                lblSummary.ForeColor = URGENT;
            }
            finally { SetLoading(false); }
        }

        // ── 통계 계산 (클라이언트) ────────────────────────────────
        private static TodoStatistics ComputeStats(List<TodoItem> todos) => new()
        {
            Total       = todos.Count,
            Urgent      = todos.Count(t => t.DDay <= 0 && !t.IsCompleted),
            High        = todos.Count(t => t.Priority is "high" or "높음" or "긴급"),
            Exams       = todos.Count(t => t.Type == "시험" || t.Category == "학업"),
            Assignments = todos.Count(t => t.Type == "과제"),
        };

        private void UpdateStats(TodoStatistics stats)
        {
            if (InvokeRequired) { Invoke(() => UpdateStats(stats)); return; }
            pnlStats.Controls.Clear();
            var items = new (string t, Color c)[]
            {
                ($"전체 {stats.Total}", TEXT), ($"🔴 긴급 {stats.Urgent}", URGENT),
                ($"🟠 높음 {stats.High}", HIGH_C), ($"📝 시험 {stats.Exams}", ACCENT),
                ($"📋 과제 {stats.Assignments}", Color.FromArgb(55, 175, 115)),
            };
            int x = 2;
            foreach (var (t, c) in items)
            {
                var lbl = new Label { Text = t, ForeColor = c, Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold), AutoSize = true, Location = new Point(x, 8) };
                pnlStats.Controls.Add(lbl);
                x += lbl.PreferredWidth + 18;
            }
        }

        // ── TODO 카드 목록 ─────────────────────────────────────────
        private void RebuildTodoList()
        {
            if (InvokeRequired) { Invoke(RebuildTodoList); return; }
            flpTodos.Controls.Clear();
            if (!_todos.Any())
            {
                flpTodos.Controls.Add(new Label { Text = "할 일이 없습니다 🎉", Font = new Font("맑은 고딕", 11f), ForeColor = SUBTEXT, AutoSize = true, Margin = new Padding(20) });
                return;
            }
            int cardW = GetTodoCardWidth();
            foreach (var todo in _todos)
            {
                var card = MakeCard(todo, cardW);   // 카드 폭을 미리 알려줘 내부 레이아웃이 올바르게 계산되도록 함
                flpTodos.Controls.Add(card);
            }
        }

        private int GetTodoCardWidth()
        {
            int available = flpTodos.ClientSize.Width
                - flpTodos.Padding.Horizontal
                - SystemInformation.VerticalScrollBarWidth
                - 6;

            return available > 120 ? available : 700;
        }

        private Panel MakeCard(TodoItem todo, int cardW)
        {
            // ── 구조 ──────────────────────────────────────────────────
            // card (Panel, H=112)
            //   ├── barLeft   [Dock=Left,  W=4]    우선순위 색상 바
            //   ├── pnlCheck  [Dock=Left,  W=36]   체크박스 전용 패널
            //   ├── pnlRight  [Dock=Right, W=140]  D-Day + 우선순위 + 수정 버튼
            //   └── pnlContent[Dock=Fill]          제목 + 부제목 + 카테고리
            //
            // card.Width를 자식 추가 *전*에 설정해 Dock 레이아웃이
            // 첫 PerformLayout부터 올바른 폭으로 계산되게 한다.
            // -----------------------------------------------------------

            const int CARD_HEIGHT  = 112;
            const int CHECK_W      = 36;
            const int RIGHT_W      = 140;

            var card = new Panel
            {
                Width = cardW,                       // ★ 자식 추가 전에 폭 확정
                Height = CARD_HEIGHT,
                BackColor = SURFACE,
                Margin = new Padding(0, 0, 0, 8),
                Cursor = Cursors.Hand,
            };

            // ── 왼쪽 우선순위 색상 바 ─────────────────────────────
            var barLeft = new Panel
            {
                Dock = DockStyle.Left, Width = 4,
                BackColor = todo.PriorityColor,
            };

            // ── 체크박스 전용 패널 (텍스트 영역과 분리) ─────────
            var pnlCheck = new Panel
            {
                Dock = DockStyle.Left, Width = CHECK_W,
                BackColor = SURFACE,
            };
            var chk = new CheckBox
            {
                Checked = todo.IsCompleted, Size = new Size(18, 18),
                Location = new Point(10, (CARD_HEIGHT - 18) / 2),  // 수직 중앙
                BackColor = SURFACE,
            };
            pnlCheck.Controls.Add(chk);

            // ── 오른쪽 패널 (D-Day + 우선순위 배지 + 수정 버튼) ──
            var pnlRight = new Panel
            {
                Dock = DockStyle.Right, Width = RIGHT_W,
                BackColor = SURFACE,
            };
            var lblDDay = new Label
            {
                Text = todo.DDayText, Font = new Font("맑은 고딕", 10f, FontStyle.Bold),
                ForeColor = todo.DDay <= 3 ? URGENT : SUBTEXT,
                AutoSize = false,
                Size = new Size(60, 24),
                Location = new Point(6, 18),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            var lblPri = new Label
            {
                Text = todo.PriorityLabel, Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                ForeColor = todo.PriorityColor,
                BackColor = Color.FromArgb(30, todo.PriorityColor.R, todo.PriorityColor.G, todo.PriorityColor.B),
                AutoSize = false,
                Size = new Size(50, 22),
                Location = new Point(72, 19),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            var btnEdit = MakeBtn("✏️", 30, 30);
            btnEdit.Font      = new Font("Segoe UI Emoji", 11f);
            btnEdit.BackColor = SURFACE2;
            btnEdit.Location  = new Point(95, 60);
            btnEdit.Click    += (s, e) => OpenEditTodoForm(todo);
            pnlRight.Controls.AddRange(new Control[] { lblDDay, lblPri, btnEdit });

            // ── 가운데 내용 패널 (제목/부제목/카테고리) ──────────
            var pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SURFACE,
                Padding = new Padding(6, 8, 8, 8),
            };

            // 부제목용 이유 — 너무 길면 카드에서는 50자 + … 으로 자름
            string reasonRaw = string.IsNullOrEmpty(todo.Reason) ? todo.Source : todo.Reason;
            string reasonShort = reasonRaw.Length > 50 ? reasonRaw.Substring(0, 50) + "…" : reasonRaw;

            // 라벨들 — Dock=Top + AutoSize=false 명시
            // Controls.Add 순서: lblCat (밑) → lblSub (중간) → lblTitle (위)
            //   Dock=Top은 나중 추가된 것이 위로 와서 자연스럽게 Title → Sub → Cat 순으로 쌓임
            var lblTitle = new Label
            {
                Text = todo.Title + (todo.IsTeamWork ? "  👥" : ""),
                Font = new Font("맑은 고딕", 10f, todo.IsCompleted ? FontStyle.Strikeout : FontStyle.Bold),
                ForeColor = todo.IsCompleted ? SUBTEXT : TEXT,
                AutoSize = false, AutoEllipsis = true,
                Dock = DockStyle.Top, Height = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0),
                Tag = "title",
            };
            var lblSub = new Label
            {
                Text = $"📅 {todo.DueDate}\n💡 {reasonShort}",   // 2줄
                Font = new Font("맑은 고딕", 8.5f), ForeColor = SUBTEXT,
                AutoSize = false,
                Dock = DockStyle.Top, Height = 42,
                TextAlign = ContentAlignment.TopLeft,
                UseMnemonic = false,
            };
            var lblCat = new Label
            {
                Text = todo.Category,
                Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                ForeColor = ACCENT,
                AutoSize = false,
                Dock = DockStyle.Top, Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            pnlContent.Controls.Add(lblCat);   // 먼저 추가 → 가장 아래
            pnlContent.Controls.Add(lblSub);
            pnlContent.Controls.Add(lblTitle); // 마지막 추가 → 가장 위

            // ── 카드에 자식 부착 (Dock 순서 주의: Fill을 먼저 add) ──
            card.Controls.Add(pnlContent);  // Fill — 가장 먼저
            card.Controls.Add(pnlRight);    // Right
            card.Controls.Add(pnlCheck);    // Left (안쪽)
            card.Controls.Add(barLeft);     // Left (바깥쪽, 가장 나중 → 가장 왼쪽)

            // ── 이벤트 ────────────────────────────────────────────
            chk.CheckedChanged += (s, e) =>
            {
                todo.IsCompleted   = chk.Checked;
                lblTitle.Font      = new Font("맑은 고딕", 10f, todo.IsCompleted ? FontStyle.Strikeout : FontStyle.Bold);
                lblTitle.ForeColor = todo.IsCompleted ? SUBTEXT : TEXT;
            };

            void hoverIn (object? s, EventArgs e) { card.BackColor = pnlContent.BackColor = pnlRight.BackColor = pnlCheck.BackColor = chk.BackColor = SURFACE2; }
            void hoverOut(object? s, EventArgs e) { card.BackColor = pnlContent.BackColor = pnlRight.BackColor = pnlCheck.BackColor = chk.BackColor = SURFACE; }
            card.MouseEnter += hoverIn;       card.MouseLeave += hoverOut;
            pnlContent.MouseEnter += hoverIn; pnlContent.MouseLeave += hoverOut;
            pnlRight.MouseEnter   += hoverIn; pnlRight.MouseLeave   += hoverOut;

            void open(object? s, EventArgs e) => ShowDetail(todo);
            card.Click += open; pnlContent.Click += open;
            lblTitle.Click += open; lblSub.Click += open; lblCat.Click += open;

            return card;
        }

        private void ShowDetail(TodoItem todo)
        {
            var dlg = new Form
            {
                Text = todo.Title, Size = new Size(400, 360), StartPosition = FormStartPosition.CenterParent,
                BackColor = SURFACE, ForeColor = TEXT, Font = new Font("맑은 고딕", 9.5f),
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
            };
            var rtb = new RichTextBox { Dock = DockStyle.Fill, BackColor = SURFACE, ForeColor = TEXT, BorderStyle = BorderStyle.None, ReadOnly = true, Font = new Font("맑은 고딕", 10f), Padding = new Padding(14) };

            rtb.SelectionFont = new Font("맑은 고딕", 12f, FontStyle.Bold); rtb.SelectionColor = todo.PriorityColor;
            rtb.AppendText($"{todo.TypeIcon} {todo.Title}\n\n");
            rtb.SelectionFont = new Font("맑은 고딕", 9.5f); rtb.SelectionColor = TEXT;
            rtb.AppendText($"우선순위: {todo.Priority}   {todo.DDayText}\n마감일:   {todo.DueDate}\n유형:     {todo.Type}{(todo.IsTeamWork ? " (팀 과제)" : "")}\n\n");
            rtb.SelectionFont = new Font("맑은 고딕", 9.5f, FontStyle.Bold); rtb.AppendText("💡 판단 이유\n");
            rtb.SelectionFont = new Font("맑은 고딕", 9.5f); rtb.SelectionColor = TEXT; rtb.AppendText($"  {todo.Reason}\n\n");
            if (todo.ActionItems.Any())
            {
                rtb.SelectionFont = new Font("맑은 고딕", 9.5f, FontStyle.Bold); rtb.AppendText("✅ 실행 항목\n");
                rtb.SelectionFont = new Font("맑은 고딕", 9.5f); rtb.SelectionColor = TEXT;
                foreach (var a in todo.ActionItems) rtb.AppendText($"  • {a}\n");
            }
            dlg.Controls.Add(rtb); dlg.ShowDialog(this);
        }

        // ── 테마 ──────────────────────────────────────────────────
        private void ApplyTheme()
        {
            BackColor = BG; pnlHeader.BackColor = SURFACE; pnlLeft.BackColor = SURFACE;
            lblMonth.ForeColor = TEXT; btnPrev.BackColor = SURFACE2; btnPrev.ForeColor = TEXT;
            btnNext.BackColor = SURFACE2; btnNext.ForeColor = TEXT;
            pnlRight.BackColor = BG; pnlTodoTop.BackColor = BG; lblTodoTitle.ForeColor = TEXT;
            pnlStats.BackColor = BG; lblSummary.ForeColor = SUBTEXT;
            flpTodos.BackColor = BG; pnlLoading.BackColor = BG; lblLoading.ForeColor = SUBTEXT;
            btnTheme.BackColor = SURFACE; btnTheme.ForeColor = TEXT;
            ApplyCalendarTheme(); RebuildTodoList();
        }

        private void ApplyCalendarTheme()
        {
            if (pnlCalendar == null) return;
            pnlCalendar.BackColor = SURFACE;
            foreach (Control c in pnlCalendar.Controls)
            {
                if (c is Label l) l.ForeColor = SUBTEXT;
                if (c is Button b && b.Tag is DateTime date && date != DateTime.Today && date != _selectedDate)
                { b.BackColor = SURFACE; b.ForeColor = TEXT; }
            }
        }

        private void SetLoading(bool on)
        {
            if (InvokeRequired) { Invoke(() => SetLoading(on)); return; }
            pnlLoading.Visible = on; flpTodos.Visible = !on;
            btnRefresh.Enabled = !on; btnRefresh.Text = on ? "⏳ 분석 중..." : "🔄 AI 생성";
        }

        private Button MakeBtn(string text, int w, int h)
        {
            var btn = new Button { Text = text, Size = new Size(w, h), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // ── TODO 추가 / 편집 ─────────────────────────────────────────
        private void OpenAddTodoForm()
        {
            using var form = new TodoForm(_http);
            if (form.ShowDialog(this) == DialogResult.OK && form.Result != null)
            {
                _todos.Add(form.Result);
                UpdateStats(ComputeStats(_todos));
                RebuildTodoList();
                RebuildCalendar();
            }
        }

        private void OpenEditTodoForm(TodoItem todo)
        {
            using var form = new TodoForm(_http, todo);
            if (form.ShowDialog(this) == DialogResult.OK && form.Result != null)
            {
                int idx = _todos.FindIndex(t => t.Id == todo.Id);
                if (idx >= 0) _todos[idx] = form.Result;
                else          _todos.Add(form.Result);
                UpdateStats(ComputeStats(_todos));
                RebuildTodoList();
                RebuildCalendar();
            }
        }

        protected override void Dispose(bool disposing) { if (disposing) _http.Dispose(); base.Dispose(disposing); }
    }
}
