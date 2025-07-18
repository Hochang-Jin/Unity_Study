using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("계단")] [Space(10)] public GameObject[] stairs;

    public bool[] isTurn;

    private enum State
    {
        Start,
        Left,
        Right
    };
    private State state;
    private Vector3 oldPosition;

    [Header("UI")] [Space(10)] 
    public GameObject UI_GameOver;
    public TextMeshProUGUI textMaxScore;
    public TextMeshProUGUI textNowScore;
    public TextMeshProUGUI textShowScore;
    private int maxScore = 0;
    private int nowScore = 0;

    [Header("Audio")] [Space(10)] 
    private AudioSource sound;

    public AudioClip bgmSound;
    public AudioClip dieSound;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        sound = GetComponent<AudioSource>();
        
        Init();
        InitStairs();
    }

    public void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;
        
        isTurn = new bool[stairs.Length];
        for (int i = 0; i < stairs.Length; i++)
        {
            stairs[i].transform.position = Vector3.zero;
            isTurn[i] = false;
        }
        
        nowScore = 0;
        
        textShowScore.text = nowScore.ToString();
        
        UI_GameOver.SetActive(false);
        
        sound.clip = bgmSound;
        sound.Play();
        sound.loop = true;
        sound.volume = 0.4f;
    }
    
    public void InitStairs()
    {
        for (int i = 0; i < stairs.Length; i++)
        {
            switch (state)
            {
                case State.Start:
                    stairs[i].transform.position = new Vector3(0.75f, -0.1f, 0);
                    state = State.Right;
                    break;
                
                case State.Left:
                    stairs[i].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                    isTurn[i] = true;
                    break;
                
                case State.Right:
                    stairs[i].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                    isTurn[i] = false;
                    break;
                    
            }
            
            oldPosition = stairs[i].transform.position;

            if (i != 0)
            {
                int ran = Random.Range(0, 5);

                if (ran < 2 && i < stairs.Length - 1)
                {
                    state = state == State.Left ? State.Right : State.Left;
                }
            }
        }
    }

    public void SpawnStair(int cnt)
    {
        int ran = Random.Range(0, 5);

        if (ran < 2)
        {
            state = state == State.Left ? State.Right : State.Left;
        }
        
        switch (state)
        {
            case State.Left:
                stairs[cnt].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                isTurn[cnt] = true;
                break;
                
            case State.Right:
                stairs[cnt].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                isTurn[cnt] = false;
                break;
        }
        oldPosition = stairs[cnt].transform.position;
    }

    public void GameOver()
    {
        sound.loop = false;
        sound.Stop();
        sound.clip = dieSound;
        sound.Play();
        sound.volume = 1;
        
        StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1f);
        
        UI_GameOver.SetActive(true);

        if (nowScore > maxScore)
        {
            maxScore = nowScore;
        }
        
        textMaxScore.text = maxScore.ToString();
        textNowScore.text = nowScore.ToString();
    }

    public void AddScore()
    {
        nowScore++;
        textShowScore.text = nowScore.ToString();
        
    }
}
