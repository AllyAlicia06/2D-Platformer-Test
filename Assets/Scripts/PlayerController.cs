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
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Automatically find UI references
        if (healthDisplay == null)
            healthDisplay = FindObjectOfType<HealthDisplay>();

        if (deathScreen == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Transform found = canvas.transform.Find("DeathScreen");
                if (found != null)
                {
                    deathScreen = found.gameObject;
                }
                else
                {
                    Debug.LogWarning("DeathScreen not found under Canvas!");
                }
            }
            else
            {
                Debug.LogWarning("Canvas not found!");
            }
        }

        UpdateHealthVisual();
    }

    void Awake()
    {
        if (healthDisplay == null)
            healthDisplay = FindObjectOfType<HealthDisplay>();

        if (deathScreen == null)
            deathScreen = GameObject.Find("DeathScreen"); // or your exact path/name

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
        
        //fall death
        if (transform.position.y < -10)
        {
            Die();
        }
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log($"Player taking damage: {damage}");
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
        Debug.Log("Player died");
        isDead = true;

        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            Debug.Log("Death screen activated");
        }
        else
        {
            Debug.LogWarning("Death screen reference is null!");
        }
    
        StartCoroutine(WaitBeforeRestart());
    }

    IEnumerator WaitBeforeRestart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void UpdateHealthVisual()
    {
        Debug.Log($"Updating health UI: currentHealth = {currentHealth}");
        if (healthDisplay != null)
        {
            healthDisplay.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("HealthDisplay reference is null!");
        }
    }
}