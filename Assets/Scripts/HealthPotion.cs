using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healthToRestore = 1;

    private int playerCurrentHealth;
    private int playerMaxHealth;

    void OnTriggerEnter2D(Collider2D other)
    {
        playerCurrentHealth = other.GetComponent<PlayerController>().currentHealth;
        playerMaxHealth = other.GetComponent<PlayerController>().maxHealth;
        
        if (other.tag == "Player" && playerCurrentHealth < playerMaxHealth)
        {
            other.GetComponent<PlayerController>().Heal(healthToRestore);
            Destroy(gameObject);
        }
    }
}
