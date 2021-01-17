using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float patrolCheckDistance;
    public new string name;
    public float maxHealth;
    [SerializeField]
    private float currentHealth;
    public Slider healthBar;
    public bool facingright;
    public float speed;
    public float visualRange;
    public float attackRange;
    public float damage;
    public bool canFly;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
    }
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingright = !facingright;
    }

    public void AttackCheck()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(transform.GetChild(1).transform.position, attackRange);
        foreach (Collider2D item in player)
        {
            if (item.CompareTag("Player"))
            {
                item.GetComponent<Character>().TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        GetComponent<Animator>().SetInteger("States", 2);
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    public void SetStateIdle()
    {
        GetComponent<Animator>().SetInteger("States", 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.GetChild(1).position, attackRange);
    }

}
