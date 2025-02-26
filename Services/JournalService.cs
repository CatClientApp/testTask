using TreeApi.Data;
using TreeApi.Models;

namespace TreeApi.Services
{
    public class JournalService
    {
        private readonly AppDbContext _context;

        public JournalService(AppDbContext context)
        {
            _context = context;
        }

        public void LogAction(string eventId, string text, object request)
        {
            var requestId = Guid.NewGuid().ToString(); // Генерация уникального RequestId
            var journalEntry = new Journal
            {
                RequestId = requestId,
                EventId = eventId,
                Timestamp = DateTime.UtcNow,
                QueryParameters = request.ToString(), // Преобразуйте объект запроса в строку
                BodyParameters = System.Text.Json.JsonSerializer.Serialize(request),
                Text = text
            };

            _context.Journals.Add(journalEntry);
            _context.SaveChanges();
        }

        public void LogError(string eventId, string text, object request, Exception ex)
        {
            var requestId = Guid.NewGuid().ToString(); // Генерация уникального RequestId
            var journalEntry = new Journal
            {
                RequestId = requestId,
                EventId = eventId,
                Timestamp = DateTime.UtcNow,
                QueryParameters = request.ToString(), // Преобразуйте объект запроса в строку
                BodyParameters = System.Text.Json.JsonSerializer.Serialize(request),
                StackTrace = ex.StackTrace ?? string.Empty,
                Text = $"{text}. Error: {ex.Message}"
            };

            _context.Journals.Add(journalEntry);
            _context.SaveChanges();
        }
    }
}