using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private float jumpHight = 10f;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    //GameMechanicVar
    private bool jumpRequest;
    private bool isGrounded = true;


    //VisualsVar
    private bool isJumping = false;
    private bool isWalking = false;
    private bool isDead = false;





    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody2D>();
        boxCollider = this.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {

        InputManager.Instance.OnJump += Jump;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnJump -= Jump;
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpHight, ForceMode2D.Impulse);
            jumpRequest = false;
        }
    }

    private void Jump()
    {
        if(isGrounded && !isDead)
        {
            jumpRequest = true;
            Debug.Log("Jump");
        }
       
    }
}
