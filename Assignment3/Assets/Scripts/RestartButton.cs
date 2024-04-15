using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.StopMusic(SoundManager.Instance.deathMusic);
            SoundManager.Instance.PlayMusic(SoundManager.Instance.gameMusic);
            MouseClick();
        }
    }

    void MouseClick()
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