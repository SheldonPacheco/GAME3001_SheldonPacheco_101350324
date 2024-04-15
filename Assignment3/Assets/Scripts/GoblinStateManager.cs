using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GoblinStateManager : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public float moveSpeed = 1f;
    public float rotationSpeed = 2f;
    public float idleTime = 5f;
    public float PatrolTime = 5f;
    public float lineOfSightDistance = 10f;
    public LayerMask playerLayer;

    public enum GoblinState { Idle, Patrol, MoveTowardsPlayer };
    public GoblinState currentState = GoblinState.Idle;
    public int currentPatrolIndex = 0;
    public Transform player;
    public float idleTimer = 0f;
    public TMP_Text GoblinStateText;
    public GameObject lineOfSightVisual;

    public static GoblinStateManager Instance { get; private set; }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Instance = this;
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (currentState == GoblinState.Idle)
        {
            GetComponent<Animator>().SetBool("Walking", false);
            //CanSeePlayer();
            if (CanSeePlayer())
            {
                currentState = GoblinState.MoveTowardsPlayer;
                GoblinStateTextUpdate();
            }
            else
            {
                if (idleTimer <= 0f)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        currentState = GoblinState.Idle;
                        GoblinStateTextUpdate();
                        idleTimer = idleTime;
                    }
                    else
                    {
                        currentState = GoblinState.Patrol;
                        GoblinStateTextUpdate();

                    }
                }
                else
                {
                    idleTimer -= Time.deltaTime;
                    GoblinStateTextUpdate();
                }
            }
        }
        else if (currentState == GoblinState.Patrol)
        {
            Patrol();
        }
        else if (currentState == GoblinState.MoveTowardsPlayer)
        {
            MoveTowardsPlayer();
        }
    }
    private bool CanSeePlayer()
    {
        Vector2 direction;
        bool flipped = GetComponent<SpriteRenderer>().flipX;
        if (flipped)
        {
            direction = -transform.right;
        }
        else
        {
            direction = transform.right;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, lineOfSightDistance, playerLayer);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Goblin"))
            {
                return true;
            }
        }

        return false;
    }



    private void GoblinStateTextUpdate()
    {
        GoblinStateText.text =  "NPC State: " + currentState.ToString() + " |  Idle timer:" + idleTimer;
    }

    private void Patrol()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        if (patrolPoints.Count == 0)
        {
            GameObject PatrolPoint = new GameObject("PatrolPoint");
            PatrolPoint.transform.position = player.position;
            patrolPoints.Add(PatrolPoint.transform);
            return;
        }
        
        Vector3 targetPosition = patrolPoints[currentPatrolIndex].transform.position;
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();
        gameObject.transform.Translate(direction * moveSpeed * Time.deltaTime );
        FlipSprite(direction);

        direction.Normalize();

        lineOfSightVisual.transform.position = transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineOfSightVisual.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            FlipSprite(direction);
            patrolPoints.RemoveAt(currentPatrolIndex);
            if (CanSeePlayer())
            {
                currentState = GoblinState.MoveTowardsPlayer;
            } else
            {
                idleTimer = idleTime;
                currentState = GoblinState.Idle;
            }

        }
    }

    private void MoveTowardsPlayer()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        Vector3 direction = player.position - transform.position;


        lineOfSightVisual.transform.position = transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineOfSightVisual.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        FlipSprite(direction);
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentState == GoblinState.MoveTowardsPlayer)
            {
                SceneManager.LoadScene("GameoverScene");
            }

        }
    }
    private void FlipSprite(Vector3 direction)
    {

        bool movingRight = direction.x > 0f;


        GetComponent<SpriteRenderer>().flipX = !movingRight;
    }
}

