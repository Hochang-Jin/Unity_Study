using UnityEngine;

public class BallAddForce : MonoBehaviour
{
    Rigidbody2D rb;
    public float ball1Force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right * ball1Force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
