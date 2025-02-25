using Microsoft.AspNetCore.Mvc;
using TreeApi.Data;
using TreeApi.Models;

namespace TreeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NodesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/nodes/get
        [HttpPost("get")]
        public IActionResult GetTree([FromBody] string treeName)
        {
            var tree = _context.Nodes.Where(n => n.TreeId == treeName.GetHashCode()).ToList();
            return Ok(tree);
        }

        // POST: api/nodes/create
        [HttpPost("create")]
        public IActionResult CreateNode([FromBody] Node node)
        {
            if (string.IsNullOrEmpty(node.Name))
                throw new SecureException("Node name is required");

            _context.Nodes.Add(node);
            _context.SaveChanges();
            return Ok(node);
        }
    }
}