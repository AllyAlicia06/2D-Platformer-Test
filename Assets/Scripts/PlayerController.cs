using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //movement
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    //ground check
    public LayerMask groundLayer;
    public float rayLength = 0.1f;
    public Vector2 groundCheckOffset = new Vector2(0f, -0.5f);
    
    //animation and all that
    //possibly collision next
    private Rigidbody2D rb;
    private Animator anim;
    private float moveInput;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool isJumping;
    
    //health
    public int maxHealth = 5;
    public int currentHealth;
    public HealthDisplay healthDisplay; //just like in game manager, this is where i will put the health image from the canvas
    public GameObject deathScreen;
    private bool isDead = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; //so the player doesnt weirdly rotate when falling
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthVisual();
    }

    void Update()
    {
        //freezes input if the player dies
        if (isDead)
            return;
        
        // Ground check using raycast
        Vector2 origin = (Vector2)transform.position + groundCheckOffset;
        isGrounded = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);

        // Horizontal movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        rb.linearVelocity = velocity;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
        }

        //so it doesnt always stay true
        isJumping = !isGrounded;
        
        // Animator
        //these lines change the variables in animator to what i have written in the code
        anim.SetBool("IsJumping", isJumping);
        anim.SetFloat("Speed",Mathf.Abs(moveInput));
        anim.SetFloat("MoveX", moveInput);
        
        // Flip sprites on anim
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveInput > 0 ? 1f : -1f;
            transform.localScale = scale;
        }
        
        /*//test damage taken
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
        
        //test health regen
        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(1);
        }*/
        
        //fall death
        if (transform.position.y < -10)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthVisual();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthVisual();
    }
    
    public void Die()
    {
        isDead = true;
        
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
        
        //restart level after waiting for 2 seconds
        StartCoroutine(WaitBeforeRestart());
    }

    IEnumerator WaitBeforeRestart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void UpdateHealthVisual()
    {
        if (healthDisplay != null)
        {
            healthDisplay.SetHealth(currentHealth);
        }
    }
}