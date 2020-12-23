using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public string magicName;
    public float damage, speed, lifeTime, range, attackRate;
    private bool isPickable = true;
    public bool onAir;
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
