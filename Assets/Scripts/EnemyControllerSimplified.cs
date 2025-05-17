using System;
using System.Collections;
using UnityEngine;

public class EnemyControllerSimplified : MonoBehaviour
{
    public float speed = 1.5f;
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb2d;
    private Animator animator;
    private Transform currentPoint;

    public int damage = 1;
    public float stompThreshold = 0.5f;
    public float damageCooldown = 1f;
    private float lastDamage = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        currentPoint=pointA.transform;
        animator.SetBool("isMoving",true);
        rb2d.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointA.transform)
        {
            rb2d.linearVelocity = new Vector2(speed, 0f);
        }
        else
        {
            rb2d.linearVelocity = new Vector2(-speed, 0f);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint=pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint=pointB.transform;
        }
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        
        Rigidbody2D playerRb2d = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb2d == null) return;

        float relativeVelocity = collision.relativeVelocity.y;

        if (relativeVelocity > stompThreshold)
        {
            Die();
        }
        else if (Time.time - lastDamage >= damageCooldown)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            lastDamage = Time.time;
        }
    }*/
    
    //THIS FINALLY WORKS HOLY FUCK
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Get the contact point of the collision
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 contactNormal = contact.normal;

        // Check if the player hit the enemy from above
        bool stomped = contactNormal.y < -0.5f;

        if (stomped)
        {
            Die();

            // Optional: Make player bounce
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 5f); // bounce up
            }
        }
        else if (Time.time - lastDamage >= damageCooldown)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            lastDamage = Time.time;
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        rb2d.linearVelocity = Vector2.zero;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        rb2d.simulated = false;
        Destroy(gameObject, 1f);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position,0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position,0.5f);
        Gizmos.DrawLine(pointA.transform.position,pointB.transform.position);
    }
}
