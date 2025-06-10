using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    private CapsuleCollider2D collider;
    
    private int nextMove = 0;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        Think();
        
        Invoke("Think", 1);
    }
    
    void FixedUpdate()
    {
        // 기본 움직임
        rigidBody.linearVelocity = new Vector2(nextMove, rigidBody.linearVelocity.y);
        
        // 바닥 확인
        Vector2 frontVec = new Vector2(rigidBody.position.x + nextMove * 0.5f, rigidBody.position.y);
        
        Debug.DrawRay(frontVec, Vector3.down, Color.yellow);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }
        
    }

    void Think()
    {
        // 랜덤 움직임
        nextMove = Random.Range(-1, 2);
        // 스프라이트 세팅
        animator.SetBool("isWalking", nextMove!=0);
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
        // 재귀
        Invoke("Think", 1);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke("Think");
        Invoke("Think", 1);
    }

    public void OnDamaged()
    {
        // 투명도
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // y축 대칭
        spriteRenderer.flipY = true;
        // Collider 끄기
        collider.enabled = false;
        // 죽는 모션
        rigidBody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // Destroy
        Invoke("DeActive", 5);
        
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
