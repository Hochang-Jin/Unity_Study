using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    private void Awake()
    {
        Enemy enemyObj = enemy.GetComponent<Enemy>();
        enemyObj.SetStatus();
        player.GetComponent<Player>().target = enemy;
    }
}
