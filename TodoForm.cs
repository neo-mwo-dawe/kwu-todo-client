// =============================================================
//  TodoForm.cs — TODO 수동 추가 / 편집 폼
//  담당: 지원
//  설명: "추가" 버튼 또는 카드 편집 버튼 클릭 시 열립니다.
//        새 TODO 추가: POST /todos
//        기존 TODO 편집: PUT /todos/{id}
// =============================================================

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace KwuTodoAI
{
    public class TodoForm : Form
    {
        // ── 모드 ──────────────────────────────────────────────
        private readonly bool       _isEdit;          // true = 편집, false = 신규 추가
        private readonly TodoItem?  _original;        // 편집 시 원본 데이터
        private readonly HttpClient _http;

        // ── 서버 API 주소 ──────────────────────────────────────
        private const string BASE_URL = "http://localhost:8000";

        // ── 결과 ──────────────────────────────────────────────
        /// <summary>저장 성공 시 서버에서 반환된(또는 입력된) TodoItem. 취소 시 null.</summary>
        public TodoItem? Result { get; private set; }

        // ── UI 컨트롤 ──────────────────────────────────────────
        private TextBox         txtTitle    = null!;
        private DateTimePicker  dtpDueDate  = null!;
        private CheckBox        chkNoDue    = null!;
        private ComboBox        cboPriority = null!;
        private ComboBox        cboCategory = null!;
        private TextBox         txtReason   = null!;
        private Button          btnSave     = null!;
        private Button          btnCancel   = null!;
        private Label           lblStatus   = null!;

        // ── 테마 색상 ──────────────────────────────────────────
        private readonly Color _bg      = Color.FromArgb(242, 245, 255);
        private readonly Color _surface = Color.White;
        private readonly Color _accent  = Color.FromArgb(82, 130, 255);
        private readonly Color _text    = Color.FromArgb(25, 30, 60);
        private readonly Color _subtext = Color.FromArgb(115, 126, 162);
        private readonly Color _urgent  = Color.FromArgb(220, 60, 60);

        // =============================================================
        //  생성자
        // =============================================================

        /// <summary>신규 추가 모드</summary>
        public TodoForm(HttpClient http)
        {
            _isEdit   = false;
            _original = null;
            _http     = http;
            InitUI();
        }

        /// <summary>편집 모드 — 기존 TodoItem 값으로 폼 채움</summary>
        public TodoForm(HttpClient http, TodoItem existing)
        {
            _isEdit   = true;
            _original = existing;
            _http     = http;
            InitUI();
            PopulateFields(existing);
        }

        // =============================================================
        //  UI 초기화
        // =============================================================
        private void InitUI()
        {
            Text            = _isEdit ? "TODO 편집" : "TODO 추가";
            Size            = new Size(420, 400);
            MinimumSize     = new Size(400, 380);
            StartPosition   = FormStartPosition.CenterParent;
            BackColor       = _bg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            Font            = new Font("맑은 고딕", 9.5f);

            int lx = 20, fx = 110, fw = 270, ly = 18;

            // ── 제목 ─────────────────────────────────────────
            AddLabel("제목 *", lx, ly);
            txtTitle = new TextBox { Location = new Point(fx, ly - 2), Size = new Size(fw, 24), MaxLength = 200 };
            Controls.Add(txtTitle);
            ly += 36;

            // ── 마감일 ────────────────────────────────────────
            AddLabel("마감일", lx, ly);
            dtpDueDate = new DateTimePicker
            {
                Location = new Point(fx, ly - 2), Size = new Size(190, 24),
                Format   = DateTimePickerFormat.Short,
                Value    = DateTime.Today.AddDays(7),
            };
            chkNoDue = new CheckBox { Text = "없음", Location = new Point(fx + 198, ly), AutoSize = true };
            chkNoDue.CheckedChanged += (s, e) => dtpDueDate.Enabled = !chkNoDue.Checked;
            Controls.Add(dtpDueDate);
            Controls.Add(chkNoDue);
            ly += 36;

            // ── 우선순위 ──────────────────────────────────────
            AddLabel("우선순위", lx, ly);
            cboPriority = new ComboBox
            {
                Location     = new Point(fx, ly - 2), Size = new Size(120, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboPriority.Items.AddRange(new object[] { "높음 (high)", "보통 (medium)", "낮음 (low)" });
            cboPriority.SelectedIndex = 1; // 기본값: 보통
            Controls.Add(cboPriority);
            ly += 36;

            // ── 카테고리 ──────────────────────────────────────
            AddLabel("카테고리", lx, ly);
            cboCategory = new ComboBox
            {
                Location      = new Point(fx, ly - 2), Size = new Size(120, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboCategory.Items.AddRange(new object[] { "학업", "행정", "장학", "기타" });
            cboCategory.SelectedIndex = 0;
            Controls.Add(cboCategory);
            ly += 36;

            // ── 메모/이유 (선택) ──────────────────────────────
            AddLabel("메모", lx, ly);
            txtReason = new TextBox
            {
                Location   = new Point(fx, ly - 2), Size = new Size(fw, 60),
                Multiline  = true, MaxLength = 300,
                ScrollBars = ScrollBars.Vertical,
            };
            Controls.Add(txtReason);
            ly += 76;

            // ── 상태 메시지 ────────────────────────────────────
            lblStatus = new Label
            {
                Location  = new Point(lx, ly), Size = new Size(fw + lx, 20),
                ForeColor = _urgent, Font = new Font("맑은 고딕", 8.5f), AutoSize = false,
            };
            Controls.Add(lblStatus);
            ly += 28;

            // ── 버튼 ──────────────────────────────────────────
            btnSave = new Button
            {
                Text      = _isEdit ? "저장" : "추가",
                Size      = new Size(90, 32), Location = new Point(fx + fw - 186, ly),
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                BackColor = _accent, ForeColor = Color.White,
                Font      = new Font("맑은 고딕", 9.5f, FontStyle.Bold),
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += async (s, e) => await OnSaveAsync();

            btnCancel = new Button
            {
                Text      = "취소",
                Size      = new Size(84, 32), Location = new Point(fx + fw - 88, ly),
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(220, 223, 235), ForeColor = _text,
                Font      = new Font("맑은 고딕", 9.5f),
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { Result = null; DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            // ── 폼 크기 자동 조정 ─────────────────────────────
            ClientSize = new Size(400, ly + 48);

            // 배경색 통일
            foreach (Control c in Controls)
            {
                if (c is Label l) { l.ForeColor = _text; }
                if (c is TextBox t) { t.BackColor = _surface; t.BorderStyle = BorderStyle.FixedSingle; }
            }
        }

        // =============================================================
        //  편집 모드: 기존 값 채우기
        // =============================================================
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

            cboPriority.SelectedIndex = item.Priority switch
            {
                "high"   => 0,
                "low"    => 2,
                _        => 1, // medium 기본값
            };

            cboCategory.SelectedIndex = item.Category switch
            {
                "행정" => 1,
                "장학" => 2,
                "기타" => 3,
                _      => 0, // 학업 기본값
            };

            txtReason.Text = item.Reason;
        }

        // =============================================================
        //  저장 처리
        // =============================================================
        private async Task OnSaveAsync()
        {
            // ── 유효성 검사 ──────────────────────────────────────
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                lblStatus.Text     = "⚠  제목을 입력하세요.";
                lblStatus.ForeColor = _urgent;
                txtTitle.Focus();
                return;
            }

            btnSave.Enabled = false;
            btnSave.Text    = "저장 중...";
            lblStatus.Text  = "";

            // ── 요청 바디 구성 ────────────────────────────────────
            string priority = cboPriority.SelectedIndex switch
            {
                0 => "high",
                2 => "low",
                _ => "medium",
            };
            string category = cboCategory.SelectedItem?.ToString() ?? "기타";
            string? dueDate = chkNoDue.Checked ? null : dtpDueDate.Value.ToString("yyyy-MM-dd");

            try
            {
                if (_isEdit && _original != null)
                {
                    // ── PUT /todos/{id} ────────────────────────────
                    var body = new
                    {
                        title        = txtTitle.Text.Trim(),
                        priority,
                        category,
                        due_date     = dueDate,
                        source_event = txtReason.Text.Trim().Length > 0 ? txtReason.Text.Trim() : (string?)null,
                        is_done      = _original.IsCompleted,
                    };
                    bool ok = await PutAsync($"/todos/{_original.Id}", body);
                    if (!ok) return;

                    // 반환값 구성 (서버 응답을 다시 읽을 수도 있지만, 단순히 입력값으로 구성)
                    var updated = _original;
                    updated.Title      = txtTitle.Text.Trim();
                    updated.Priority   = priority;
                    updated.Category   = category;
                    updated.DueDate    = dueDate ?? "";
                    updated.Reason     = txtReason.Text.Trim();
                    updated.RecalculateDDay();
                    Result = updated;
                }
                else
                {
                    // ── POST /todos ────────────────────────────────
                    var body = new
                    {
                        title        = txtTitle.Text.Trim(),
                        priority,
                        category,
                        due_date     = dueDate,
                        source_event = txtReason.Text.Trim().Length > 0 ? txtReason.Text.Trim() : (string?)null,
                    };
                    var created = await PostAsync<TodoItem>("/todos", body);
                    if (created == null) return;

                    created.Reason = txtReason.Text.Trim();
                    created.RecalculateDDay();
                    Result = created;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text    = _isEdit ? "저장" : "추가";
            }
        }

        // =============================================================
        //  HTTP 헬퍼
        // =============================================================
        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        private async Task<bool> PutAsync(string endpoint, object body)
        {
            try
            {
                string json     = JsonSerializer.Serialize(body);
                var    content  = new StringContent(json, Encoding.UTF8, "application/json");
                var    response = await _http.PutAsync(BASE_URL + endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    ShowError($"서버 오류 {(int)response.StatusCode}: {response.ReasonPhrase}");
                    return false;
                }
                return true;
            }
            catch (HttpRequestException)   { ShowError("서버에 연결할 수 없습니다.\nuvicorn main:app --reload 실행 여부를 확인하세요."); return false; }
            catch (TaskCanceledException)  { ShowError("요청 시간 초과."); return false; }
            catch (Exception ex)           { ShowError($"오류: {ex.Message}"); return false; }
        }

        private async Task<T?> PostAsync<T>(string endpoint, object body) where T : class
        {
            try
            {
                string json     = JsonSerializer.Serialize(body);
                var    content  = new StringContent(json, Encoding.UTF8, "application/json");
                var    response = await _http.PostAsync(BASE_URL + endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    ShowError($"서버 오류 {(int)response.StatusCode}: {response.ReasonPhrase}");
                    return null;
                }
                string resp = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(resp, _json);
            }
            catch (HttpRequestException)   { ShowError("서버에 연결할 수 없습니다.\nuvicorn main:app --reload 실행 여부를 확인하세요."); return null; }
            catch (TaskCanceledException)  { ShowError("요청 시간 초과."); return null; }
            catch (Exception ex)           { ShowError($"오류: {ex.Message}"); return null; }
        }

        private void ShowError(string msg)
        {
            if (InvokeRequired) { Invoke(() => ShowError(msg)); return; }
            lblStatus.Text      = "⚠  " + msg;
            lblStatus.ForeColor = _urgent;
        }

        // =============================================================
        //  헬퍼
        // =============================================================
        private void AddLabel(string text, int x, int y)
        {
            Controls.Add(new Label
            {
                Text      = text, Location = new Point(x, y + 2), AutoSize = true,
                ForeColor = _text, Font = new Font("맑은 고딕", 9f),
            });
        }
    }
}
