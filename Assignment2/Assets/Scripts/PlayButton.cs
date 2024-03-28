using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonPress);


                SceneManager.LoadScene("PlayScene");

            }
        }
        
    }
}