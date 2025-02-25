namespace TreeApi.Models
{
    public class Journal
    {
        public long Id { get; set; }
        public string EventId { get; set; } = string.Empty; // Инициализация значения по умолчанию
        public DateTime Timestamp { get; set; }
        public string QueryParameters { get; set; } = string.Empty; // Инициализация значения по умолчанию
        public string BodyParameters { get; set; } = string.Empty; // Инициализация значения по умолчанию
        public string StackTrace { get; set; } = string.Empty; // Инициализация значения по умолчанию
        public string Text { get; set; } = string.Empty; // Инициализация значения по умолчанию
    }
}