using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healthToRestore = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        int playerCurrentHealth = other.GetComponent<PlayerController>().currentHealth;
        int playerMaxHealth = other.GetComponent<PlayerController>().maxHealth;
        
        if (other.tag == "Player" && playerCurrentHealth < playerMaxHealth)
        {
            other.GetComponent<PlayerController>().Heal(healthToRestore);
            Destroy(gameObject);
        }
    }
}
