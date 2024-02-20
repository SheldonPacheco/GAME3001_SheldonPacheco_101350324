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
    Vector2Int startTile;
    TileType tileState = TileType.INVALID;  
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
        float xStart = -8.4983f;    // left (-x)
        float yStart = -4.4963f;    // bottom (-y)
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
        startTile = new Vector2Int(17, 4); //right most side
        goalTile = new Vector2Int(0, 4); //left most side
        tiles[startTile.y, startTile.x] = (int)TileType.STONE;
        tiles[goalTile.y, goalTile.x] = (int)TileType.STONE;
    }

    void Update()
    {
        ColorGrid();
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cell = WorldToGrid(mouse);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int state = (int)tileState;
            ++state;
            state %= (int)TileType.INVALID;
            tileState = (TileType)state;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            tileState = TileType.INVALID;
        }

        if (Input.GetKeyDown(KeyCode.Space) && tileState != TileType.INVALID)
        {
            tiles[cell.y, cell.x] = (int)tileState;
        }
        grid[cell.y][cell.x].GetComponent<SpriteRenderer>().color = TileColor((TileType)tileState);
        Debug.Log("Mouse Position: " + mouse + " - Grid Cell: " + cell);
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

    List<Vector2Int> Neighbours(Vector2Int cell)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        for (int row = -1; row <= 1; row++)
        {
            for (int col = -1; col <= 1; col++)
            {
                int newRow = cell.y + row;
                int newCol = cell.x + col;

                if ((row != 0 || col != 0) && IsValidPosition(new Vector2Int(newCol, newRow)))
                {
                    neighbours.Add(new Vector2Int(newCol, newRow));
                }
            }
        }

        return neighbours;
    }
    bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < colCount && position.y >= 0 && position.y < rowCount;
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
    float TotalCost(Vector2Int start, Vector2Int end, TileType type)
    {
        float terrainCost = TerrainCost(type);
        float distanceCost = Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
        return terrainCost + distanceCost;
    }

    void Costs(Vector2Int currentCell, Vector2Int goal, TileType currentType)
    {
        List<Vector2Int> neighbours = Neighbours(currentCell);
        foreach (var neighbourCell in neighbours)
        {
            TileType neighbourType = (TileType)tiles[neighbourCell.y, neighbourCell.x];
            float cost = TotalCost(neighbourCell, goal, neighbourType);
            Debug.Log("Cost for neighbor at (" + neighbourCell.x + ", " + neighbourCell.y + "): " + cost);
        }
    }
    Vector2Int WorldToGrid(Vector2 position)
    {
        int col = Mathf.FloorToInt((position.x + 8.4983f));
        int row = Mathf.FloorToInt((position.y + 4.4963f));

        col = Mathf.Clamp(col, 0, colCount - 1);
        row = Mathf.Clamp(row, 0, rowCount - 1);

        return new Vector2Int(col, row);
    }
}