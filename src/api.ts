import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'https://test.vmarmysh.com',
});

// Получение данных дерева
export const getTreeData = async (treeName: string) => {
    const response = await apiClient.post('/api.user.tree.get', { treeName });
    return response.data;
};

// Создание узла
export const createTreeNode = async (
    treeName: string,
    parentNodeId: number | null,
    nodeName: string
) => {
    const response = await apiClient.post('/api.user.tree.node.create', {
        treeName,
        parentNodeId,
        nodeName,
    });
    return response.data;
};

// Редактирование узла
export const renameTreeNode = async (
    treeName: string,
    nodeId: number,
    newNodeName: string
) => {
    const response = await apiClient.post('/api.user.tree.node.rename', {
        treeName,
        nodeId,
        newNodeName,
    });
    return response.data;
};

// Удаление узла
export const deleteTreeNode = async (treeName: string, nodeId: number) => {
    const response = await apiClient.post('/api.user.tree.node.delete', {
        treeName,
        nodeId,
    });
    return response.data;
};