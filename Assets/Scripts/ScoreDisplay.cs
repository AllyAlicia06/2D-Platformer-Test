using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Sprite[] scoreSprites;
    //those are the states of the sprite basically
    //0-empty, 1-one collectible, etc...
    //THIS is where i put the sprites of the score and this shit is on the score image in my canvas
    
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    //this basically changes the image of the score based of what i give to the variable
    public void SetScore(int score)
    {
        score = Mathf.Clamp(score, 0, scoreSprites.Length - 1);
        image.sprite = scoreSprites[score];
    }
}
