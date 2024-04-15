using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        float moveSpeed = 2f;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.position += movement;
        if (movement != Vector3.zero)
        {
            GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Walking", false);
        }

        if (horizontalInput > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (horizontalInput < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (verticalInput > 0)
        {
            
        }
        else if (verticalInput < 0)
        {
            
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goblin"))
        {
            if (GoblinStateManager.Instance.currentState == GoblinStateManager.GoblinState.Idle || GoblinStateManager.Instance.currentState == GoblinStateManager.GoblinState.Patrol)
            {
                SceneManager.LoadScene("VictoryScene");
            }

        }
    }
}
