// 통합 데이터 모델 — 지원(기반) + 유빈(UI 확장 필드) 병합
// Python 서버 GenerateTodoResponse와 필드명 일치

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
        // 서버는 "high"/"medium"/"low" 반환 → UI에 한국어로 표시
        [JsonIgnore]
        public string PriorityLabel => Priority switch
        {
            "high"   => "높음",
            "medium" => "보통",
            "low"    => "낮음",
            _        => Priority,   // 이미 한국어이거나 알 수 없는 값은 그대로
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

        // 우선순위 색상 — 영어/한국어 모두 처리
        [JsonIgnore]
        public Color PriorityColor => Priority switch
        {
            "high"   or "높음" or "긴급" => Color.FromArgb(230, 130, 30),
            "medium" or "보통"           => Color.FromArgb(60,  140, 220),
            "low"    or "낮음"           => Color.FromArgb(130, 140, 155),
            _                            => Color.FromArgb(130, 140, 155),
        };

        // 마감 임박도 색상 (카드 왼쪽 바용)
        [JsonIgnore]
        public Color DeadlineColor => DDay switch
        {
            <= 0 => Color.FromArgb(220, 60,  60),
            <= 3 => Color.FromArgb(230, 130, 30),
            <= 7 => Color.FromArgb(60,  140, 220),
            _    => Color.FromArgb(130, 140, 155),
        };

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
