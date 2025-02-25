using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TreeApi.Data;
using TreeApi.Models;

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
            var query = _context.Journals.AsQueryable();

            if (filter.From.HasValue)
                query = query.Where(j => j.Timestamp >= filter.From.Value);
            if (filter.To.HasValue)
                query = query.Where(j => j.Timestamp <= filter.To.Value);
            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(j => j.QueryParameters.Contains(filter.Search) || j.BodyParameters.Contains(filter.Search));

            var total = query.Count();
            var items = query.Skip(skip).Take(take).ToList();

            return Ok(new
            {
                skip,
                count = total,
                items
            });
        }

        // POST: api/journal/getSingle
        [HttpPost("getSingle")]
        public IActionResult GetSingle([FromQuery] long id)
        {
            var journalEntry = _context.Journals.Find(id);
            if (journalEntry == null)
                throw new SecureException("Journal entry not found");

            return Ok(journalEntry);
        }
    }

    public class JournalFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Search { get; set; }
    }
}