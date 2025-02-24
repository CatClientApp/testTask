import React, { useState } from 'react';

interface TreeNode {
    id: number;
    name: string;
    children?: TreeNode[];
}

const Tree: React.FC<{ nodes: TreeNode[]; onEdit: (id: number, newName: string) => void; onDelete: (id: number) => void }> = ({ nodes, onEdit, onDelete }) => {
    return (
        <ul className="space-y-2">
            {nodes.map((node) => (
                <li key={node.id} className="flex items-center">
                    <span>{node.name}</span>
                    <button
                        className="ml-2 text-blue-500"
                        onClick={() => onEdit(node.id, prompt('Enter new name') || '')}
                    >
                        Edit
                    </button>
                    <button
                        className="ml-2 text-red-500"
                        onClick={() => onDelete(node.id)}
                    >
                        Delete
                    </button>
                    {node.children && <Tree nodes={node.children} onEdit={onEdit} onDelete={onDelete} />}
                </li>
            ))}
        </ul>
    );
};

export default Tree;