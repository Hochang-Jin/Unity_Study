using System;
using UnityEngine;

public class WallReflect2 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRB = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 ballVelocity = other.gameObject.GetComponent<Ball>().velocity;
            Vector2 reflect = Vector2.Reflect(ballVelocity, -other.GetContact(0).normal).normalized;
            float power = ballVelocity.magnitude;
            ballRB.linearVelocity = Vector2.zero;
            ballRB.AddForce(reflect * power *0.8f,ForceMode2D.Impulse);
        }
    }
}