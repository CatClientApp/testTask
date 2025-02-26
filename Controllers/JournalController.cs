using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TreeApi.Data;
using TreeApi.Models;
using TreeApi.Exceptions;

namespace TreeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JournalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JournalController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/journal/getRange
        [HttpPost("getRange")]
        public IActionResult GetRange([FromQuery] int skip, [FromQuery] int take, [FromBody] JournalFilter filter)
        {
            try
            {
                // Проверка параметров пагинации
                if (skip < 0 || take <= 0)
                    return BadRequest("Invalid skip or take values.");

                // Фильтрация записей
                var query = _context.Journals.AsQueryable();
                if (filter.From.HasValue)
                    query = query.Where(j => j.Timestamp >= filter.From.Value);
                if (filter.To.HasValue)
                    query = query.Where(j => j.Timestamp <= filter.To.Value);
                if (!string.IsNullOrEmpty(filter.Search))
                    query = query.Where(j => j.QueryParameters.Contains(filter.Search) || j.BodyParameters.Contains(filter.Search));

                // Получение данных
                var total = query.Count();
                var items = query
                    .OrderByDescending(j => j.Timestamp) // Сортировка по времени (самые новые записи первыми)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return Ok(new
                {
                    skip,
                    count = total,
                    items
                });
            }
            catch (SecureException ex)
            {
                // Логирование ошибки
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/journal/getSingle
        [HttpPost("getSingle")]
        public IActionResult GetSingle([FromQuery] long id)
        {
            try
            {
                var journalEntry = _context.Journals.Find(id);
                if (journalEntry == null)
                    throw new SecureException("Journal entry not found");

                return Ok(journalEntry);
            }
            catch (SecureException ex)
            {
                // Логирование ошибки
                return NotFound(ex.Message); // Возвращаем 404 Not Found
            }
        }
    }

    public class JournalFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Search { get; set; }
    }
}