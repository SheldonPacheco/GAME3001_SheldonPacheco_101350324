using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    private class Node
    {
        public Vector2Int position;
        public float startCost;
        public float heuristicCost;
        public Node node;

        public Node(Vector2Int position, float startCost, float heuristicCost, Node node = null)
        {
            this.position = position;
            this.startCost = startCost;
            this.heuristicCost = heuristicCost;
            this.node = node;
        }

        public float totalCost => startCost + heuristicCost;
    }

    public static List<List<Vector2Int>> FindPaths(Vector2Int start, Vector2Int goal, float[,] costs, TileType[,] tileTypes, int maxPaths = 3)
    {
        List<List<Vector2Int>> totalPaths = new List<List<Vector2Int>>();

        if (!CheckValidPositions(start, costs) || !CheckValidPositions(goal, costs))
            return totalPaths;

        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        int pathsFound = 0;

        Node startNode = new Node(start, 0, CalculateHeuristic(start, goal));
        Queue<Node> openSet = new Queue<Node>();
        openSet.Enqueue(startNode);

        while (openSet.Count > 0 && pathsFound < maxPaths)
        {
            Node currentNode = openSet.Dequeue();

            if (currentNode.position == goal)
            {
                totalPaths.Add(CreatePath(currentNode));
                pathsFound++;
                continue;
            }

            closedSet.Add(currentNode.position);

            foreach (Vector2Int neighbor in GetNeighbors(currentNode.position, costs, tileTypes))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float totalDistanceCost = currentNode.startCost + costs[neighbor.y, neighbor.x];
                Node neighborNode = new Node(neighbor, totalDistanceCost, CalculateHeuristic(neighbor, goal), currentNode);

                openSet.Enqueue(neighborNode);
                closedSet.Add(neighbor);
            }
        }

        return totalPaths;
    }

    private static List<Vector2Int> GetNeighbors(Vector2Int position, float[,] costs, TileType[,] tileTypes)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
        {
            Vector2Int neighborPos = position + dir;
            if (CheckValidPositions(neighborPos, costs) &&
                tileTypes[neighborPos.y, neighborPos.x] != TileType.STONE &&
                tileTypes[neighborPos.y, neighborPos.x] != TileType.WATER)
            {
                neighbors.Add(neighborPos);
            }
        }

        return neighbors;
    }

    private static List<Vector2Int> CreatePath(Node node)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        while (node != null)
        {
            path.Add(node.position);
            node = node.node;
        }

        path.Reverse();
        return path;
    }

    private static bool CheckValidPositions(Vector2Int position, float[,] costs)
    {
        int width = costs.GetLength(1);
        int height = costs.GetLength(0);
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    private static float CalculateHeuristic(Vector2Int from, Vector2Int to)
    {
        return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
    }
}
