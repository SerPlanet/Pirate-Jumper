using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStateMachineBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemChest chest = animator.GetComponentInParent<ItemChest>();
        if (chest != null)
        {
            chest.ActivateItem();
        }
    }
}
