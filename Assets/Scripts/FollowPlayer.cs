using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform followTarget;
    public float minX = 0f;
    public float maxX = 10f;

    // Update is called once per frame
    void Update()
    {
        float clampedX = Mathf.Clamp(followTarget.position.x, minX, maxX);
        Vector3 pos = new Vector3(clampedX, transform.position.y, -10);
        transform.position = pos;
    }
}
