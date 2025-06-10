using UnityEngine;

public class Que : MonoBehaviour
{
    public GameObject que;
    public GameObject line;
    public GameObject ball;
    public GameObject[] balls;
    private Rigidbody2D ballRB;
    private Rigidbody2D[] ballRBs;
    private float zRotation = 0f;
    public float rotateSpeed = 50f;
    public float ballSpeed = 5f;

    private float angle;
    private Vector2 target, mouse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody2D>();
        ballRBs = new Rigidbody2D[balls.Length];
        for (int i = 0; i < balls.Length; i++)
        {
            ballRBs[i] = balls[i].gameObject.GetComponent<Rigidbody2D>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //zRotation += Input.GetAxisRaw("Vertical") * Time.deltaTime  * rotateSpeed;
        //ball.transform.eulerAngles = new Vector3(0, 0, zRotation);
        target = ball.transform.position;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        ball.transform.rotation = Quaternion.AngleAxis(angle - 180f, Vector3.forward);
        
        bool isStopped = true;
        
        for (int i = 0; i < balls.Length; i++)
        { 
            if(ballRBs[i].linearVelocity != Vector2.zero)
                isStopped = false;
        }
        
        Vector2 ballPos = ball.transform.position;
        Vector2 quePos = que.transform.position;
        Vector2 vel = ballPos - quePos;
        
        //if (ball.GetComponent<Rigidbody2D>().linearVelocity == Vector2.zero)
        if(isStopped)
        {
            que.SetActive(true);
            line.SetActive(true);
            
            if (Input.GetMouseButtonDown(0))
            {
                ballRB.AddForce(vel * ballSpeed, ForceMode2D.Impulse);
            }
        }
        else
        {
            que.SetActive(false);
            line.SetActive(false);
        }
        
    }
    
    
}
