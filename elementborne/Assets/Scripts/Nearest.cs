using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nearest : MonoBehaviour
{
    public float radius;
    public LayerMask Obstacle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Enemy: " + NearestEnemy());
        Debug.Log("Obstacle: " + NearestObstacle());
    }

    public Transform NearestEnemy()
    {
        Vector2 distance = Vector2.positiveInfinity;
        Transform nearestEnemy = null;
        Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D item in enemy)
        {
            if (item.CompareTag("Enemy"))
            {
                Vector2 temp = item.transform.position - transform.position;
                if (temp.magnitude < distance.magnitude)
                {
                    distance = temp;
                    nearestEnemy = item.transform;
                }
            }
        }
        return nearestEnemy;
    }
    public Transform NearestObstacle()
    {
        
        Vector2 distance = Vector2.positiveInfinity;
        Transform nearestObstacle = null;
        Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radius,Obstacle);
        foreach (Collider2D item in enemy)
        {
            if (item.CompareTag("Ground"))
            {
                Vector2 temp = item.transform.position - transform.position;
                if (temp.magnitude < distance.magnitude)
                {
                    distance = temp;
                    nearestObstacle = item.transform;
                }
            }
        }
        return nearestObstacle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
