﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class MagicController : MonoBehaviour
{
    public string magicName;
    public float damage, speed, lifeTime, range, attackRate;
    private bool isPickable = true;
    public bool onAir;

    private void Start()
    {
        Destroy(gameObject, 1);
    }

    public bool IsPickable
    {
        get { return isPickable; }
        set { isPickable = value; }
    }
    public bool ActivateOnAir(bool isGrounded)
    {
        return (onAir || isGrounded);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    public void SkillDestroy()
    {
        //Debug.Log("Destroy game object");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
