using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TileType
{
    GRASS,
    WATER,
    MUD,
    STONE,
    INVALID,
}

public class Map : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;
    public TMP_Text instructionText;
    public TMP_Text pathCostText;

    private List<List<GameObject>> grid = new List<List<GameObject>>();
    private List<List<Vector2Int>> totalPaths = new List<List<Vector2Int>>();
    private TileType[,] tileTypes;

    private Vector2Int startTilePosition;
    private Vector2Int goalTilePosition;
    private float[,] tileCosts;
    public Actor actor;
    public GameObject actorObject;
    private static Vector3 startTileTransform;
    private bool debugViewActive = false;


    void Start()
    {
        CreateGrid();
        CreateTileMap();
        AssignCosts();       
    }

    void CreateGrid()
    {
        
        for (int row = 0; row < 10; row++)
        {
            grid.Add(new List<GameObject>());
            for (int col = 0; col < 10; col++)
            {
                GameObject tile = Instantiate(tilePrefab, gridParent);
                tile.transform.position = new Vector3(col, -row, 0); 

                grid[row].Add(tile);

            }
        }

        
        startTilePosition = new Vector2Int(9, 0);
        startTileTransform = new Vector3(startTilePosition.x, startTilePosition.y, 0);
        goalTilePosition = new Vector2Int(0, 9);

        Instantiate(actorObject, startTileTransform, Quaternion.identity);
        actor = Object.FindFirstObjectByType<Actor>();
        instructionText.text = " Instructions:\n 'F' find all paths.\n 'R' to reload.\n 'M' shortest path.\n 'H' debug view.\n";
        pathCostText.text = "Total path cost: ";
    }

    void CreateTileMap()
    {
        TileType[] tileTypesChoice = { TileType.GRASS, TileType.WATER, TileType.STONE, TileType.MUD };
        TileType[] tileTypesChoicePassable = { TileType.GRASS, TileType.MUD };

        int randomTile = Random.Range(0, tileTypesChoice.Length);
        int randomTile2 = Random.Range(0, tileTypesChoice.Length);
        int randomTile3 = Random.Range(0, tileTypesChoice.Length);
        int randomTile4 = Random.Range(0, tileTypesChoicePassable.Length);
        int randomTile5 = Random.Range(0, tileTypesChoice.Length);
        int randomTile6 = Random.Range(0, tileTypesChoicePassable.Length);
        int randomTile7 = Random.Range(0, tileTypesChoice.Length);
        int randomTile8 = Random.Range(0, tileTypesChoicePassable.Length);
        int randomTile9 = Random.Range(0, tileTypesChoice.Length);
        int randomTile10 = Random.Range(0, tileTypesChoicePassable.Length);
        int randomTile11 = Random.Range(0, tileTypesChoice.Length);
        int randomTile12 = Random.Range(0, tileTypesChoice.Length);
        int randomTile13 = Random.Range(0, tileTypesChoice.Length);
        int randomTile14 = Random.Range(0, tileTypesChoice.Length);
        int randomTile15 = Random.Range(0, tileTypesChoice.Length);
        int randomTile16 = Random.Range(0, tileTypesChoice.Length);
        int randomTile17 = Random.Range(0, tileTypesChoicePassable.Length);

        TileType randomTileType = tileTypesChoice[randomTile];
        TileType randomTileType2 = tileTypesChoice[randomTile2];
        TileType randomTileType3 = tileTypesChoice[randomTile3];
        TileType randomTileType4 = tileTypesChoicePassable[randomTile4];
        TileType randomTileType5 = tileTypesChoice[randomTile5];
        TileType randomTileType6 = tileTypesChoicePassable[randomTile6];
        TileType randomTileType7 = tileTypesChoice[randomTile7];
        TileType randomTileType8 = tileTypesChoicePassable[randomTile8];
        TileType randomTileType9 = tileTypesChoice[randomTile9];
        TileType randomTileType10 = tileTypesChoicePassable[randomTile10];
        TileType randomTileType11 = tileTypesChoice[randomTile11];
        TileType randomTileType12 = tileTypesChoice[randomTile12];
        TileType randomTileType13 = tileTypesChoice[randomTile13];
        TileType randomTileType14 = tileTypesChoice[randomTile14];
        TileType randomTileType15 = tileTypesChoice[randomTile15];
        TileType randomTileType16 = tileTypesChoice[randomTile16];
        TileType randomTileType17 = tileTypesChoicePassable[randomTile17];

        tileTypes = new TileType[10, 10]
        {
            { randomTileType, randomTileType, randomTileType, randomTileType5, randomTileType5, randomTileType5, randomTileType5, randomTileType, randomTileType, randomTileType8 },
            { randomTileType, randomTileType, randomTileType, randomTileType, randomTileType4, randomTileType4, randomTileType4, randomTileType4, randomTileType4, randomTileType8 },
            { randomTileType16, randomTileType16, randomTileType16, randomTileType9, randomTileType9, randomTileType9, randomTileType9, randomTileType13, randomTileType13, randomTileType13 },
            { randomTileType11, randomTileType11, randomTileType2, randomTileType9, randomTileType9, randomTileType9, randomTileType9, randomTileType13, randomTileType13, randomTileType13 },
            { randomTileType11, randomTileType11, randomTileType2, randomTileType2, randomTileType3, randomTileType3, randomTileType7, randomTileType7, randomTileType7, randomTileType7 },
            { randomTileType6, randomTileType17, randomTileType17, randomTileType17, randomTileType17, randomTileType3, randomTileType7, randomTileType7, randomTileType7, randomTileType7 },
            { randomTileType6, randomTileType17, randomTileType17, randomTileType17, randomTileType11, randomTileType11, randomTileType7, randomTileType7, randomTileType7, randomTileType7 },
            { randomTileType15, randomTileType15, randomTileType11, randomTileType11, randomTileType11, randomTileType11, randomTileType12, randomTileType12, randomTileType12, randomTileType10 },
            { randomTileType15, randomTileType15, randomTileType14, randomTileType14, randomTileType14, randomTileType14, randomTileType12, randomTileType12, randomTileType12, randomTileType10 },
            { randomTileType4, randomTileType14, randomTileType14, randomTileType14, randomTileType14, randomTileType14, randomTileType14, randomTileType12, randomTileType12, randomTileType10 },
        };
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                GameObject tile = grid[row][col];
                Tile tileComponent = tile.GetComponent<Tile>();
                tileComponent.SetTileType(tileTypes[row, col]);
            }
        }

    }

    void AssignCosts()
    {
        
        tileCosts = new float[10, 10];
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                switch (tileTypes[row, col])
                {
                    case TileType.GRASS:
                        tileCosts[row, col] = 10.0f;
                        break;
                    case TileType.WATER:
                        tileCosts[row, col] = Mathf.Infinity;
                        break;
                    case TileType.MUD:
                        tileCosts[row, col] = 50.0f;
                        break;
                    case TileType.STONE:
                        tileCosts[row, col] = Mathf.Infinity;
                        break;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FindAllPaths();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleDebugView();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            FollowShortestPath();
        }
    }

    void ToggleDebugView()
    {
        debugViewActive = !debugViewActive;

        
        foreach (List<GameObject> row in grid)
        {
            foreach (GameObject tile in row)
            {
                tile.GetComponent<Tile>().ToggleDebugView(debugViewActive);
            }
        }
    }

    void FindAllPaths()
    {
        totalPaths.Clear();
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++) 
            {
                List<List<Vector2Int>> newPaths = AStar.FindPaths(startTilePosition, goalTilePosition, tileCosts, tileTypes, 3);
                totalPaths.AddRange(newPaths);
            }
        }
        DisplayPaths();
    }

    void DisplayPaths()
    {

        
        foreach (var path in totalPaths)
        {
            foreach (var tilePosition in path)
            {
                
                GameObject tile = grid[tilePosition.y][tilePosition.x];
                SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                Color color = renderer.color;
                color.a = 0.8f; 
                renderer.color = color;
            }
        }
    }
    void FollowShortestPath()
    {
        
        List<Vector2Int> shortestPath = new List<Vector2Int>();
        float shortestPathCost = Mathf.Infinity;

        foreach (var path in totalPaths)
        {
            float pathCost = CalculatePathCost(path);
            if (pathCost < shortestPathCost)
            {
                shortestPathCost = pathCost;
                shortestPath = path;
            }
        }
        pathCostText.text = "Total path cost:\n " + shortestPathCost.ToString() + "\n Maps are random \n sorry.";
        actor.SetPath(shortestPath, tileTypes);
    }

    float CalculatePathCost(List<Vector2Int> path)
    {
        float totalCost = 0;
        foreach (Vector2Int tile in path)
        {
            totalCost += tileCosts[tile.y, tile.x];
        }
        return totalCost;
    }
    public static Vector3 GetStart()
    {
        return startTileTransform;
    }
}
