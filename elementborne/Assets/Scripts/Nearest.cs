using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nearest : MonoBehaviour
{
    public float radius;
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> Obstacles = new List<GameObject>();

    public Transform NearestEnemy()
    {
        foreach (var item in Enemies)
        {
            if (item == null)
            {
                Enemies.Remove(item);
            }
        }

        float diff = transform.position.x - Enemies[0].transform.position.x;
        Transform nearest = Enemies[0].transform;
        for (int i = 1; i < Enemies.Count; i++)
        {
            float new_diff = transform.position.x - Enemies[i].transform.position.x;

            if (new_diff < diff && new_diff > 0)
            {
                nearest = Enemies[i].transform;
                diff = new_diff;
            }
        }

        return nearest;
    }
    public Transform NearestObstacle()
    {
        foreach (var item in Obstacles)
        {
            if (item == null)
            {
                Obstacles.Remove(item);
            }
        }

        float diff = transform.position.x - Obstacles[0].transform.position.x;
        Transform nearest = Obstacles[0].transform;
        for (int i = 1; i < Obstacles.Count; i++)
        {
            float new_diff = transform.position.x - Obstacles[i].transform.position.x;

            if (new_diff < diff && new_diff > 0)
            {
                nearest = Obstacles[i].transform;
                diff = new_diff;
            }
        }

        return nearest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
