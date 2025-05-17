using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //patrol
    public float moveSpeed = 2f;
    public float patrolRadius = 5f;
    private bool moveRight = true;
    private Rigidbody2D rb;
    private bool isPatrolling = true;
    
    //player in radius
    public float detectionRadius = 1f;
    public float chaseSpeed = 3f;
    private Transform player;
    
    //health and damage
    public int health = 1;
    public int damage = 1;
    
    //ground check
    public LayerMask groundLayer;
    private bool isGrounded;
    //ground check ahead
    public float groundCheckRadius = 0.5f;
    
    //damage delay ig
    public float damageDelay = 1f;
    private float lastDamage = 0f;
    
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIfGrounded();
        
        if (isPatrolling)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
        }
        
        CheckPlayerDetection();
    }

    bool CheckIfGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
    }

    bool CheckForGroundAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(moveRight ? 1 : -1, 0), groundCheckRadius, groundLayer);
        return hit.collider != null;
    }

    void FlipAnimation(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;
    }
    
    void Patrol()
    {
        if (CheckForGroundAhead())
        {
            if (moveRight)
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                anim.SetBool("isMoving", true);
                FlipAnimation(1);
            }
            else
            {
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                anim.SetBool("isMoving", true);
                FlipAnimation(-1);
            }
        }
        else //if no ground ahead
        {
            moveRight = !moveRight;
            anim.SetBool("isMoving", false);
            Invoke("ResumePatrol", 1f);
        }

        //this changes direction when it reaches patrol radius limits
        if (transform.position.x > patrolRadius)
        {
            moveRight = false;
            anim.SetBool("isMoving", false);
            Invoke("ResumePatrol", 1f); //waits 1sec in idle before moving again
        }
        else if (transform.position.x < -patrolRadius)
        { 
            moveRight = true;
            anim.SetBool("isMoving", false);
            Invoke("ResumePatrol", 1f);
        }
    }

    void ResumePatrol()
    {
        isPatrolling = true;
    }

    //check if the player is withing detection radius
    void CheckPlayerDetection()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        //if the player is in radius, start the chase
        if (distanceToPlayer < detectionRadius)
        {
            isPatrolling = false;
        }
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            if (player.position.x > transform.position.x)
            {
                rb.linearVelocity = new Vector2(chaseSpeed, rb.linearVelocity.y);
                FlipAnimation(1);
            }
            else
            {
                rb.linearVelocity = new Vector2(-chaseSpeed, rb.linearVelocity.y);
                FlipAnimation(-1);
            }

            anim.SetBool("isMoving", true);

            // Damage player if close and enough time has passed (damage delay)
            if (Mathf.Abs(transform.position.x - player.position.x) < 0.25f)
            {
                if (Time.time - lastDamage >= damageDelay)
                {
                    player.GetComponent<PlayerController>().TakeDamage(damage);
                    lastDamage = Time.time; // Update the last damage time
                }
            }
        }
        else
        {
            anim.SetBool("isMoving", false);  // Stop chasing if player is out of range
        }
    }

    public void TakeDamage(int damageFromPlayer)
    {
        health -= damageFromPlayer;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("Die");
        Destroy(gameObject, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.relativeVelocity.y > 0) //if the player is falling
            {
                Die();
            }
            else
            {
                if (Time.time - lastDamage >= damageDelay)
                {
                    collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                    lastDamage = Time.time;
                }
            }
        }
    }
}
