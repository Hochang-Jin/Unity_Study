using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private Vector3 oldPosition;
    private bool isTurn = false;
    
    private int moveCnt = 0;
    private int turnCnt = 0;
    private int spawnCnt = 0;
    
    private bool isDie = false;

    private AudioSource sound;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        
        startPosition = transform.position;
        Init();
    }
    
    private void Init()
    {
        animator.SetBool("Die", false);
        transform.position = startPosition;
        oldPosition = startPosition;
        moveCnt = 0;
        turnCnt = 0;
        spawnCnt = 0;
        isTurn = false;
        spriteRenderer.flipX = false;
        isDie = false;
    }

    public void CharTurn()
    {
        isTurn = isTurn != true;

        spriteRenderer.flipX = isTurn;
        CharMove();
    }

    public void CharMove()
    {
        if(isDie)
            return;
        
        sound.Play();
        
        moveCnt++;
        
        MoveDirection();

        if (isFailTurn())
        {
            CharDie();
            return;
        }

        if (moveCnt > 5)
        {
            RespawnStairs();
        }
        
        GameManager.instance.AddScore();
    }

    private void MoveDirection()
    {
        if (isTurn) // Left
        {
            oldPosition += new Vector3(-0.75f, 0.5f, 0);
        }
        else // Right
        {
            oldPosition += new Vector3(0.75f, 0.5f, 0);
        }
        
        transform.position = oldPosition;
        animator.SetTrigger("Move");
    }

    private bool isFailTurn()
    {
        bool result = GameManager.instance.isTurn[turnCnt] != isTurn;
        
        turnCnt++;

        if (turnCnt > GameManager.instance.stairs.Length - 1)
        {
            turnCnt = 0;
        }
        
        return result;
    }

    private void RespawnStairs()
    {
        GameManager.instance.SpawnStair(spawnCnt);
        spawnCnt++;
        if (spawnCnt > GameManager.instance.stairs.Length - 1)
        {
            spawnCnt = 0;
        }
    }

    private void CharDie()
    {
        GameManager.instance.GameOver();
        animator.SetBool("Die", true);
        isDie = true;
    }

    public void ButtonRestart()
    {
        Init();
        GameManager.instance.Init();
        GameManager.instance.InitStairs();
    }
}
