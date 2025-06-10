using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    public GameObject target;
    private void Awake(){
        base.Awake();
        // 시작 시 가운데로 걸어옴 
        Vector3 targetPosition = new Vector3(-1, 0, 0);
        StartCoroutine(WalkToCenter(targetPosition));
        
        // status setting
        health = 10;
        healthMax = 10;
        attack = 3;
        defense = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack(target);
        }
    }
}