using UnityEngine;

public class RangedEnemyBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] waypoints;
    public Transform[] coverPoints;
    public float coverDistanceThreshold = 10f;

    private GameObject player;
    private int nextWaypoint = 0;
    private int nextCoverPoint = 0;
    public static bool isHiding = false;

    // Decision tree nodes
    private DistanceNode farDistance = new DistanceNode();
    private DistanceNode nearDistance = new DistanceNode();
    private PatrolAction patrolAction = new PatrolAction();
    private VisibleNode visible = new VisibleNode();
    private MoveToVisibleAction moveToVisibleAction = new MoveToVisibleAction();
    private MoveToTargetAction moveToTargetAction = new MoveToTargetAction();
    private FarAttackAction farAttackAction = new FarAttackAction();
    private HideBehindCoverAction hideBehindCoverAction = new HideBehindCoverAction();
    float timerHide = 3.0f;
    float timerAttack = 3.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        farDistance.agent = nearDistance.agent = gameObject;
        farDistance.target = nearDistance.target = player;
        farDistance.distance = 15.0f; // Increased distance for ranged enemy
        nearDistance.distance = 3.5f;

        visible.agent = gameObject;
        visible.target = player;

        patrolAction.agent = gameObject;
        patrolAction.waypoints = waypoints;
        patrolAction.speed = 10f;

        moveToVisibleAction.agent = gameObject;
        moveToVisibleAction.target = player;
        moveToVisibleAction.waypoints = waypoints;
        moveToVisibleAction.speed = 10f;

        moveToTargetAction.agent = gameObject;
        moveToTargetAction.target = player;
        moveToTargetAction.speed = 10f;

        farAttackAction.agent = gameObject;
        farAttackAction.target = player;
        farAttackAction.projectilePrefab = projectilePrefab;

        hideBehindCoverAction.agent = gameObject;
        hideBehindCoverAction.coverPoints = coverPoints;
        hideBehindCoverAction.speed = 5f;

        // Constructing the decision tree
        farDistance.no = patrolAction;
        farDistance.yes = visible;
        visible.no = moveToVisibleAction;
        visible.yes = nearDistance;
        nearDistance.no = moveToTargetAction;
        nearDistance.yes = isHiding ? hideBehindCoverAction : farAttackAction;
        
    }

    void Update()
    {
        timerHide -= Time.deltaTime;
        if (timerHide > 0)
        {
            nearDistance.yes = farAttackAction;
            isHiding = false;
            timerAttack = 3.0f;
        }
           
        if (timerHide <= 0)
        {
            timerAttack -= Time.deltaTime;
            nearDistance.yes = hideBehindCoverAction;
            isHiding = true;
            
            if (timerAttack <= 0)
            {
                timerHide = 3.0f;

            }
            
        }
        
        TreeNode.Traverse(farDistance);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Waypoint"))
        {
            nextWaypoint++;
            nextWaypoint %= waypoints.Length;
            patrolAction.nextWaypoint = nextWaypoint;
        }
        else if (collision.CompareTag("Cover"))
        {
            isHiding = true;
            Debug.Log("hit");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cover"))
        {
            isHiding = false;
        }
    }
}

