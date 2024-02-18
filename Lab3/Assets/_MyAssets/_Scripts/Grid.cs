using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum TileType : int
{
    GRASS,
    WATER,
    MUD,
    STONE,
    INVALID,
}

// Homework hints:
// Associate a terrain cost with each unique tile type ie grass = 10, water = 50, etc
// Furthermore, the total cost of each tile should be terrain cost + distance score
// where distance score is the distance from the current tile to the goal tile
// (you will need to define a goal tile)
public class Grid : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    List<List<GameObject>> grid = new List<List<GameObject>>();
    int rowCount = 10;      // vertical tile count
    int colCount = 18;      // horizontal tile count
    Vector2Int goalTile;
    int[,] tiles =
    {
        { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
        { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }
    };

    void Start()
    {
        float xStart = -8.379f;    // left (-x)
        float yStart = -4.491f;    // bottom (-y)
        float x = xStart;
        float y = yStart;

        for (int row = 0; row < rowCount; row++)
        {
            grid.Add(new List<GameObject>());
            for (int col = 0; col < colCount; col++)
            {
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector3(x, y);
                x += 1.0f;
                grid[row].Add(tile);
            }
            x = xStart;
            y += 1.0f;
        }

       
        goalTile = new Vector2Int(colCount - 1, rowCount - 1);
    }

    void Update()
    {
        ColorGrid();
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cell = WorldToGrid(mouse);
        grid[cell.y][cell.x].GetComponent<SpriteRenderer>().color = TileColor(TileType.INVALID);

        grid[cell.y][cell.x].GetComponent<SpriteRenderer>().color = TileColor(TileType.INVALID);
        grid[cell.y][cell.x].GetComponent<SpriteRenderer>().color = TileColor(TileType.INVALID);
        grid[cell.y][cell.x].GetComponent<SpriteRenderer>().color = TileColor(TileType.INVALID);
        grid[cell.y][cell.x].GetComponent<SpriteRenderer>().color = TileColor(TileType.INVALID);

        Costs(cell, goalTile, (TileType)tiles[cell.y, cell.x]);
    }

    void ColorGrid()
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                GameObject tile = grid[row][col];
                TileType type = (TileType)tiles[row, col];
                tile.GetComponent<SpriteRenderer>().color = TileColor(type);
            }
        }
    }

    Color TileColor(TileType type)
    {
        Color color = Color.white;
        switch (type)
        {
            case TileType.GRASS:
                color = Color.green;
                break;
            case TileType.WATER:
                color = Color.blue;
                break;
            case TileType.MUD:
                color = Color.red;
                break;
            case TileType.STONE:
                color = Color.grey;
                break;
            case TileType.INVALID:
                color = Color.magenta;
                break;
        }
        return color;
    }

    List<GameObject> Neighbours(Vector2Int cell)
    {
        List<GameObject> neighbours = new List<GameObject>();

        for (int row = -1; row <= 1; row++)
        {
            for (int col = -1; col <= 1; col++)
            {
                int newRow = cell.y + row;
                int newCol = cell.x + col;

                if (newRow >= 0 && newRow < rowCount && newCol >= 0 && newCol < colCount &&
                    (row != 0 || col != 0))
                {
                    neighbours.Add(grid[newRow][newCol]);
                }
            }
        }

        return neighbours;
    }

    float TerrainCost(TileType type)
    {
        switch (type)
        {
            case TileType.GRASS:
                return 10.0f;
            case TileType.WATER:
                return 50.0f;
            case TileType.MUD:
                return 30.0f;
            case TileType.STONE:
                return 20.0f;
            default:
                return Mathf.Infinity;
        }
    }

    float DistanceCost(Vector2Int start, Vector2Int end)
    {
        return Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
    }

    float TotalCost(Vector2Int start, Vector2Int end, TileType type)
    {
        float terrainCost = TerrainCost(type);
        float distanceCost = DistanceCost(start, end);
        return terrainCost + distanceCost;
    }

    void Costs(Vector2Int currentCell, Vector2Int goal, TileType currentType)
    {
        List<GameObject> neighbours = Neighbours(currentCell);
        foreach (var neighbour in neighbours)
        {
            Vector2Int neighbourCell = WorldToGrid(neighbour.transform.position);
            TileType neighbourType = (TileType)tiles[neighbourCell.y, neighbourCell.x];
            float cost = TotalCost(neighbourCell, goal, neighbourType);
            Debug.Log("Cost for neighbor at (" + neighbourCell.x + ", " + neighbourCell.y + "): " + cost);
        }
    }

    Vector2Int WorldToGrid(Vector2 position)
    {
        Vector2Int cell = new Vector2Int((int)position.x, (int)position.y);
        cell.x = Mathf.Clamp(cell.x, 0, colCount - 1);
        cell.y = Mathf.Clamp(cell.y, 0, rowCount - 1);
        return cell;
    }

    Vector2 GridToWorld(Vector2Int cell)
    {
        cell.x = Mathf.Clamp(cell.x, 0, colCount - 1);
        cell.y = Mathf.Clamp(cell.y, 0, rowCount - 1);
        return new Vector2(cell.x + 0.5f, cell.y + 0.5f);
    }
}