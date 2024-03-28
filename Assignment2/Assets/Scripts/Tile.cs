using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool debugViewActive = false;
    private TileType tileType;
    public float tileCost;
    public TMP_Text costText;
    public TMP_Text infoText;
    public TMP_Text passableText;

    public Sprite[] grassSprites = new Sprite[4];
    public Sprite[] waterSprites = new Sprite[3];
    public Sprite[] obstacleSprites = new Sprite[5];
    public Sprite[] mudSprites = new Sprite[4];


    public TileType GetTileType()
    {
        return tileType;
    }

    public void ToggleDebugView(bool active)
    {
        debugViewActive = active;

        
        costText.gameObject.SetActive(active);
        infoText.gameObject.SetActive(active);
        passableText.gameObject.SetActive(active);
    }
    public float GetTileCost()
    {
        return tileCost;
    }
    void SetTileCost(float tileCost)
    {
        this.tileCost = tileCost;
    }
    
    public void SetTileType(TileType type)
    {
        tileType = type;

        
        if (type == TileType.STONE)
        {

            GetComponent<SpriteRenderer>().sprite = obstacleSprites[Random.Range(0, obstacleSprites.Length)];
            SetTileCost(tileCost = Mathf.Infinity);
            costText.text = "Cost:\n " + GetTileCost().ToString();
            infoText.text = "Type:\n " + GetTileType().ToString();
            passableText.text = "Passable: " + "N";
        }
        else if (type == TileType.GRASS)
        {


            GetComponent<SpriteRenderer>().sprite = grassSprites[Random.Range(0, grassSprites.Length)];
            SetTileCost(tileCost = 10.0f);
            costText.text = "Cost:\n " + GetTileCost().ToString();
            infoText.text = "Type:\n " + GetTileType().ToString();
            passableText.text = "Passable: " + "Y";
        }
        else if (type == TileType.WATER)
        {

            GetComponent<SpriteRenderer>().sprite = waterSprites[Random.Range(0, waterSprites.Length)];
            SetTileCost(tileCost = Mathf.Infinity);
            costText.text = "Cost:\n " + GetTileCost().ToString();
            infoText.text = "Type:\n " + GetTileType().ToString();
            passableText.text = "Passable: " + "N";
        }
        else if (type == TileType.MUD)
        {

            GetComponent<SpriteRenderer>().sprite = mudSprites[Random.Range(0, mudSprites.Length)];
            SetTileCost(tileCost = 50.0f);
            costText.text = "Cost:\n " + GetTileCost().ToString();
            infoText.text = "Type:\n " + GetTileType().ToString();
            passableText.text = "Passable: " + "Y";
        }
    }
    private void Start()
    {
        costText.GetComponent<RectTransform>().localPosition = new Vector3(-0.0033f, 0.1873f, 0f);
        costText.GetComponent<RectTransform>().sizeDelta = new Vector2(0.5603f, 0.1979f);
        infoText.GetComponent<RectTransform>().localPosition = new Vector3(-0.0077f, -0.0398f, 0f);
        infoText.GetComponent<RectTransform>().sizeDelta = new Vector2(0.5557f, 0.2267f);
        passableText.GetComponent<RectTransform>().localPosition = new Vector3(0.0494f, -0.222f, 0f);
        passableText.GetComponent<RectTransform>().sizeDelta = new Vector2(0.6605f, 0.1186f);
    }
}
