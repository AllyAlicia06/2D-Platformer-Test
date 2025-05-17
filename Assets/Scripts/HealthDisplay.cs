using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite[] healthSprite;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetHealth(int health)
    {
        health = Mathf.Clamp(health, 0, healthSprite.Length - 1);
        image.sprite = healthSprite[health];
    }

}
