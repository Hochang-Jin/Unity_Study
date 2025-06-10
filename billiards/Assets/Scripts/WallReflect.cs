using System;
using UnityEngine;

public class WallReflect : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRB = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 ballVelocity = other.gameObject.GetComponent<Ball>().velocity;
            ballRB.linearVelocity = Vector2.Reflect(ballVelocity, -other.GetContact(0).normal);
        }
    }

    Vector2 CalculateReflect(Vector2 a, Vector2 b)
    {
        Vector2 p = -Vector2.Dot(a, b)/ b.magnitude * b/b.magnitude;

        Vector2 reflect = a + 2 * p;
        
        return reflect;

    }
}
