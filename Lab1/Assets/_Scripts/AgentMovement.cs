using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] Vector3 m_position;
    [SerializeField] float m_speed=2.5f;

    public static AgentMovement instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            m_position = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            m_position.z = 0;
            LookAt2D(m_position);
        }
        //Vector3 direction = Vector3.right;
        //Debug.Log(direction);
        //transform.position = direction* m_speed*Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, m_position, m_speed * Time.deltaTime);

        LookAt2D(m_position);
    }
    void LookAt2D(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;  
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
    }
    void OnTriggerEnter2D(Collider2D agent)   
    {
        if (agent.CompareTag("Island"))
        {
            SceneLoader.LoadSceneByIndex(2);
        }
    }
}
