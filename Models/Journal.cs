namespace TreeApi.Models
{
    public class Journal
    {
        public long Id { get; set; }
        public string RequestId { get; set; } = string.Empty; 
        public string EventId { get; set; } = string.Empty; 
        public DateTime Timestamp { get; set; } 
        public string QueryParameters { get; set; } = string.Empty; 
        public string BodyParameters { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty; 
        public string Text { get; set; } = string.Empty;
    }
}