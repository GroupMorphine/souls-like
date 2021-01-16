using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatlikeIdleB : StateMachineBehaviour
{
    GameObject thisObject;
    Vector2 direction;
    GameObject target;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        direction = Vector2.left;
        thisObject = animator.gameObject;
    }

     
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (direction.x > 0 && !thisObject.GetComponent<Enemy>().facingright)
        {
            thisObject.gameObject.GetComponent<Enemy>().Flip();
        }
        else if (direction.x <= 0 && thisObject.GetComponent<Enemy>().facingright)
        {
            thisObject.gameObject.GetComponent<Enemy>().Flip();
        }
        
        if (target != null)
        {
            direction = target.transform.position - thisObject.transform.position;
            thisObject.GetComponent<Rigidbody2D>().velocity = direction.normalized * thisObject.GetComponent<Enemy>().speed;
            Collider2D player = Physics2D.OverlapCircle(thisObject.transform.GetChild(1).transform.position, thisObject.GetComponent<Enemy>().attackRange);
            if (player != null && player.CompareTag("Player"))
            {
                animator.SetInteger("States", 1);
            }
        }
        else
        {
            Collider2D[] player = Physics2D.OverlapCircleAll(thisObject.transform.position, thisObject.GetComponent<Enemy>().visualRange);
            foreach (Collider2D item in player)
            {
                if (item.CompareTag("Player"))
                {
                    target = item.gameObject;
                }
            }
            thisObject.GetComponent<Rigidbody2D>().velocity = direction.normalized * thisObject.GetComponent<Enemy>().speed;
            RaycastHit2D hitInfo = Physics2D.Raycast(thisObject.transform.GetChild(0).transform.position, Vector2.down, thisObject.GetComponent<Enemy>().patrolCheckDistance);
            if (hitInfo.collider == null || (hitInfo.collider != null && !hitInfo.collider.CompareTag("Ground")))
            {
                direction *= -1;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thisObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
    }
}
