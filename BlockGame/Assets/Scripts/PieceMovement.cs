using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    float timeSinceMove = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceMove += Time.deltaTime;
        if (timeSinceMove >= 1)
        {
            transform.Translate(Vector3.down);
            timeSinceMove = 0.0f;
        }

    }
}
