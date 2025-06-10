using System;
using Mono.Cecil.Cil;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private float zRotation = 0f;
    public float rotateSpeed = 50f;
    public GameObject whiteball;
    public Transform launchPos;
    public float ballSpeed = 5f;
    
    private void Update()
    {
        zRotation += Input.GetAxisRaw("Vertical") * Time.deltaTime  * rotateSpeed;
        
        if(zRotation > 90f)
            zRotation = 90f;
        if(zRotation < 0)
            zRotation = 0; 
        transform.eulerAngles = new Vector3(0, 0, zRotation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ball = Instantiate(whiteball, launchPos.position, Quaternion.Euler(Vector3.zero));
            ball.GetComponent<Ball>().startVelocity = new Vector2(ballSpeed * Mathf.Cos(zRotation*Mathf.Deg2Rad), ballSpeed * Mathf.Sin(zRotation*Mathf.Deg2Rad));
        }
    }
}
