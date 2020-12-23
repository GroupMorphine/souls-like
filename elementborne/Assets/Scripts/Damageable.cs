using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public new string name;
    public float maxHealth;
    [SerializeField]
    protected float currentHealth;

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }else if (currentHealth>= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
