using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    private float maxSpeed = 5;
    private float jumpPower = 20;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;
    
    
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "jump":
                audioSource.clip = audioJump;
                break;
            case "attack":
                audioSource.clip = audioAttack;
                break;
            case "damaged":
                audioSource.clip = audioDamaged;
                break;
            case "item":
                audioSource.clip = audioItem;
                break;
            case "die":
                audioSource.clip = audioDie;
                break;
            case "finish":
                audioSource.clip = audioFinish;
                break;
        }
        
        audioSource.Play();
    }
    private void Update()
    {
        // 점프
        if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping"))
        {
            rigidBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            PlaySound("jump");
        }
        
        
        // 버튼을 그만 누르면 속도를 줄임
        if (Input.GetButtonUp("Horizontal"))
        {
            //rigidBody.linearVelocity = new Vector2(0.5f * rigidBody.linearVelocity.normalized.x, rigidBody.linearVelocity.y);
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
        }
        
        // 애니메이션
        if (Input.GetButton("Horizontal"))
        {
            // 애니메이션 방향전환
            if (rigidBody.linearVelocity.x != 0)
            {
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
                //spriteRenderer.flipX = rigidBody.linearVelocity.x < 0;
            }
            // 애니메이션 전환 (Idle -> Walking)
            animator.SetBool("isWalking", true);
            
        }
        
        // 애니메이션 전환 (Walikng -> Idle)
            // 키에서 손 떼면  Idle
        //if(Input.GetButtonUp("Horizontal"))
          //  animator.SetBool("isWalking", false);
          if(rigidBody.linearVelocity.x == 0)
              animator.SetBool("isWalking",false);
          else
              animator.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        // 좌우 움직임
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        rigidBody.AddForce(Vector2.right * moveHorizontal, ForceMode2D.Impulse);
        // 좌우 최대 속도 설정
        if (rigidBody.linearVelocity.x > maxSpeed)
        {
            rigidBody.linearVelocity = new Vector2(maxSpeed, rigidBody.linearVelocity.y);
        }
        else if (rigidBody.linearVelocity.x < -maxSpeed)
        {
            rigidBody.linearVelocity = new Vector2(-maxSpeed, rigidBody.linearVelocity.y);
        }
        
        // 착지 확인
        if (rigidBody.linearVelocity.y < 0)
        {
            Debug.DrawRay(rigidBody.position, Vector3.down, Color.yellow);
            RaycastHit2D rayHit = Physics2D.Raycast(rigidBody.position, Vector3.down, 1, LayerMask.GetMask("platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    animator.SetBool("isJumping", false);
                }
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (rigidBody.linearVelocity.y < 0 && transform.position.y > other.transform.position.y)
            {
                OnAttack(other.transform);
                rigidBody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            }
            else
                OnDamaged(other.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            // 점수
            bool isBronze = other.gameObject.name.Contains("Bronze");
            bool isSilver = other.gameObject.name.Contains("Silver");
            bool isGold = other.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stageScore += 50;
            if (isSilver)
                gameManager.stageScore += 100;
            if (isGold)
                gameManager.stageScore += 200;
            // Deactive
            other.gameObject.SetActive(false);
            
            PlaySound("item");
        }

        if (other.gameObject.tag == "Finish")
        {
            PlaySound("finish");
            // 다음 스테이지
            gameManager.NextStage();
            
        }
    }

    void OnAttack(Transform enemy)
    {
        // 점수
        gameManager.stageScore += 100;
        // 몹 사망
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
        PlaySound("attack");
    }
    private void OnDamaged(Vector2 position)
    {
        // Damaged
        gameManager.HealthDown();
        
        // Layer : Player - 8, PlayerDamaged - 9
        gameObject.layer = 9;
        
        // 살짝 투명하게
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        
        // 반작용
        int dirc = transform.position.x - position.x > 0 ? 1 : -1; // 오른쪽에서 맞았으면 오른쪽으로, 왼쪽에서 맞았으면 왼쪽으로 튐
        Debug.Log(dirc);
        Debug.Log("player:"+transform.position.x + ", enemy:" + position.x);
        rigidBody.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
        
        // 애니메이션
        animator.SetTrigger("doDamaged");
        
        PlaySound("damaged");
        
        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        
    }

    public void OnDie()
    {
        // sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // sprite flip y
        spriteRenderer.flipY = true;
        // collider Disable
        capsuleCollider.enabled = false;
        // die effect
        rigidBody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // play sound
        PlaySound("die");
    }

    public void VelocityZero()
    {
        rigidBody.linearVelocity = Vector2.zero;
    }
}
