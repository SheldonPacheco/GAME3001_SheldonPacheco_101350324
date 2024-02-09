using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Perform a raycast to check if the mouse click hits the collider
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // If the collider is hit, load the game
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {              
                SceneManager.LoadScene("PlayScene");
            }
        }
    }

}
