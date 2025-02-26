using Microsoft.AspNetCore.Mvc;
using TreeApi.Data;
using TreeApi.Models;
using TreeApi.Services;
using System.Linq;

namespace TreeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JournalService _journalService;

        public NodesController(AppDbContext context, JournalService journalService)
        {
            _context = context;
            _journalService = journalService;
        }

        // POST: api/nodes/get
        [HttpPost("get")]
        public IActionResult GetTree([FromBody] TreeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TreeName))
                    return BadRequest("Tree name is required.");

                int treeId = request.TreeName.GetHashCode();
                var nodes = _context.Nodes.Where(n => n.TreeId == treeId).ToList();

                if (!nodes.Any())
                {
                    var root = new Node
                    {
                        Name = "Root",
                        TreeId = treeId,
                        ParentNodeId = null
                    };
                    _context.Nodes.Add(root);
                    _context.SaveChanges();

                    // Логирование создания корневого узла
                    _journalService.LogAction("get", $"Created root node for tree '{request.TreeName}'", request);

                    return Ok(new { id = root.Id, name = root.Name, children = new List<Node>() });
                }

                var rootNodes = nodes.Where(n => n.ParentNodeId == null).ToList();
                var tree = BuildTree(nodes, rootNodes);

                // Логирование успешного получения дерева
                _journalService.LogAction("get", $"Retrieved tree '{request.TreeName}'", request);

                return Ok(tree);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _journalService.LogError("get", $"Error retrieving tree '{request.TreeName}'", request, ex);
                throw;
            }
        }

        // POST: api/nodes/create
        [HttpPost("create")]
        public IActionResult CreateNode([FromBody] NodeCreationRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TreeName) || string.IsNullOrEmpty(request.NodeName))
                    return BadRequest("Tree name and node name are required.");

                int treeId = request.TreeName.GetHashCode();

                if (request.ParentNodeId == null && _context.Nodes.Any(n => n.TreeId == treeId && n.ParentNodeId == null))
                    return BadRequest("A root node already exists for this tree.");

                if (_context.Nodes.Any(n => n.ParentNodeId == request.ParentNodeId && n.Name == request.NodeName && n.TreeId == treeId))
                    return BadRequest("Node name must be unique among siblings.");

                var newNode = new Node
                {
                    Name = request.NodeName,
                    TreeId = treeId,
                    ParentNodeId = request.ParentNodeId
                };

                _context.Nodes.Add(newNode);
                _context.SaveChanges();

                // Логирование создания узла
                _journalService.LogAction("create", $"Created node '{request.NodeName}' in tree '{request.TreeName}'", request);

                return Ok(newNode);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _journalService.LogError("create", $"Error creating node '{request.NodeName}' in tree '{request.TreeName}'", request, ex);
                throw;
            }
        }

        // POST: api/nodes/delete
        [HttpPost("delete")]
        public IActionResult DeleteNode([FromBody] NodeDeletionRequest request)
        {
            try
            {
                int treeId = request.TreeName.GetHashCode();
                var nodeToDelete = _context.Nodes.FirstOrDefault(n => n.Id == request.NodeId && n.TreeId == treeId);
                if (nodeToDelete == null)
                    return NotFound("Node not found.");

                DeleteNodeAndChildren(nodeToDelete);
                _context.SaveChanges();

                // Логирование удаления узла
                _journalService.LogAction("delete", $"Deleted node with ID '{request.NodeId}' from tree '{request.TreeName}'", request);

                return Ok("Node deleted successfully.");
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _journalService.LogError("delete", $"Error deleting node with ID '{request.NodeId}' from tree '{request.TreeName}'", request, ex);
                throw;
            }
        }

        // POST: api/nodes/rename
        [HttpPost("rename")]
        public IActionResult RenameNode([FromBody] NodeRenameRequest request)
        {
            try
            {
                int treeId = request.TreeName.GetHashCode();
                var nodeToRename = _context.Nodes.FirstOrDefault(n => n.Id == request.NodeId && n.TreeId == treeId);
                if (nodeToRename == null)
                    return NotFound("Node not found.");

                if (_context.Nodes.Any(n => n.ParentNodeId == nodeToRename.ParentNodeId && n.Name == request.NewNodeName && n.TreeId == treeId))
                    return BadRequest("New node name must be unique among siblings.");

                nodeToRename.Name = request.NewNodeName;
                _context.SaveChanges();

                // Логирование переименования узла
                _journalService.LogAction("rename", $"Renamed node with ID '{request.NodeId}' to '{request.NewNodeName}' in tree '{request.TreeName}'", request);

                return Ok(nodeToRename);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _journalService.LogError("rename", $"Error renaming node with ID '{request.NodeId}' in tree '{request.TreeName}'", request, ex);
                throw;
            }
        }

        private void DeleteNodeAndChildren(Node node)
        {
            var children = _context.Nodes.Where(n => n.ParentNodeId == node.Id).ToList();
            foreach (var child in children)
            {
                DeleteNodeAndChildren(child);
            }
            _context.Nodes.Remove(node);
        }

        private List<object> BuildTree(List<Node> allNodes, List<Node> currentLevel)
        {
            return currentLevel.Select(node => new
            {
                id = node.Id,
                name = node.Name,
                children = BuildTree(allNodes, allNodes.Where(n => n.ParentNodeId == node.Id).ToList())
            }).ToList<object>();
        }
    }

    public class TreeRequest
    {
        public required string TreeName { get; set; }
    }

    public class NodeCreationRequest
    {
        public required string TreeName { get; set; }
        public int? ParentNodeId { get; set; }
        public required string NodeName { get; set; }
    }

    public class NodeDeletionRequest
    {
        public required string TreeName { get; set; }
        public required int NodeId { get; set; }
    }

    public class NodeRenameRequest
    {
        public required string TreeName { get; set; }
        public required int NodeId { get; set; }
        public required string NewNodeName { get; set; }
    }
}