namespace TreeApi.Models
{
    public class Node
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public int? ParentNodeId { get; set; } 
        public int TreeId { get; set; } 

        public virtual Node? ParentNode { get; set; } 
        public virtual ICollection<Node> Children { get; set; } = new List<Node>(); 
    }
}