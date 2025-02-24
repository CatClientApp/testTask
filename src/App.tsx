import React, { useState, useEffect } from 'react';
import Tree from './components/Tree';
import Modal from './components/Modal';
import {
  createTreeNode,
  renameTreeNode,
  deleteTreeNode,
  getTreeData,
} from './api'

function App() {
  // Состояние для хранения данных дерева
  const [treeData, setTreeData] = useState<any[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [currentNode, setCurrentNode] = useState<{ id?: number; name?: string; parentId?: number | null }>({});
  const [treeName] = useState('unique-tree-name'); // Уникальное имя дерева (например, GUID)

  // Загрузка данных дерева при монтировании компонента
  useEffect(() => {
    fetchTreeData();
  }, []);

  // Функция для загрузки данных дерева
  const fetchTreeData = async () => {
    try {
      const data = await getTreeData(treeName);
      setTreeData(data || []);
    } catch (error) {
      console.error('Ошибка при загрузке дерева:', error);
    }
  };

  // Функция для создания нового узла
  const handleCreateNode = async (nodeName: string) => {
    try {
      await createTreeNode(treeName, currentNode.parentId || null, nodeName);
      fetchTreeData(); // Обновляем дерево после создания
      setIsModalOpen(false); // Закрываем модальное окно
    } catch (error) {
      console.error('Ошибка при создании узла:', error);
    }
  };

  // Функция для редактирования узла
  const handleEditNode = async (id: number, newName: string) => {
    try {
      await renameTreeNode(treeName, id, newName);
      fetchTreeData(); // Обновляем дерево после редактирования
    } catch (error) {
      console.error('Ошибка при редактировании узла:', error);
    }
  };

  // Функция для удаления узла
  const handleDeleteNode = async (id: number) => {
    try {
      await deleteTreeNode(treeName, id);
      fetchTreeData(); // Обновляем дерево после удаления
    } catch (error) {
      console.error('Ошибка при удалении узла:', error);
    }
  };

  return (
    <div className="p-4 md:p-8">
      <h1 className="text-xl font-bold">Tree Editor</h1>

      {/* Кнопка для создания корневого узла */}
      <button
        className="mb-4 px-4 py-2 bg-green-500 text-white rounded"
        onClick={() => {
          setCurrentNode({ parentId: null });
          setIsModalOpen(true);
        }}
      >
        Create Root Node
      </button>

      {/* Компонент дерева */}
      <Tree
        nodes={treeData}
        onEdit={(id, newName) => handleEditNode(id, newName)}
        onDelete={(id) => handleDeleteNode(id)}
      />

      {/* Модальное окно */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={(name) => handleCreateNode(name)}
      />
    </div>
  );
}

export default App;