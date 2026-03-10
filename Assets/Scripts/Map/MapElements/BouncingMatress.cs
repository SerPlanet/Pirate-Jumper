using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingMatress : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.TryGetComponent<MovementScript>(out MovementScript player))
        {
            animator.SetTrigger("BounceStart");
            player.AddJumpForce(jumpForce);
        }
    }
}
