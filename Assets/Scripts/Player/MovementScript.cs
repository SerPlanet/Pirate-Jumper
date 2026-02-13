using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private float minJumpHeight = 5f;      // Minimale Sprunghöhe
    [SerializeField] private float maxJumpHeight = 12f;     // Maximale Sprunghöhe
    [SerializeField] private float jumpFallOffMultiplier = 5f; // Wie schnell der Sprung abbricht

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    //GameMechanicVar
    private bool isJumpPressed = false;
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
        InputManager.Instance.OnJumpPressed += JumpPressed;
        InputManager.Instance.OnJumpReleased += JumpReleased;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnJumpPressed -= JumpPressed;
        InputManager.Instance.OnJumpReleased -= JumpReleased;
    }

    private void FixedUpdate()
    {
        if (!isJumpPressed && rb.velocity.y > minJumpHeight)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * jumpFallOffMultiplier * Time.deltaTime;
        }
    }

    private void JumpPressed()
    {
        if (!isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * maxJumpHeight, ForceMode2D.Impulse);
            isJumpPressed = true;
            isJumping = true;
            isGrounded = false;
            Debug.Log("Jump Started");
        }
    }

    private void JumpReleased()
    {
        isJumpPressed = false;
        Debug.Log("Jump Released");
    }
}
