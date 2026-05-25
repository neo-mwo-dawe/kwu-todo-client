// 통합 데이터 모델 — 지원(기반) + 유빈(UI 확장 필드) 병합
// Python 서버 GenerateTodoResponse와 매핑

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace KwuTodoAI
{
    public class TodoItem
    {
        // ── 기본 필드 ──────────────────────────────────────────────
        [JsonPropertyName("id")]           public int          Id            { get; set; }
        [JsonPropertyName("title")]        public string       Title         { get; set; } = "";
        [JsonPropertyName("priority")]     public string       Priority      { get; set; } = "보통";
        [JsonPropertyName("dueDate")]      public string       DueDate       { get; set; } = "";
        [JsonPropertyName("dDay")]         public int          DDay          { get; set; }
        [JsonPropertyName("type")]         public string       Type          { get; set; } = "기타";
        [JsonPropertyName("reason")]       public string       Reason        { get; set; } = "";
        [JsonPropertyName("actionItems")]  public List<string> ActionItems   { get; set; } = new();
        [JsonPropertyName("isTeamWork")]   public bool         IsTeamWork    { get; set; }
        [JsonPropertyName("isCompleted")]  public bool         IsCompleted   { get; set; }
        [JsonPropertyName("isAIGenerated")]public bool         IsAIGenerated { get; set; }
        [JsonPropertyName("source")]       public string       Source        { get; set; } = "";

        // ── UI 표시용 계산 속성 (JSON 제외) ────────────────────────
        [JsonIgnore]
        public string DDayText =>
            DDay == 0 ? "D-DAY" : DDay > 0 ? $"D-{DDay}" : $"D+{Math.Abs(DDay)}";

        [JsonIgnore]
        public string DDayLabel => DDayText;

        [JsonIgnore]
        public string TypeIcon => Type switch
        {
            "시험"     => "📝",
            "과제"     => "📋",
            "발표"     => "🎤",
            "학사행정"  => "🏫",
            _          => "📌",
        };

        // 우선순위 색상 (카드 텍스트용)
        [JsonIgnore]
        public Color PriorityColor => Priority switch
        {
            "긴급" => Color.FromArgb(220, 60,  60),
            "높음" => Color.FromArgb(230, 130, 30),
            "보통" => Color.FromArgb(60,  140, 220),
            _      => Color.FromArgb(130, 140, 155),
        };

        // 마감 임박도 색상 (카드 왼쪽 바용)
        [JsonIgnore]
        public Color DeadlineColor => DDay switch
        {
            <= 0 => Color.FromArgb(220, 60,  60),   // 지남 / D-Day
            <= 3 => Color.FromArgb(230, 130, 30),   // 3일 이내
            <= 7 => Color.FromArgb(60,  140, 220),  // 일주일 이내
            _    => Color.FromArgb(130, 140, 155),  // 여유
        };

        // ── 편의 메서드 ───────────────────────────────────────────
        public void RecalculateDDay()
        {
            if (DateTime.TryParse(DueDate, out var due))
                DDay = (int)(due.Date - DateTime.Today).TotalDays;
        }

        public static TodoItem Create(string title, string priority, string dueDate,
            string reason = "", bool isAI = false)
        {
            var item = new TodoItem
            {
                Title = title, Priority = priority, DueDate = dueDate,
                Reason = reason, IsCompleted = false, IsAIGenerated = isAI,
            };
            item.RecalculateDDay();
            return item;
        }
    }

    // ── Python GenerateTodoResponse와 1:1 매핑 ────────────────────
    public class TodoResponse
    {
        [JsonPropertyName("todos")]              public List<TodoItem> Todos            { get; set; } = new();
        [JsonPropertyName("generated_count")]    public int            GeneratedCount   { get; set; }
        [JsonPropertyName("based_on_schedules")] public List<string>   BasedOnSchedules { get; set; } = new();
    }

    // ── 통계 (클라이언트에서 계산) ────────────────────────────────
    public class TodoStatistics
    {
        public int Total       { get; set; }
        public int Urgent      { get; set; }
        public int High        { get; set; }
        public int Exams       { get; set; }
        public int Assignments { get; set; }
    }
}
