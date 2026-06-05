// =============================================================
//  MainForm.cs — 메인 폼 (디자이너 분리형)
//  · 컨트롤 선언/배치 → MainForm.Designer.cs
//  · 동작/이벤트     → MainForm.cs (이 파일)
//  · 캘린더 셀과 TODO 카드는 런타임에 동적 생성
// =============================================================
//
// ─────────────────────────────────────────────────────────────────────
// [변경 사항] — 원본 (지원의 develop 브랜치) 대비
//   1. partial class 로 분리 → MainForm.Designer.cs 신설
//   2. 캘린더 토/일 색상 로직: "평소 검정/흰색, 선택 시 토=파랑/일=빨강"
//      (원본은 평일/주말 색을 항상 다르게 표시)
//   3. 다크모드에서 캘린더 셀 글자색이 클릭 전엔 안 바뀌던 문제 해결
//      → ApplyTheme() 안에서 RebuildCalendar() 호출로 즉시 갱신
//   4. "이번 달 일정" → "오늘의 일정" (또는 선택한 날짜 일정) 패널 신설
//      → flpSchedule, RebuildScheduleList() 추가
//   5. 투두 필터링: GetVisibleTodos() — 선택 날짜 기준 +1개월 범위
//      (과거 날짜 선택 시 완료된 TODO 도 함께 표시)
//   6. 통계(ComputeStats) 의 Urgent/High 계산을 DDay 기준으로 변경
//   7. 헤더 라벨(lblTitle/lblDate)을 디자이너로 옮겨 다크모드 색상 적용
//   8. ShowDetail 코드 생성 폼 제거 → TodoDetailForm (별도 폼) 사용
//   9. OpenAddTodoForm / OpenEditTodoForm 이 AddTodoForm / EditTodoForm 호출
//      (원본은 단일 TodoForm 클래스 사용 — 본 변경에서 분리)
//  10. 수정 버튼: ✏ → Segoe Fluent Icons 의 Edit 글리프() 사용,
//      다크모드 흰색으로 명확히 표시
//  11. SAT_C(파랑) / SUN_C(빨강) / SAFE_C(초록) 색 상수 추가
//   ※ 핵심 변경 지점마다 코드에 "[변경]" 주석으로 표시
// ─────────────────────────────────────────────────────────────────────

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
    public partial class MainForm : Form
    {
        // ── 상태 ───────────────────────────────────────────────────
        private bool      _isDark;
        private DateTime  _currentMonth = DateTime.Today;
        private DateTime  _selectedDate = DateTime.Today;
        private List<TodoItem> _todos    = new();
        private readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(60) };

        // ── 테마 색상 ──────────────────────────────────────────────
        private Color BG       => _isDark ? Color.FromArgb(15, 18, 30)    : Color.FromArgb(242, 245, 255);
        private Color SURFACE  => _isDark ? Color.FromArgb(24, 28, 46)    : Color.White;
        private Color SURFACE2 => _isDark ? Color.FromArgb(32, 38, 60)    : Color.FromArgb(245, 248, 255);
        private Color ACCENT   => Color.FromArgb(82, 130, 255);
        private Color TEXT     => _isDark ? Color.FromArgb(232, 238, 255) : Color.FromArgb(25, 30, 60);
        private Color SUBTEXT  => _isDark ? Color.FromArgb(170, 185, 220) : Color.FromArgb(115, 126, 162);
        private Color URGENT   => Color.FromArgb(220, 60, 60);
        private Color HIGH_C   => Color.FromArgb(230, 130, 30);
        // [변경] 색상 상수 신설 — 안전(초록), 토요일(파랑), 일요일(빨강)
        private Color SAFE_C   => Color.FromArgb( 55, 175, 115);
        private Color SAT_C    => Color.FromArgb( 70, 130, 230);   // 토요일 (파랑)
        private Color SUN_C    => Color.FromArgb(215,  75,  75);   // 일요일 (빨강)

        // ── 생성자 ─────────────────────────────────────────────────
        public MainForm(bool isDark = false)
        {
            _isDark = isDark;
            InitializeComponent();
            WireUp();
            ApplyTheme();
            RebuildCalendar();
            RebuildScheduleList();
            _ = LoadTodosAsync();
        }

        // ── 이벤트 연결 & 동적 UI 조정 ────────────────────────────
        private void WireUp()
        {
            lblDate.Text = DateTime.Today.ToString("yyyy년 MM월 dd일 (ddd)");

            // 헤더: 테마 토글 + 우상단 고정
            pnlHeader.Resize += (s, e) => btnTheme.Location = new Point(pnlHeader.Width - 46, 11);
            btnTheme.Click   += (s, e) =>
            {
                _isDark = !_isDark;
                btnTheme.Text = _isDark ? "☀" : "🌙";
                ApplyTheme();
            };

            // 캘린더 이전·다음 달
            btnPrev.Click += (s, e) => { _currentMonth = _currentMonth.AddMonths(-1); RebuildCalendar(); };
            btnNext.Click += (s, e) => { _currentMonth = _currentMonth.AddMonths( 1); RebuildCalendar(); };
            pnlLeft.Resize += (s, e) =>
            {
                btnNext.Location = new Point(pnlLeft.Width - 44, 8);
                btnPrev.Location = new Point(pnlLeft.Width - 80, 8);
            };

            // 오른쪽 상단: 추가/AI 생성 버튼 우측 정렬
            pnlTodoTop.Resize += (s, e) =>
            {
                btnRefresh.Location = new Point(pnlTodoTop.Width - 112, 6);
                btnAdd.Location     = new Point(pnlTodoTop.Width - 200, 6);
            };
            btnRefresh.Click += async (s, e) => await LoadTodosAsync();
            btnAdd.Click     += (s, e) => OpenAddTodoForm();

            // 로딩 라벨 중앙 정렬
            pnlLoading.Resize += (s, e) =>
            {
                lblLoading.Location = new Point(
                    (pnlLoading.Width  - lblLoading.PreferredWidth)  / 2,
                    (pnlLoading.Height - lblLoading.PreferredHeight) / 2);
            };

            // 창 크기 변경 시 카드 너비 자동 조정
            flpTodos.Resize += (s, e) =>
            {
                int w = GetTodoCardWidth();
                if (w <= 50) return;
                foreach (Control c in flpTodos.Controls) c.Width = w;
            };
        }

        // ── 캘린더 ────────────────────────────────────────────────
        private void RebuildCalendar()
        {
            lblMonth.Text = _currentMonth.ToString("yyyy년 MM월");
            pnlCalendar.Controls.Clear();

            int cellW = 38, cellH = 32;
            // [변경] 요일 순서를 월~일 → 일~토 로 변경
            //   · col 0 = 일요일(SUN_C 빨강), col 6 = 토요일(SAT_C 파랑)
            string[] dn = { "일", "월", "화", "수", "목", "금", "토" };

            for (int i = 0; i < 7; i++)
                pnlCalendar.Controls.Add(new Label
                {
                    Text = dn[i], Size = new Size(cellW, 20), Location = new Point(i * cellW, 0),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                    // [변경] 헤더 색 — col 0(일) 빨강, col 6(토) 파랑, 평일 SUBTEXT
                    ForeColor = i == 0 ? SUN_C : i == 6 ? SAT_C : SUBTEXT,
                });

            var dueDates = _todos
                .Where(t => !string.IsNullOrEmpty(t.DueDate))
                .Select(t => { DateTime.TryParse(t.DueDate, out var d); return d.Date; })
                .ToHashSet();

            DateTime first = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            // [변경] DayOfWeek (Sunday=0..Saturday=6) 를 그대로 사용
            //   (이전엔 +6 % 7 로 월요일 시작에 맞췄던 계산이 필요 없음)
            int startCol   = (int)first.DayOfWeek;
            int days       = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);

            for (int d = 1; d <= days; d++)
            {
                var  date    = new DateTime(_currentMonth.Year, _currentMonth.Month, d);
                int  idx     = startCol + d - 1;
                int  col     = idx % 7;
                int  row     = idx / 7;
                bool isToday = date == DateTime.Today;
                bool isSel   = date == _selectedDate.Date;
                bool hasTodo = dueDates.Contains(date);

                // [변경] 요일 색상 규칙 (원본: 평일/주말 색을 항상 다르게 표시)
                //   · 평상시(미선택)         : 모든 요일 TEXT 색 (라이트=검정, 다크=흰색)
                //   · 선택했을 때 (isSel)   : 일=빨강, 토=파랑, 평일=진한 남색 (하늘색 배경 위 가독성)
                //   · 오늘(isToday)         : 흰색 (ACCENT 진한 파란 배경 위)
                //   ※ 요일 순서 일~토 로 변경됨 → col 0 = 일요일, col 6 = 토요일
                Color foreColor;
                if (isToday)
                    foreColor = Color.White;
                else if (isSel)
                    foreColor = col == 0 ? SUN_C
                              : col == 6 ? SAT_C
                              : Color.FromArgb(20, 30, 60);
                else
                    foreColor = TEXT;

                var btn = new Button
                {
                    Text = d.ToString(), Size = new Size(cellW - 2, cellH - 2),
                    Location = new Point(col * cellW, 24 + row * cellH),
                    FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Tag = date,
                    Font      = new Font("맑은 고딕", 8.5f, isToday ? FontStyle.Bold : FontStyle.Regular),
                    BackColor = isToday ? ACCENT : isSel ? Color.FromArgb(185, 215, 255) : SURFACE,
                    ForeColor = foreColor,
                    UseVisualStyleBackColor = false,
                };
                btn.FlatAppearance.BorderSize = 0;

                if (hasTodo && !isToday)
                    btn.Paint += (s, pe) =>
                    {
                        pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using var br = new SolidBrush(URGENT);
                        pe.Graphics.FillEllipse(br, btn.Width / 2 - 3, btn.Height - 7, 6, 6);
                    };

                btn.Click += (s, e) =>
                {
                    _selectedDate = (DateTime)((Button)s!).Tag!;
                    RebuildCalendar();
                    RebuildScheduleList();
                    RebuildTodoList();
                };
                pnlCalendar.Controls.Add(btn);
            }
            ApplyCalendarTheme();
        }

        // ── 오늘/선택 날짜의 일정 목록 ────────────────────────────
        // [신규] 원본에 없던 메서드 — 캘린더 아래 "오늘의 일정" 패널에
        //   선택된 날짜의 TODO 만 추려서 표시 (제목 좌측에 우선순위 색 바)
        private void RebuildScheduleList()
        {
            // 라벨: "오늘의 일정" 또는 "M월 d일 (ddd) 일정"
            bool isToday = _selectedDate.Date == DateTime.Today;
            lblSchedTitle.Text = isToday
                ? "📅 오늘의 일정"
                : $"📅 {_selectedDate:M월 d일 (ddd)} 일정";
            lblSchedTitle.ForeColor = TEXT;

            flpSchedule.Controls.Clear();

            var items = _todos
                .Where(t =>
                {
                    if (!DateTime.TryParse(t.DueDate, out var d)) return false;
                    return d.Date == _selectedDate.Date;
                })
                .OrderBy(t => t.PriorityLabel == "긴급" ? 0 : t.PriorityLabel == "높음" ? 1 : 2)
                .ToList();

            if (!items.Any())
            {
                flpSchedule.Controls.Add(new Label
                {
                    Text = "일정이 없습니다 ✨",
                    Font = new Font("맑은 고딕", 9f),
                    ForeColor = SUBTEXT, AutoSize = true,
                    Margin = new Padding(4, 8, 4, 4),
                });
                return;
            }

            foreach (var t in items)
            {
                var row = new Panel
                {
                    Width = flpSchedule.ClientSize.Width - 8,
                    Height = 36,
                    BackColor = SURFACE2,
                    Margin = new Padding(0, 0, 0, 6),
                };
                var bar = new Panel { Dock = DockStyle.Left, Width = 4, BackColor = t.PriorityColor };
                var lbl = new Label
                {
                    Text = "  " + t.Title,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("맑은 고딕", 9f, t.IsCompleted ? FontStyle.Strikeout : FontStyle.Regular),
                    ForeColor = t.IsCompleted ? SUBTEXT : TEXT,
                    AutoEllipsis = true,
                };
                row.Controls.Add(lbl);
                row.Controls.Add(bar);
                row.Click += (s, e) => ShowDetail(t);
                lbl.Click += (s, e) => ShowDetail(t);
                flpSchedule.Controls.Add(row);
            }
        }

        // ── Python 서버에서 TODO 로드 ─────────────────────────────
        private async Task LoadTodosAsync()
        {
            SetLoading(true);
            try
            {
                string json = await _http.GetStringAsync("http://localhost:8000/todos/generate");
                var resp = JsonSerializer.Deserialize<TodoResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (resp != null)
                {
                    _todos = resp.Todos;
                    _todos.ForEach(t => t.RecalculateDDay());
                    UpdateStats(ComputeStats(GetVisibleTodos()));
                    RebuildTodoList();
                    RebuildCalendar();
                    RebuildScheduleList();
                }
            }
            catch
            {
                if (InvokeRequired) { Invoke(() => SetLoading(false)); return; }
                lblSummary.Text      = "⚠  서버 연결 실패 — uvicorn main:app --reload 를 먼저 실행하세요";
                lblSummary.ForeColor = URGENT;
            }
            finally { SetLoading(false); }
        }

        // ── 표시 대상 TODO 필터링 ─────────────────────────────────
        // [신규] 원본은 _todos 전체를 그대로 보여줌 → 본 메서드로 필터링
        //   기본: 선택 날짜(또는 오늘) 기준 +1개월 범위
        //   - 오늘 이후 날짜 선택: 미완료만
        //   - 과거 날짜 선택   : 완료된 것도 함께 표시 (회고용)
        private List<TodoItem> GetVisibleTodos()
        {
            DateTime baseDate = _selectedDate.Date;
            DateTime endDate  = baseDate.AddMonths(1);
            bool     showDone = baseDate < DateTime.Today;

            return _todos
                .Where(t =>
                {
                    if (!DateTime.TryParse(t.DueDate, out var d)) return false;
                    if (d.Date < baseDate || d.Date > endDate)    return false;
                    if (!showDone && t.IsCompleted)               return false;
                    return true;
                })
                .OrderBy(t => t.DDay)
                .ToList();
        }

        // ── 통계 ──────────────────────────────────────────────────
        // [변경] Urgent/High 계산을 Priority 문자열 기반에서 DDay 기반으로 변경
        //   (PriorityLabel/PriorityColor 변경과 일관성 유지)
        private static TodoStatistics ComputeStats(List<TodoItem> todos) => new()
        {
            Total       = todos.Count,
            Urgent      = todos.Count(t => t.DDay <= 3 && !t.IsCompleted),
            High        = todos.Count(t => t.DDay > 3 && t.DDay <= 7 && !t.IsCompleted),
            Exams       = todos.Count(t => t.Type == "시험" || t.Category == "학업"),
            Assignments = todos.Count(t => t.Type == "과제"),
        };

        private void UpdateStats(TodoStatistics stats)
        {
            if (InvokeRequired) { Invoke(() => UpdateStats(stats)); return; }
            pnlStats.Controls.Clear();
            pnlStats.Controls.Add(lblSummary); // 기본 라벨 다시 추가
            lblSummary.Visible = false;        // 통계가 있으면 안내 문구 숨김
            var items = new (string t, Color c)[]
            {
                ($"전체 {stats.Total}", TEXT),
                ($"🔴 긴급 {stats.Urgent}", URGENT),
                ($"🟠 높음 {stats.High}",   HIGH_C),
                ($"📝 시험 {stats.Exams}",  ACCENT),
                ($"📋 과제 {stats.Assignments}", SAFE_C),
            };
            int x = 2;
            foreach (var (t, c) in items)
            {
                var lbl = new Label { Text = t, ForeColor = c, Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold), AutoSize = true, Location = new Point(x, 8) };
                pnlStats.Controls.Add(lbl);
                x += lbl.PreferredWidth + 18;
            }
        }

        // ── TODO 카드 목록 ────────────────────────────────────────
        private void RebuildTodoList()
        {
            if (InvokeRequired) { Invoke(RebuildTodoList); return; }
            flpTodos.Controls.Clear();
            var visible = GetVisibleTodos();
            UpdateStats(ComputeStats(visible));
            if (!visible.Any())
            {
                flpTodos.Controls.Add(new Label
                {
                    Text = _selectedDate.Date < DateTime.Today
                        ? "이 날짜 이후 1개월 내 일정이 없습니다 ✨"
                        : "선택한 날짜 이후 1개월 내 할 일이 없습니다 🎉",
                    Font = new Font("맑은 고딕", 11f), ForeColor = SUBTEXT,
                    AutoSize = true, Margin = new Padding(20),
                });
                return;
            }
            int cardW = GetTodoCardWidth();
            foreach (var todo in visible)
                flpTodos.Controls.Add(MakeCard(todo, cardW));
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
            const int CARD_HEIGHT = 112, CHECK_W = 36, RIGHT_W = 150;

            var card = new Panel
            {
                Width = cardW, Height = CARD_HEIGHT,
                BackColor = SURFACE, Margin = new Padding(0, 0, 0, 8),
                Cursor = Cursors.Hand,
            };

            // 왼쪽 우선순위 색상 바
            var barLeft = new Panel { Dock = DockStyle.Left, Width = 4, BackColor = todo.PriorityColor };

            // 체크박스 패널
            var pnlCheck = new Panel { Dock = DockStyle.Left, Width = CHECK_W, BackColor = SURFACE };
            var chk = new CheckBox
            {
                Checked = todo.IsCompleted, Size = new Size(18, 18),
                Location = new Point(10, (CARD_HEIGHT - 18) / 2),
                BackColor = SURFACE,
            };
            pnlCheck.Controls.Add(chk);

            // 오른쪽: D-Day + 우선순위 배지 + 편집 버튼
            var pnlRight = new Panel { Dock = DockStyle.Right, Width = RIGHT_W, BackColor = SURFACE };
            var lblDDay = new Label
            {
                Text = todo.DDayText, Font = new Font("맑은 고딕", 10f, FontStyle.Bold),
                // [변경] D-Day 글자색을 4단계 PriorityColor 로 통일
                //   (긴급 빨강 / 높음 주황 / 안전 초록 / 낮음 회색)
                //   원본 및 이전 버전은 2색만 사용 (DDay<=3 빨강 / 그 외 회색)
                ForeColor = todo.PriorityColor,
                AutoSize = false, Size = new Size(60, 24),
                Location = new Point(6, 18), TextAlign = ContentAlignment.MiddleCenter,
            };
            var lblPri = new Label
            {
                Text = todo.PriorityLabel, Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                ForeColor = todo.PriorityColor,
                BackColor = Color.FromArgb(30, todo.PriorityColor.R, todo.PriorityColor.G, todo.PriorityColor.B),
                AutoSize = false, Size = new Size(56, 22),
                Location = new Point(72, 19), TextAlign = ContentAlignment.MiddleCenter,
            };
            // [변경] 연필 모양 편집 버튼 — Segoe Fluent Icons / MDL2 Assets 폰트의
            //   Edit 글리프()를 사용해 첨부 이미지와 흡사한 깔끔한 연필 아이콘 표시.
            //   라이트=진한 남색, 다크=흰색으로 모드별 또렷이 보이도록 설정.
            var btnEdit = new Button
            {
                Text = "",   // Segoe MDL2 Assets / Segoe Fluent Icons: 편집(연필) 글리프
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                // Windows 11이면 Segoe Fluent Icons, 그 이전이면 Segoe MDL2 Assets로 자동 폴백
                Font = new Font("Segoe Fluent Icons", 13f),
                BackColor = SURFACE2,
                ForeColor = _isDark ? Color.White : Color.FromArgb(50, 60, 90),
                Location = new Point(100, 60),
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = false,
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => OpenEditTodoForm(todo);
            pnlRight.Controls.AddRange(new Control[] { lblDDay, lblPri, btnEdit });

            // 가운데 내용 패널
            var pnlContent = new Panel
            {
                Dock = DockStyle.Fill, BackColor = SURFACE,
                Padding = new Padding(6, 8, 8, 8),
            };
            // [변경] null 안전 처리 — 서버가 source_event 를 null 로 반환하면
            //   reasonRaw 가 null 이 되어 .Length 호출 시 NullReferenceException 발생하던 문제 해결
            string reasonRaw   = !string.IsNullOrEmpty(todo.Reason) ? todo.Reason
                                : !string.IsNullOrEmpty(todo.Source) ? todo.Source
                                : "";
            string reasonShort = reasonRaw.Length > 50 ? reasonRaw.Substring(0, 50) + "…" : reasonRaw;

            var lblTitle = new Label
            {
                Text = todo.Title + (todo.IsTeamWork ? "  👥" : ""),
                Font = new Font("맑은 고딕", 10f, todo.IsCompleted ? FontStyle.Strikeout : FontStyle.Bold),
                ForeColor = todo.IsCompleted ? SUBTEXT : TEXT,
                AutoSize = false, AutoEllipsis = true,
                Dock = DockStyle.Top, Height = 28,
                TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0),
            };
            var lblSub = new Label
            {
                Text = $"📅 {todo.DueDate}\n💡 {reasonShort}",
                Font = new Font("맑은 고딕", 8.5f), ForeColor = SUBTEXT,
                AutoSize = false, Dock = DockStyle.Top, Height = 42,
                TextAlign = ContentAlignment.TopLeft, UseMnemonic = false,
            };
            var lblCat = new Label
            {
                Text = todo.Category, Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                ForeColor = ACCENT, AutoSize = false,
                Dock = DockStyle.Top, Height = 20, TextAlign = ContentAlignment.MiddleLeft,
            };
            pnlContent.Controls.Add(lblCat);
            pnlContent.Controls.Add(lblSub);
            pnlContent.Controls.Add(lblTitle);

            card.Controls.Add(pnlContent);
            card.Controls.Add(pnlRight);
            card.Controls.Add(pnlCheck);
            card.Controls.Add(barLeft);

            // 이벤트
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

        // ── TODO 상세 다이얼로그 → 별도 폼(TodoDetailForm) ────────
        // [변경] 원본은 ShowDetail 안에서 임시 Form 을 직접 코드로 구성
        //   → 디자이너 분리형 TodoDetailForm 으로 이관 (편집 가능 + 저장 버튼)
        private void ShowDetail(TodoItem todo)
        {
            using var dlg = new TodoDetailForm(_http, todo, _isDark);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Result != null)
            {
                // 상세 폼에서 제목/이유를 편집·저장한 경우 화면 갱신
                int idx = _todos.FindIndex(t => t.Id == dlg.Result.Id);
                if (idx >= 0) _todos[idx] = dlg.Result;
                RebuildTodoList();
                RebuildCalendar();
                RebuildScheduleList();
            }
        }

        // ── 테마 적용 ─────────────────────────────────────────────
        private void ApplyTheme()
        {
            BackColor = BG;

            // 헤더
            pnlHeader.BackColor = SURFACE;
            lblTitle.ForeColor  = TEXT;
            lblDate.ForeColor   = SUBTEXT;
            btnTheme.BackColor  = SURFACE;
            btnTheme.ForeColor  = TEXT;

            // 본문 분리선
            pnlSep.BackColor = _isDark ? Color.FromArgb(45, 50, 75) : Color.FromArgb(220, 225, 240);

            // 왼쪽
            pnlLeft.BackColor    = SURFACE;
            lblMonth.ForeColor   = TEXT;
            btnPrev.BackColor    = SURFACE2; btnPrev.ForeColor = TEXT;
            btnNext.BackColor    = SURFACE2; btnNext.ForeColor = TEXT;
            lblSchedTitle.ForeColor = TEXT;
            flpSchedule.BackColor   = SURFACE;

            // 오른쪽
            pnlRight.BackColor    = BG;
            pnlTodoTop.BackColor  = BG;
            lblTodoTitle.ForeColor = TEXT;
            pnlStats.BackColor    = BG;
            lblSummary.ForeColor  = SUBTEXT;
            flpTodos.BackColor    = BG;
            pnlLoading.BackColor  = BG;
            lblLoading.ForeColor  = SUBTEXT;

            // [변경] 원본의 ApplyCalendarTheme()은 BackColor만 손봐서
            //   다크모드 전환 후 클릭 전까지 글자색이 안 바뀌는 문제가 있었음.
            //   캘린더는 통째로 다시 그려야 모든 셀(요일 라벨 + 날짜 버튼)의
            //   BackColor/ForeColor가 새 테마에 맞춰 즉시 갱신됨.
            RebuildCalendar();
            RebuildTodoList();
            RebuildScheduleList();
        }

        private void ApplyCalendarTheme()
        {
            // ApplyTheme에서 RebuildCalendar를 호출하므로 별도 색상 보정은 불필요.
            // 기존 호출부 호환을 위해 남겨두고 배경색만 동기화.
            pnlCalendar.BackColor = SURFACE;
        }

        // ── 로딩 표시 ─────────────────────────────────────────────
        private void SetLoading(bool on)
        {
            if (InvokeRequired) { Invoke(() => SetLoading(on)); return; }
            pnlLoading.Visible = on; flpTodos.Visible = !on;
            btnRefresh.Enabled = !on; btnRefresh.Text = on ? "⏳ 분석 중..." : "🔄 AI 생성";
        }

        // ── TODO 추가 / 편집 (전용 폼 사용) ─────────────────────
        // [변경] 원본은 단일 TodoForm 클래스를 모드 분기로 재사용 →
        //   AddTodoForm / EditTodoForm 으로 분리 (각각 디자이너 분리형)
        private void OpenAddTodoForm()
        {
            // [변경] 현재 테마 (_isDark)를 추가 폼에 전달 → 다크모드 일관성
            using var form = new AddTodoForm(_http, _isDark);
            if (form.ShowDialog(this) == DialogResult.OK && form.Result != null)
            {
                _todos.Add(form.Result);
                UpdateStats(ComputeStats(GetVisibleTodos()));
                RebuildTodoList();
                RebuildCalendar();
                RebuildScheduleList();
            }
        }

        private void OpenEditTodoForm(TodoItem todo)
        {
            // [변경] 현재 테마 (_isDark)를 편집 폼에 전달 → 다크모드 일관성
            using var form = new EditTodoForm(_http, todo, _isDark);
            if (form.ShowDialog(this) == DialogResult.OK && form.Result != null)
            {
                int idx = _todos.FindIndex(t => t.Id == todo.Id);
                if (idx >= 0) _todos[idx] = form.Result;
                else          _todos.Add(form.Result);
                UpdateStats(ComputeStats(GetVisibleTodos()));
                RebuildTodoList();
                RebuildCalendar();
                RebuildScheduleList();
            }
        }
    }
}
