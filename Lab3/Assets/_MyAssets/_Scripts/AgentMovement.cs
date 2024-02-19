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
    public static Steering steering;
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
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0f;
            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        }

        Vector3 steeringForce = Steering.Seek(transform.position, rb.velocity, moveSpeed, targetPosition);
        
        Vector3 leftDirection = Quaternion.Euler(0.0f, 0.0f, raySpread) * transform.right;
        RaycastHit2D lefthit = Physics2D.Raycast(transform.position + leftDirection, leftDirection);
        if (lefthit.collider != null && !lefthit.collider.CompareTag("Player"))
        {
            steeringForce += new Vector3(transform.up.x, transform.up.y) * moveSpeed * -1.0f;
        }
        Vector3 middleLeftDirection = Quaternion.Euler(0.0f, 0.0f, raySpread / 2.0f) * transform.right;
        RaycastHit2D middleLeftHit = Physics2D.Raycast(transform.position + middleLeftDirection, middleLeftDirection);
        if (middleLeftHit.collider != null && !middleLeftHit.collider.CompareTag("Player"))
        {
            steeringForce += new Vector3(transform.up.x, transform.up.y) * moveSpeed * -0.5f;
        }    
        Vector3 middleRightDirection = Quaternion.Euler(0.0f, 0.0f, -raySpread / 2.0f) * transform.right;
        RaycastHit2D middleRightHit = Physics2D.Raycast(transform.position + middleRightDirection, middleRightDirection);
        if (middleRightHit.collider != null && !middleRightHit.collider.CompareTag("Player"))
        {
            steeringForce += new Vector3(transform.up.x, transform.up.y) * moveSpeed * -0.5f;
        }
        Vector3 rightDirection = Quaternion.Euler(0.0f, 0.0f, -raySpread) * transform.right;
        RaycastHit2D righthit = Physics2D.Raycast(transform.position + rightDirection, rightDirection);
        if (righthit.collider != null && !righthit.collider.CompareTag("Player"))
        {
            steeringForce += new Vector3(transform.up.x, transform.up.y) * moveSpeed;
        }

        Debug.DrawLine(transform.position, transform.position + leftDirection * raySpread);
        Debug.DrawLine(transform.position, transform.position + middleLeftDirection * raySpread);
        Debug.DrawLine(transform.position, transform.position + middleRightDirection * raySpread);
        Debug.DrawLine(transform.position,transform.position + rightDirection * raySpread);
        ApplySteering(steeringForce);

        LookAt2D(targetPosition);
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
    void ApplySteering(Vector2 steeringForce)
    {
        Vector2 currentVelocity = rb.velocity;
        rb.AddForce(steeringForce - currentVelocity);
    }


    void LookAt2D(Vector3 target)
    {
        Vector3 lookDirection = target - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
   
}
