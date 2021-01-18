using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nearest : MonoBehaviour
{
    public float radius;
    public LayerMask Obstacle;

    public Transform NearestEnemy()
    {
        Vector2 distance = Vector2.positiveInfinity;
        Transform nearestEnemy = null;
        RaycastHit2D[] enemy = Physics2D.RaycastAll(transform.position, Vector2.left, float.MaxValue);
        //Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (RaycastHit2D item in enemy)
        {
            if (item.collider.CompareTag("Enemy"))
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
        RaycastHit2D[] enemy = Physics2D.RaycastAll(transform.position, Vector2.left, float.MaxValue, Obstacle);
        //Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radius, Obstacle);
        foreach (RaycastHit2D item in enemy)
        {
            if (item.collider.CompareTag("Ground"))
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
    }
}
