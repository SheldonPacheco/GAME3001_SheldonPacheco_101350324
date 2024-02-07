using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 targetPosition = Vector3.zero;
    private GameObject target;
    public Transform planet;
    Rigidbody2D rb;
    public float raySpread = 10.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = new GameObject("target");
        
        //Vector2 a = new Vector2 (2.0f, 3.0f);
        //Vector2 b = new Vector2(10.0f, 12.0f);
        //Vector2 ab = b - a;
        // Vector2 n1 = ab.normalized;
        // Vector2 n2 = ab / MathF.Sqrt(ab.x * ab.x + ab.y *ab.y);
        // float length1 = n1.magnitude;
        // float length2 = n2.magnitude;
        // Debug.Log(n1);
        // Debug.Log(n2);
        // Debug.Log(length1);
        // Debug.Log(length2);

    }
    void Update()
    {
        
        // Check for mouse input.
        if (Input.GetMouseButton(0))
        {
            // Convert mouse position to world position.
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0f; // Ensure the Z-coordinate is correct for a 2D game  .
            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        }
        Seek(targetPosition);
        //Vector3 direction  = (targetPosition - transform.position).normalized;  

        // transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //float distance = Mathf.Min(moveSpeed * Time.deltaTime, (targetPosition - transform.position).magnitude;
        //transform.position += (targetPosition - transform.position).normalized * moveSpeed * Time.deltaTime;

        //homework change this to a generic seek function
        //you should supply two gameobjects a seeker and a target
        // the seeker must contain a rigidbody, amd a maximum speed. all the target needs is a position
        //Vector2 currentVelocity = rb.velocity;
        //Vector2 desiredVelocity = direction * moveSpeed;
        // rb.AddForce(desiredVelocity - currentVelocity);
        // Rotate to look at the target position.
        
    }
    void Seek(Vector3 target)
    {
        

        Vector2 currentVelocity = rb.velocity;
        Vector2 desiredVelocity = (target - transform.position).normalized * moveSpeed;
        

        Vector3 leftDirection = Quaternion.Euler(0.0f, 0.0f, raySpread) * transform.right;
        Vector3 rightDirection = Quaternion.Euler(0.0f, 0.0f, -raySpread) * transform.right;

        RaycastHit2D lefthit = Physics2D.Raycast(transform.position+leftDirection, leftDirection);
        RaycastHit2D righthit = Physics2D.Raycast(transform.position + rightDirection, rightDirection);
        if (lefthit.collider != null&&!lefthit.collider.CompareTag("Player"))
        {
            desiredVelocity += new Vector2(transform.up.x, transform.up.y)* moveSpeed * -1.0f;
           // Debug.Log("hit:"+lefthit.collider.gameObject.name);
        }
        else if (righthit.collider != null && !righthit.collider.CompareTag("Player"))
        {
            desiredVelocity += new Vector2(transform.up.x, transform.up.y) * moveSpeed;
            //Debug.Log("hit:" + righthit.collider.gameObject.name);
        }
        Debug.DrawLine(transform.position, transform.position + leftDirection * raySpread);
        Debug.DrawLine(transform.position, transform.position + rightDirection * raySpread);
        //Debug.DrawLine(transform.position, transform.position+transform.right * 10f);    

        rb.AddForce(desiredVelocity - currentVelocity);

        LookAt2D(target);
    }
    void LookAt2D(Vector3 target)
    {
        Vector3 lookDirection = target - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f,0.0f, angle);
    }
}

