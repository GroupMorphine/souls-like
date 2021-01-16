using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    //{Fire : #FF4400, Water : #00E7FF, Stone : #2B1801, Electric : #C600FD, Wind : #FFFFFF};
    public string magicName;
    public float damage, speed, lifeTime, range, attackRate;
    private bool isPickable = true;
    public bool onAir;
    public Color elementcolor = new Color();

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().material.SetColor("Color_RR", elementcolor);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
