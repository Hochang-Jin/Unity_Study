using System;
using UnityEngine;

public class Enemy : Creature
{
    private void Awake()
    {
        base.Awake();
        // 시작 시 가운데로 걸어옴 
        Vector3 targetPosition = new Vector3(1, 0, 0);
        StartCoroutine(WalkToCenter(targetPosition));
    }

    public void SetStatus(float HP = 10, float MaxHP = 10, float Attack = 3, float Defense = 1)
    {
        this.health = HP;
        this.healthMax = MaxHP;
        this.attack = Attack;
        this.defense = Defense;
    }
}
