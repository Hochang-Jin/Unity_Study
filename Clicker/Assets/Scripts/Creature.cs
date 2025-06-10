using System;
using System.Collections;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    protected float health;
    protected float healthMax;
    protected float healthRegen;
    
    protected float mana;
    protected float manaMax;
    protected float manaRegen;
    
    protected float attack;
    protected float attackSpeed;
    protected float defense;
    
    protected Animator animator;
    
    protected IEnumerator WalkToCenter(Vector3 targetPos)
    {
        float moveSpeed = 4f;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos; 
        // 여기서 걸어온 다음 할 동작 넣기 (ex: 애니메이션 idle 전환 등)
        animator.SetBool("isMove",false);
    }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isMove",true);
    }

    protected void Attack(GameObject target)
    {
        Creature targetCreature = target.GetComponent<Creature>();
        Animator animatorThis = GetComponent<Animator>();
        Animator targetAnimator = target.GetComponent<Animator>();
        
        // calculate Damage
        targetCreature.health -= this.attack - targetCreature.defense;
        
        // animation
        animatorThis.SetTrigger("isAttack");
        targetAnimator.SetTrigger("isDamaged");
        
    }
}
