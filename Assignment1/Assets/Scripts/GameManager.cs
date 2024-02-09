using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject agentPrefab;
    public GameObject obstaclePrefab;
    
    private GameObject target;
    private GameObject agent;
    private GameObject obstacle;

    
    bool startArrival = false;
    bool ObstacleAvoidance = false;
    bool startFlee = false;
    bool startSeek = false;
    



    private void Start()
    {
        
        
    }
    private void Update()
    {
        Menu();
        if (startArrival==true) {
            StartArrival();
        }
        if (ObstacleAvoidance == true)
        {
            StartObstacleAvoidance();
        }
        if (startFlee==true)
        {
            StartFlee();
        }
        if (startSeek == true)
        {
            StartSeek();
        }
    }

    private void SetupStartScene()
    {
        // Add text, button, and other elements for the Start Scene
        // ...
    }

    private void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Seek();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Flee();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Arrival();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ObstacleAvoidanceBehavior();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ClearScene();
        }
    }

    private void Seek()
    {
        ClearScene();
        target = Instantiate(targetPrefab, new Vector3(8.2f, -2.9f, 0f), Quaternion.identity);
        agent = Instantiate(agentPrefab, new Vector3(-8.0f, -3.36f, 0f), Quaternion.identity);   
        startSeek = true;
    }
    private void StartSeek()
    {
        CheckCollision();
        Vector3 direction = CalculateDirection();
        float distance = CalculateDistance();
        GetAgentRigidbody().velocity = direction * 3f;
        GetAnimator().SetBool("Walking", true);
      
    }
        private void Flee()
    {
        ClearScene();
        target = Instantiate(targetPrefab, new Vector3(8.2f, -2.9f, 0f), Quaternion.identity);
        agent = Instantiate(agentPrefab, new Vector3(7.5f, -3.36f, 0f), Quaternion.identity);
        GetSpriteRenderer().flipX = true;
        startFlee = true;
    }
    private void StartFlee()
    {
        Vector3 direction = CalculateDirectionFlee();
        float distance = CalculateDistance();
        GetAgentRigidbody().velocity = direction * 3f;
        GetAnimator().SetBool("Walking",true);  
        if (distance > 16.0f)
        {
            startFlee = false;
            GetAnimator().SetBool("Walking", false);
        }
    }

        private void Arrival()
    {
        
        ClearScene();
        target = Instantiate(targetPrefab, new Vector3(8.2f, -2.9f, 0f), Quaternion.identity);
        agent = Instantiate(agentPrefab, new Vector3(-8.0f, -3.36f, 0f), Quaternion.identity);
        startArrival = true;

    }
    private void StartArrival()
    {

        float distance = CalculateDistance();
        Vector3 direction = CalculateDirection();
        GetAnimator().SetBool("Walking", true);
        if (distance > 15f)
        {
            GetAgentRigidbody().velocity = direction * 6f;
        }
        else if (distance > 10)
        {         
            GetAgentRigidbody().velocity = direction * 4f;
        }
        else if (distance > 1.0f)
        {
            GetAgentRigidbody().velocity = direction * 2f;            
            CheckCollision();
        }


    }


    private void ObstacleAvoidanceBehavior()
    {
        ClearScene();
        target = Instantiate(targetPrefab, new Vector3(8.2f, -2.9f, 0f), Quaternion.identity);
        obstacle = Instantiate(obstaclePrefab, new Vector3(0f, -3.4f, 2f), Quaternion.identity);
        agent = Instantiate(agentPrefab, new Vector3(-8.0f, -3.36f, 0f), Quaternion.identity);
        ObstacleAvoidance = true;
    }
    private void StartObstacleAvoidance()
    {       
        float distanceToObstacle = ObstacleDistance();
        Vector3 rayDirection = Vector3.right;
        Vector3 direction = CalculateDirection();
        float distanceFromTarget = CalculateDistance();

        RaycastHit2D hit = Physics2D.Raycast(agent.transform.position+rayDirection,rayDirection);

        GetAgentRigidbody().velocity = direction * 2f;
        GetAnimator().SetBool("Walking", true);
        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            Debug.Log(hit.collider.gameObject.name + hit.collider.tag);
            Debug.Log(distanceToObstacle);
            if (distanceToObstacle < 3.5f)
            {

                GetAgentRigidbody().velocity =  Vector2.up * 3f;
                
            }

        }
        if (distanceToObstacle > 4.2f && GetAgentRigidbody().position.y > -1.1f)
        {

            GetAgentRigidbody().velocity = Vector2.zero;
            GetAgentRigidbody().velocity = Vector2.down *5f;

        }
        if (distanceFromTarget < 6.0f)
        {
            GetAgentRigidbody().velocity = direction * 1f;
        } 
        if (distanceFromTarget < 1.2f)
        {
            CheckCollision();
        }
    }

    private void ClearScene()
    {
        Destroy(target);
        Destroy(agent);
        Destroy(obstacle);
        startArrival = false;
        ObstacleAvoidance = false;
        startFlee = false;
        startSeek = false;
    }
    private Vector3 CalculateDirection()
    {
        return (target.transform.position - agent.transform.position).normalized;
    }
    private Vector3 CalculateDirectionFlee()
    {
        return (agent.transform.position- target.transform.position).normalized;
    }
    private float CalculateDistance()
    {
        return Vector3.Distance(agent.transform.position, target.transform.position);
    }
    private float ObstacleDistance()
    {
        return Vector3.Distance(agent.transform.position, obstacle.transform.position);
    }
    private Rigidbody2D GetAgentRigidbody()
    {
        return agent.GetComponent<Rigidbody2D>();
    }
    private SpriteRenderer GetSpriteRenderer()
    {
        return agent.GetComponent<SpriteRenderer>();
    }
    private Animator GetAnimator()
    {
        return agent.GetComponent<Animator>();
    }
    private void CheckCollision()
    {
        
        float distanceFromTarget = CalculateDistance();
        RaycastHit2D hit = Physics2D.Raycast(agent.transform.position, Vector2.right);
        if (hit.collider != null && !hit.collider.CompareTag("Player")&&!hit.collider.CompareTag("Obstacle"))
        {
            if (distanceFromTarget < 1.2f)
            {
                GetAnimator().Play("MeleeAttack");
                MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
                musicPlayer.PlayDeathSound();
                Destroy(target);
                GetAnimator().SetBool("Walking", false);
                startArrival = false;
                ObstacleAvoidance = false;
                startFlee = false;
                startSeek = false;
            }
        }
            

    }
}
