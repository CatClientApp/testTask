namespace TreeApi.Models
{
    public class Journal
    {
        public long Id { get; set; } // Уникальный идентификатор записи (автоинкремент)
        public string RequestId { get; set; } = string.Empty; // Уникальный идентификатор запроса
        public string EventId { get; set; } = string.Empty; // ID события (например, "create", "delete")
        public DateTime Timestamp { get; set; } // Время создания записи
        public string QueryParameters { get; set; } = string.Empty; // Параметры запроса
        public string BodyParameters { get; set; } = string.Empty; // Параметры тела запроса
        public string StackTrace { get; set; } = string.Empty; // Стек вызовов (если есть ошибка)
        public string Text { get; set; } = string.Empty; // Текст события
    }
}