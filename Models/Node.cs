namespace TreeApi.Models
{
    public class Node
    {
        public int Id { get; set; } // Уникальный идентификатор узла
        public string Name { get; set; } = string.Empty; // Имя узла
        public int? ParentNodeId { get; set; } // ID родительского узла (null для корневого узла)
        public int TreeId { get; set; } // Идентификатор дерева (хэш имени дерева)

        // Навигационные свойства
        public virtual Node? ParentNode { get; set; } // Родительский узел
        public virtual ICollection<Node> Children { get; set; } = new List<Node>(); // Дочерние узлы
    }
}