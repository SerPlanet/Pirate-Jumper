using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private static Vector2 startPos = new Vector2(-1,0.01f);
    [SerializeField] private float minJumpHeight = 5f;      // Minimale Sprungh�he
    [SerializeField] private float maxJumpHeight = 12f;     // Maximale Sprungh�he
    [SerializeField] private float jumpFallOffMultiplier = 5f; // Wie schnell der Sprung abbricht
    [SerializeField] private Animator animator;

    [SerializeField] private SpriteRenderer itemSlot;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpingAudioClip;
    [SerializeField] private AudioClip dieingAudio;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.1f;

    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    private float jumpBufferCounter;

    [Header("JumpSqash")]
    [SerializeField] float squashDuration = 0.10f;
    [SerializeField] Vector2 jumpSquashScale = new Vector2(1.1f, 0.85f);
    [SerializeField] Vector2 landSquashScale = new Vector2(1.15f, 0.8f);

    Vector3 originalScale;


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    //GameMechanicVar
    private bool isJumpPressed = false;
    public bool isGrounded = true;
    private bool checkForFalling = true;


    //VisualsVar
    private bool isJumping = false;
    private bool isWalking = false;
    private bool isDead = false;

    private bool gamePaused = false;

    //Save vars
    private Vector2 savedVelocity;
    private bool isUsingItem,gamerIsRunning;

    //Items

    private float timeTillItemRunsout = 15f;
    private float currentTime;

    private bool itemTimeActive, heiligenscheinActive;





    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody2D>();
        boxCollider = this.GetComponent<BoxCollider2D>();
        animator = this.GetComponentInChildren<Animator>();
        rb.simulated = false;
        originalScale = transform.localScale;
    }

    private void Start()
    {
        InputManager.Instance.OnJumpPressed += JumpPressed;
        InputManager.Instance.OnJumpReleased += JumpReleased;
        GameManager.OnGameStateChange += GameStateChanged;
        ResetPlayer();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnJumpPressed -= JumpPressed;
        InputManager.Instance.OnJumpReleased -= JumpReleased;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !isJumpPressed)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (isGrounded && rb.velocity.y < 0f && checkForFalling)
        {
            Debug.Log(rb.velocity.y);
            if(rb.velocity.y < -0.5f)
            {
                isGrounded = false;
                checkForFalling = false;
                animator.SetBool("isJumping", true);
                animator.SetBool("isGrounded", isGrounded);
            }
            else
            {
                isGrounded = true;
                checkForFalling = true;
                animator.SetBool("isJumping", false);
                animator.SetBool("isGrounded", true);
            }
        }
        if (gamerIsRunning)
        {
            if((transform.position.x < 3 || transform.position.y < -4 )&& !isDead)
        {
                if (!heiligenscheinActive)
                {
                     GameManager.Instance.GameEnds();
                }
                else
                {
                    PlayerDies();
                    MapManager.Instance.UseHeiligenschein();
                }
           
           
            //transform.position = new Vector3 (transform.position.x, transform.position.y-1);
        }
        }
        
    }

    private void Update()
    {
        if (itemTimeActive && !gamePaused)
        {
            currentTime += Time.deltaTime;
            if(currentTime > timeTillItemRunsout)
            {
                DeaktivateItem();
            }
            UIManager.Instance.SetUpItemTime(currentTime);
        }
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }


    private void JumpPressed()
    {
        /*
        if (!isDead && isGrounded && gamerIsRunning)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * maxJumpHeight, ForceMode2D.Impulse);
            isJumpPressed = true;
            isJumping = true;
            isGrounded = false;
            checkForFalling = false;
            AudioManager.Instance.PlayPlayerAudi(jumpingAudioClip,1,1.1f);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isGrounded", isGrounded);  
        }*/
        if (isDead || !gamerIsRunning)
        return;

        jumpBufferCounter = jumpBufferTime;

        if (isGrounded)
        {
            OnJumpSquash();
            PerformJump();
        }
    }
    #region Jump
    private void JumpReleased()
    {
        if (isGrounded)
        {
            
        }
        isJumpPressed = false;
    }

    private void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * maxJumpHeight, ForceMode2D.Impulse);

        // kleine Vorwärtsbewegung
        CameraManager.Instance.JumpCamera();
       // transform.position += new Vector3(0.2f, 0, 0);

        isJumpPressed = true;
        isJumping = true;
        isGrounded = false;
        checkForFalling = false;

        AudioManager.Instance.PlayPlayerAudi(jumpingAudioClip, 1, 1.1f);

        animator.SetBool("isJumping", true);
        animator.SetBool("isGrounded", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<CollectCoin>(out CollectCoin coin)){
            return;
        }
        if(collision.TryGetComponent<CoinMagnet>(out CoinMagnet coinMagnet)){
            return;
        }
        if(collision.TryGetComponent<ChestOpen>(out ChestOpen chest))
        {
            return;
        }
        if (!isUsingItem)
        {

            if (!isGrounded)
            {
                OnLandSquash();
                CameraManager.Instance.LandCamera();
               // CameraManager.Instance.Shake(0.05f, 0.1f);
            }
            isGrounded = true;
            isJumping = false;
            checkForFalling = false;

            animator.SetBool("isJumping", false);
            animator.SetBool("isGrounded", true);
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<CollectCoin>(out CollectCoin coin)){
            return;
        }
        if(collision.TryGetComponent<CoinMagnet>(out CoinMagnet coinMagnet)){
            return;
        }
        if(collision.TryGetComponent<ChestOpen>(out ChestOpen chest))
        {
            return;
        }
        checkForFalling = true;
      
    }

    public void AddJumpForce(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            isJumpPressed = true;
            isJumping = true;
            isGrounded = false;
            checkForFalling = false;
            AudioManager.Instance.PlayPlayerAudi(jumpingAudioClip,1,1.1f);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isGrounded", isGrounded);  
    }

    public void OnJumpSquash()
    {
        StartCoroutine(JumpSquashRoutine());
    }

    IEnumerator JumpSquashRoutine()
    {
        transform.localScale = new Vector3(jumpSquashScale.x, jumpSquashScale.y, 1);

        yield return new WaitForSeconds(squashDuration);

        transform.localScale = originalScale;
    }

    public void OnLandSquash()
    {
        StartCoroutine(LandSquashRoutine());
    }

    IEnumerator LandSquashRoutine()
    {
        transform.localScale = new Vector3(landSquashScale.x, landSquashScale.y, 1);

        yield return new WaitForSeconds(squashDuration);

        transform.localScale = originalScale;
    }
    #endregion
    private IEnumerator MoveToPoint(Vector3 target, float duration)
    {
        gamerIsRunning = false;
        Vector3 start = transform.position;
        float time = 0;
        isGrounded = true;
        animator.SetBool("Walking", true); 
        animator.SetBool("isJumping", false);
        animator.SetBool("isGrounded", isGrounded);  
        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        GameManager.Instance.GameRunning();
        StarGame();
    }
    private void StarGame()
    {
        gamerIsRunning = true;
        rb.simulated = true;
    }

    private void OnDestroy()
    {
         GameManager.OnGameStateChange -= GameStateChanged;
    }

    #region ChangeCharachter

    public void ChangeCharachter(RuntimeAnimatorController controller)
    {
        Debug.Log("CharachterViusualChanged" + controller);
        animator.runtimeAnimatorController = controller;
    }
    #endregion
    #region Items


    private void DeaktivateItem()
    {
        currentTime = 0;
        
        itemTimeActive = false;
        itemSlot.enabled = false;

        //item bools
        heiligenscheinActive = false;
        GameManager.Instance.StopDoubleMoney();
        GameManager.Instance.StopMagnet();
        UIManager.Instance.HideItem();
    }
    public void UseHeiligenschein(Sprite heiligenschein, float duration)
    {
        GameManager.Instance.StopMagnet();
        GameManager.Instance.StopDoubleMoney();
        currentTime = 0;
        timeTillItemRunsout = duration;
        heiligenscheinActive = true;
        itemTimeActive = true;
        itemSlot.enabled = true;
        itemSlot.sprite = heiligenschein;
        //MapManager.Instance.StopUseItem();
    }

    public void UseDoubleMoney(float duration)
    {
        GameManager.Instance.StopMagnet();
        heiligenscheinActive = false;
        timeTillItemRunsout = duration;
        itemSlot.enabled = false;
        GameManager.Instance.UseDoubleMoney();
        itemTimeActive = true;
        currentTime = 0;
    }

    public void UseMagnet(float duration)
    {
        GameManager.Instance.StopDoubleMoney();
        heiligenscheinActive = false;
        timeTillItemRunsout = duration;
        itemSlot.enabled = false;
        GameManager.Instance.StartMagnet();
        itemTimeActive = true;
        currentTime = 0;
    }

    private void RevivePlayer()
    {
        StartCoroutine( MoveToPoint(new Vector3(4,4,0), 2));
        gamerIsRunning = false;
        itemSlot.enabled = false;
        isDead = false;
        animator.SetBool("isDead", isDead);
        DeaktivateItem();
    }
    public void UseKanon(Vector3 pos)
    {
        isUsingItem = true;
        transform.position = pos;
        UnPausePlayer();
        MapManager.Instance.UseKanon();
        StartCoroutine(CannonLaunchRoutine(40, 15));
    }

    private IEnumerator CannonLaunchRoutine(float force, float fakeXSpeed)
    {
        // Reset vertical velocity
        CameraManager.Instance.Shake(0.1f, 0.3f);
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        // Nur nach oben schießen!
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        isJumpPressed = true;
        isJumping = true;
        isGrounded = false;
        checkForFalling = false;

        animator.SetBool("isJumping", true);
        animator.SetBool("isGrounded", false);

        AudioManager.Instance.PlayPlayerAudi(jumpingAudioClip, 1, 1.1f);

        StartCoroutine(WaitSecToCheckGrounded());
        // Rotation während Flug
        while (!isGrounded)
        {
            Vector2 direction = new Vector2(fakeXSpeed, rb.velocity.y);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f; // Verschiebt Bereich nach unten

            angle = Mathf.Clamp(angle, -135f, -45f);

            transform.rotation = Quaternion.Euler(0, 0, angle);

            
            yield return null;
        }

        // Nach Landung Rotation zurücksetzen
        transform.rotation = Quaternion.identity;
        MapManager.Instance.StopUseItem();
        GameManager.Instance.GameRunning();
        
       
    }

    private IEnumerator WaitSecToCheckGrounded()
    {
        yield return new WaitForSeconds(1);
        isUsingItem = false;
    }
    #endregion

    #region GameStates
    private void PausePlayerAtSpot()
    {
        gamePaused = true;
        savedVelocity = rb.velocity;   // aktuelle Bewegung merken
        rb.velocity = Vector2.zero;    // stoppen
        rb.simulated = false;          // Physics stoppen

        animator.speed = 0f;           // Animation stoppen
    }
    private void UnPausePlayer()
    {
        gamePaused = false;
        rb.simulated = true;           // Physics wieder aktivieren
        rb.velocity = savedVelocity;   // alte Bewegung wiederherstellen

        animator.speed = 1f; 
    }

    private void ResetPlayer()
    {
        gamerIsRunning = false;
        itemSlot.enabled = false;
        isDead = false;
        transform.position = startPos;
        animator.SetBool("isDead", isDead);
        GameManager.Instance.PlayerIsReady();
    }
    private void PlayerDies()
    {
        CameraManager.Instance.Shake(0.3f,0.2f);
        isDead = true;
        isJumping=false;
        AudioManager.Instance.PlayPlayerAudi(dieingAudio,1,1);
        animator.SetBool("isDead", isDead);
        animator.SetTrigger("isDeadTrigger");
        animator.SetBool("Walking", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isGrounded",true);
        rb.simulated = false; 
        if (heiligenscheinActive)
        {
            RevivePlayer();
        }
    }
    private void GameStateChanged(GameStates gameState)
    {
        switch (gameState){
            case(GameStates.StartGame):
            StartCoroutine(MoveToPoint(new Vector3(4,0.01f), 1));
            break;
            case(GameStates.GameEnds):
            PlayerDies();
            break;
            case(GameStates.GameRunning):
            UnPausePlayer();
            break;
            case(GameStates.PauseGame):
            PausePlayerAtSpot();
            break;
            case(GameStates.Loading):
            ResetPlayer();
            
            break;
            case(GameStates.Lobby):
            ResetPlayer();
            break;
            case(GameStates.Item):
                PausePlayerAtSpot();
            break;
        }
    }
    #endregion
}
