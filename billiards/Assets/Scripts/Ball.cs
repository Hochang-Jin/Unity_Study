using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    protected Rigidbody2D rb;
    public Vector2 velocity;
    public Vector2 startVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = startVelocity;
    }

    private void Update()
    {
        
        velocity = rb.linearVelocity;
    }
}
