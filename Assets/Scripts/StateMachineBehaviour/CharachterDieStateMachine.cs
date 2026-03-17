using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterDieStateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MovementScript charachter = animator.GetComponentInParent<MovementScript>();
        
        if (charachter != null)
        {
            charachter.OnCharachterDied();
        }
        Debug.Log("AnimationWorked");
    }
}
