using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject agentPrefab;
    public GameObject obstaclePrefab;
    public GameObject sensorPrefab;

    private GameObject target;
    private GameObject agent;
    private GameObject obstacle;
    private GameObject sensor;

    public AudioSource musicSource;
    public AudioClip gameMusic;
    public AudioClip soundEffect;
    bool startArrival = false;
    bool ObstacleAvoidance = false;




    private void Start()
    {
        // PlayMusic();
        // SetupStartScene();            
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
        agent = Instantiate(agentPrefab, new Vector3(-8.0f, -2.9f, 0f), Quaternion.identity);



        Vector3 direction = (target.transform.position - agent.transform.position).normalized;
        agent.GetComponent<Rigidbody2D>().velocity = direction * 3f;          
    }

    private void Flee()
    {
        ClearScene();
        target = Instantiate(targetPrefab, new Vector3(8.2f, -2.9f, 0f), Quaternion.identity);
        agent = Instantiate(agentPrefab, new Vector3(7.5f, -2.9f, 0f), Quaternion.identity);

        Vector3 direction = (agent.transform.position - target.transform.position).normalized;
        agent.GetComponent<Rigidbody2D>().velocity = direction * 3f;
    }

    private void Arrival()
    {
        
        ClearScene();
        target = Instantiate(targetPrefab, new Vector3(8.2f, -2.9f, 0f), Quaternion.identity);
        agent = Instantiate(agentPrefab, new Vector3(-8.0f, -2.9f, 0f), Quaternion.identity);
        startArrival = true;

    }
    private void StartArrival()
    {

        float distance = CalculateDistance();
        Vector3 direction = CalculateDirection();
        if (distance > 15f)
        {
            GetAgentRigidbody().velocity = direction * 6f;
            Debug.Log(distance);
        }
        else if (distance > 10)
        {
            
            GetAgentRigidbody().velocity = direction * 4f;
            Debug.Log(distance);
        }
        else if (distance > 1.4f)
        {

            
            GetAgentRigidbody().velocity = direction * 2f;
            Debug.Log(distance);
        }
        else if (distance > 1.3f)
        {
            
            GetAgentRigidbody().velocity = Vector2.zero;
            Debug.Log(distance);
            startArrival = false;
        }


    }


    private void ObstacleAvoidanceBehavior()
    {
        ClearScene();
        target = Instantiate(targetPrefab, new Vector3(8.2f, -2.9f, 0f), Quaternion.identity);
        obstacle = Instantiate(obstaclePrefab, new Vector3(0f, -3.4f, 2f), Quaternion.identity);
        agent = Instantiate(agentPrefab, new Vector3(-8.0f, -2.9f, 0f), Quaternion.identity);
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
        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            Debug.Log(hit.collider.gameObject.name + hit.collider.tag);
            Debug.Log(distanceToObstacle);
            if (distanceToObstacle < 3.5f)
            {

                GetAgentRigidbody().velocity = Vector2.up * 3f;

            }

        }
        if (distanceFromTarget < 6.0f)
        {
            GetAgentRigidbody().velocity = Vector2.down * 3f;

            GetAgentRigidbody().velocity = direction * 1f;
        } 
        if (distanceFromTarget < 2.0f)
        {
            GetAgentRigidbody().velocity = Vector2.zero;
            ObstacleAvoidance = false;
        }
    }
    private void PlayMusic()
    {
        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.volume = 0.5f; // Adjust the volume as needed
        musicSource.Play();
        DontDestroyOnLoad(gameObject);
    }

    private void ClearScene()
    {
        Destroy(target);
        Destroy(agent);
        Destroy(obstacle);
        Destroy(sensor);
    }
    private Vector3 CalculateDirection()
    {
        return (target.transform.position - agent.transform.position).normalized;
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
}
