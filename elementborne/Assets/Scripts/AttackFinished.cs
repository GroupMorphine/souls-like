using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFinished : StateMachineBehaviour
{
    GameObject player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x*0.8f, player.GetComponent<Rigidbody2D>().velocity.y);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.GetComponent<Animator>().ResetTrigger("attack");
    }
}
