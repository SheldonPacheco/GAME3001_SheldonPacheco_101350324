using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer; 
    private List<Vector2Int> path = new List<Vector2Int>();
    private int currentWaypointIndex = 0;
    private TileType[,] tileTypes; 
    float timer = 0;    
    public void SetPath(List<Vector2Int> path, TileType[,] types) 
    {
        this.path = path;
        currentWaypointIndex = 0;
        this.tileTypes = types;
        spriteRenderer.flipX = true;
        gameObject.GetComponent<Animator>().SetBool("Walking", true);
        timer = 4.65f;
        SoundManager.Instance.StopSFX(SoundManager.Instance.goalReached);
        SoundManager.Instance.soundFXSource.volume = 0.3f;
    }

    void Update()
    {
        if (path.Count == 0 || currentWaypointIndex >= path.Count)
        {

            return;
        }

        Vector3 targetPosition = new Vector3(path[currentWaypointIndex].x, -path[currentWaypointIndex].y, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spriteRenderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (currentWaypointIndex == path.Count - 1)
            {
                timer -= Time.deltaTime;
                SoundManager.Instance.soundFXSource.volume = 0.010f;
                SoundManager.Instance.PlaySFX(SoundManager.Instance.goalReached);
                gameObject.GetComponent<Animator>().SetBool("Walking", false);
                if (timer <= 0)
                {
                    SoundManager.Instance.StopSFX(SoundManager.Instance.goalReached);
                }             
            }
            else
            {
                currentWaypointIndex++;
                SoundManager.Instance.PlaySFX(SoundManager.Instance.walkingSound);
            }
        }
    }

    private void Start()
    {
        transform.position = Map.GetStart();
        
    }
}
