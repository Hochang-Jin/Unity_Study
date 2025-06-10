using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalScore;
    public int stageScore;
    public int stageIndex;
    public int health;
    public PlayerMove PlayerMove;
    public GameObject[] stages;

    public Image[] UIHealths;
    public Text UIScore;
    public Text UIStage;
    public GameObject RestartButton;

    private void Update()
    {
        UIScore.text = (totalScore + stageScore).ToString();
    }

    public void NextStage()
    {
        // Change Stage
        if (stageIndex < stages.Length - 1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            PlayerReposition();
            
            UIStage.text = "STAGE " + (stageIndex + 1).ToString();
        }
        else
        {
            // control lock
            Time.timeScale = 0;
            
            // Result UI
            Debug.Log("game clear");
            Text btnText = RestartButton.GetComponentInChildren<Text>();
            btnText.text = "Restart";
            RestartButton.SetActive(true);
        }
        
        // Calculate Score
        totalScore += stageScore;
        stageScore = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIHealths[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            UIHealths[0].color = new Color(1, 0, 0, 0.4f);
            // player Die
            PlayerMove.OnDie();
            
            // result UI
            Debug.Log("Game Over");
            RestartButton.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // HealthDown
            HealthDown();
            
            // player Reposition
            if (health > 0)
            {
                PlayerReposition();
            }
        }
    }

    void PlayerReposition()
    {
        PlayerMove.transform.position = new Vector3(0, 0, -1);
        PlayerMove.VelocityZero();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Text btnText = RestartButton.GetComponentInChildren<Text>();
        btnText.text = "Replay?";
    }
}
