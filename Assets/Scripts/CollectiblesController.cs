using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    public int collectibleValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.AddScore(collectibleValue);
            Destroy(gameObject);
        }
    }
}
