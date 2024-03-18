using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using Utils;
public enum TileType : int
{
    GRASS,
    WATER,
    MUD,
    STONE,
    INVALID,
}

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

    [SerializeField] Vector2Int goal;
    [SerializeField] Vector2Int playerStart;
    [SerializeField] GameObject playerOutline;
    [SerializeField] Transform player;
    [SerializeField] Transform viewer;
    int currentIndex = 0;
    int nextIndex = 1;
    float t = 0.0f;

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

        PriorityQueue<Vector2Int, float> pq = new PriorityQueue<Vector2Int, float>();
        pq.Enqueue(new Vector2Int(1, 1), 3.0f);
        pq.Enqueue(new Vector2Int(2, 2), 2.0f);
        pq.Enqueue(new Vector2Int(3, 3), 1.0f);

        while (pq.Count > 0)
        {
            Vector2Int cell = pq.Dequeue();
            Debug.Log(cell);
        }
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

        // Additional logic for following path
        FollowPath();
        CheckVisibility();
        // Revert each tile to white
        int rowCount = tiles.GetLength(0);
        int colCount = tiles.GetLength(1);
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                grid[row][col].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        // Colors ray & player green if viewer can see player, otherwise red
        Vector3 toPlayer = (player.position - viewer.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(viewer.position, toPlayer, 1000.0f);
        bool playerHit = hit && hit.collider.CompareTag("Player");
        Color hitColor = playerHit ? Color.green : Color.red;
        playerOutline.GetComponent<SpriteRenderer>().color = hitColor;
        Debug.DrawLine(viewer.position, viewer.position + toPlayer * 1000.0f, hitColor);

        // Figure out the cells that the player & viewer are in
        Vector2Int playerCell = Pathing.WorldToGrid(player.position, tiles);
        Vector2Int viewerCell = Pathing.WorldToGrid(viewer.position, tiles);
        grid[playerCell.y][playerCell.x].GetComponent<SpriteRenderer>().color = Color.magenta;
        grid[viewerCell.y][viewerCell.x].GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    void CheckVisibility()
    {
        // Iterate through all tiles
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                Vector2Int tilePosition = new Vector2Int(col, row);
                bool isVisible = IsVisibleFromPlayer(tilePosition);
                Color tileColor = isVisible ? Color.green : Color.red;
                grid[row][col].GetComponent<SpriteRenderer>().color = tileColor;
            }
        }
    }

    bool IsVisibleFromPlayer(Vector2Int tilePosition)
    {
        Vector2Int playerTile = WorldToGrid(player.position);
        List<Vector2Int> line = Utils.Utils.Line(playerTile, tilePosition);

        // Check if any tile along the line of sight is an obstacle
        foreach (Vector2Int point in line)
        {
            TileType tileType = (TileType)tiles[point.y, point.x];
            if (tileType == TileType.STONE) // Adjust for your obstacle type
            {
                return false; // Tile is not visible
            }
        }

        return true; // Tile is visible
    }
    void ColorGrid()
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                GameObject tile = grid[row][col];
                TileType type = (TileType)tiles[row, col];

                // Check if the tile is visible from the player's position
                bool isVisible = IsVisibleFromPlayer(new Vector2Int(col, row));

                // Set the color based on visibility
                if (isVisible)
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.green; // Green if visible
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.red; // Red if not visible
                }
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

    void FollowPath()
    {
        // Revert each tile to its type-based colour
        int rows = tiles.GetLength(0);
        int cols = tiles.GetLength(1);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                TileType type = (TileType)tiles[row, col];
                GameObject tile = grid[row][col];
                tile.GetComponent<SpriteRenderer>().color = TileColor(type);
            }
        }

        List<Vector2Int> path = Pathing.FloodFill(startTile, goalTile, tiles, 16);
        Vector2Int current = path[currentIndex];
        Vector2Int next = path[nextIndex];

        Vector3 currentWorld = Pathing.GridToWorld(current, tiles);
        Vector3 nextWorld = Pathing.GridToWorld(next, tiles);
        player.position = Vector3.Lerp(currentWorld, nextWorld, t);
        t += Time.deltaTime;

        if (Vector3.Distance(player.position, nextWorld) <= 0.01f && next != goalTile)
        {
            currentIndex++;
            nextIndex++;
            t = 0.0f;
        }

        // Render floodfill/path in purple
        foreach (Vector2Int cell in path)
        {
            GameObject tile = grid[cell.y][cell.x];
            tile.GetComponent<SpriteRenderer>().color = TileColor(TileType.INVALID);
        }

        GameObject startTileObj = grid[startTile.y][startTile.x];
        GameObject goalTileObj = grid[goalTile.y][goalTile.x];
        startTileObj.GetComponent<SpriteRenderer>().color = Color.red;
        goalTileObj.GetComponent<SpriteRenderer>().color = Color.cyan;
    }
}