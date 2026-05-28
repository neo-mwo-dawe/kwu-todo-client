// 통합 데이터 모델 — 지원(기반) + 유빈(UI 확장 필드) 병합
// Python 서버 GenerateTodoResponse와 필드명 일치
//
// ─────────────────────────────────────────────────────────────────────
// [변경 사항] — 원본 (지원의 develop 브랜치) 대비
//   1. PriorityLabel  : 서버의 Priority 문자열 기반 → "마감일(DDay) 기반"으로 변경
//                       (D≤3 긴급 / D≤7 높음 / D≤14 안전 / 그 외 낮음)
//   2. PriorityColor  : Priority 문자열 기반 → 동일한 DDay 기반 4단계 색상
//   3. DeadlineColor  : 별도 4단계 → PriorityColor 와 동일 (단일 진실 원천)
//   ※ 변경 위치마다 "[변경]" 주석 표시
// ─────────────────────────────────────────────────────────────────────

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace KwuTodoAI
{
    public class TodoItem
    {
        // ── 서버 응답 필드 (Python snake_case → C# camelCase) ────────
        // Python TodoResponse: id(str UUID), title, due_date, priority,
        //                      category, source_event, is_done, created_at
        [JsonPropertyName("id")]           public string       Id            { get; set; } = "";
        [JsonPropertyName("title")]        public string       Title         { get; set; } = "";
        [JsonPropertyName("priority")]     public string       Priority      { get; set; } = "medium";  // "high"/"medium"/"low"
        [JsonPropertyName("due_date")]     public string       DueDate       { get; set; } = "";        // "YYYY-MM-DD"
        [JsonPropertyName("category")]     public string       Category      { get; set; } = "기타";
        [JsonPropertyName("source_event")] public string       Source        { get; set; } = "";
        [JsonPropertyName("is_done")]      public bool         IsCompleted   { get; set; }

        // ── 로컬 전용 필드 (서버에 없음, DataManager 로컬 저장/UI 용) ─
        [JsonPropertyName("dDay")]         public int          DDay          { get; set; }
        [JsonPropertyName("type")]         public string       Type          { get; set; } = "기타";
        [JsonPropertyName("reason")]       public string       Reason        { get; set; } = "";
        [JsonPropertyName("actionItems")]  public List<string> ActionItems   { get; set; } = new();
        [JsonPropertyName("isTeamWork")]   public bool         IsTeamWork    { get; set; }
        [JsonPropertyName("isAIGenerated")]public bool         IsAIGenerated { get; set; }

        // ── UI 표시용 계산 속성 (JSON 제외) ────────────────────────────
        // [변경] 원본은 Priority 문자열("high"/"medium"/"low")로 라벨 결정
        //        → 마감일(DDay) 기준으로 변경 (긴급/높음/안전/낮음)
        // ※ 우선순위는 "마감일(DDay)" 기준으로 자동 계산해 표시합니다.
        //   - D-DAY ~ D-3   : 긴급 (빨강)
        //   - D-4   ~ D-7   : 높음 (주황)
        //   - D-8   ~ D-14  : 안전 (초록)
        //   - 그 이상       : 낮음 (회색)

        [JsonIgnore]
        public string PriorityLabel => DDay switch
        {
            <= 3  => "긴급",
            <= 7  => "높음",
            <= 14 => "안전",
            _     => "낮음",
        };

        [JsonIgnore]
        public string DDayText =>
            DDay == 0 ? "D-DAY" : DDay > 0 ? $"D-{DDay}" : $"D+{Math.Abs(DDay)}";

        [JsonIgnore]
        public string TypeIcon => Type switch
        {
            "시험"    => "📝",
            "과제"    => "📋",
            "발표"    => "🎤",
            "학사행정" => "🏫",
            _         => "📌",
        };

        // ── 우선순위 색상 — 마감일(DDay) 기준 ─────────────────────────
        // [변경] 원본은 Priority 문자열로 색상 결정 → DDay 기준으로 변경
        //   긴급(빨강), 높음(주황), 안전(초록), 낮음(회색)
        [JsonIgnore]
        public Color PriorityColor => DDay switch
        {
            <= 3  => Color.FromArgb(220, 60,  60),    // 긴급 (빨강)
            <= 7  => Color.FromArgb(230, 130, 30),    // 높음 (주황)
            <= 14 => Color.FromArgb( 55, 175, 115),   // 안전 (초록)
            _     => Color.FromArgb(130, 140, 155),   // 낮음 (회색)
        };

        // 마감 임박도 색상 (카드 왼쪽 바용) — PriorityColor와 동일 기준
        // [변경] 원본은 별도 switch로 색을 계산 → 중복 제거, PriorityColor 재사용
        [JsonIgnore]
        public Color DeadlineColor => PriorityColor;

        // ── 편의 메서드 ───────────────────────────────────────────────
        public void RecalculateDDay()
        {
            if (DateTime.TryParse(DueDate, out var due))
                DDay = (int)(due.Date - DateTime.Today).TotalDays;
        }

        // 새 TODO 로컬 생성용 팩토리 (수동 추가 폼에서 사용)
        public static TodoItem Create(string title, string priority, string dueDate,
            string reason = "", string type = "기타", bool isAI = false)
        {
            var item = new TodoItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = title, Priority = priority, DueDate = dueDate,
                Type = type, Reason = reason, IsCompleted = false, IsAIGenerated = isAI,
            };
            item.RecalculateDDay();
            return item;
        }
    }

    // ── Python GenerateTodoResponse와 1:1 매핑 ─────────────────────────
    public class TodoResponse
    {
        [JsonPropertyName("todos")]              public List<TodoItem> Todos            { get; set; } = new();
        [JsonPropertyName("generated_count")]    public int            GeneratedCount   { get; set; }
        [JsonPropertyName("based_on_schedules")] public List<string>   BasedOnSchedules { get; set; } = new();
    }

    // ── 통계 (클라이언트에서 계산) ─────────────────────────────────────
    public class TodoStatistics
    {
        public int Total       { get; set; }
        public int Urgent      { get; set; }
        public int High        { get; set; }
        public int Exams       { get; set; }
        public int Assignments { get; set; }
    }
}
