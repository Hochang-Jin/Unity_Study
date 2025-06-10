using System;
using UnityEngine;

public class Ball2 : Ball
{
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = startVelocity;
    }

    private void Update()
    {
        rb.linearVelocity -= rb.linearVelocity * 0.3f * Time.deltaTime;
        if (rb.linearVelocity.sqrMagnitude < 0.2f)
        {
            rb.linearVelocity = Vector2.zero;
        }
        velocity = rb.linearVelocity;
    }
}
