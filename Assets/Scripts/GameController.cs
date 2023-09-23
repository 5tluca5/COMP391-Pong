using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum Players
{
    Player1,
    Player2,
};

public class GameController : MonoBehaviour
{
    public Canvas canvas;
    public Player player1;
    public Player player2;
    public BallGenerator ballGenerator;

    [Header("Score")]
    public TextMesh p1Score;
    public TextMesh p2Score;

    [Header("Result page")]
    public GameObject resultPage;
    public TMPro.TMP_Text winnerText;

    [Header("Game parameter")]
    public float maxMoveDistance;
    public float moveSpeed;
    bool isGameOver = false;

    private Players lastLoser;

    static GameController instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if(canvas == null)
            canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

        if (player1 == null)
            player1 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if (player2 == null)
            player2 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Player>();

        Init();
    }

    private void LateUpdate()
    {
        if (isGameOver) return;

        if(Input.GetKey(KeyCode.W))
        {
            MovePlayerUpward(player1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MovePlayerDownward(player1);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            MovePlayerUpward(player2);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MovePlayerDownward(player2);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            ballGenerator.GenerateBall(lastLoser);
        }
    }

    private void Init()
    {
        isGameOver = false;
        maxMoveDistance = canvas.GetComponent<RectTransform>().sizeDelta.y / 2 - 50;
        moveSpeed = Screen.safeArea.height;
        lastLoser = Random.Range(0, 2) == 0 ? Players.Player1 : Players.Player2;

        player2.score.Subscribe(score => { 
            p2Score.text = score.ToString();
        }).AddTo(p2Score.gameObject);
        
        player1.score.Subscribe(score => {
            p1Score.text = score.ToString();
        }).AddTo(p1Score.gameObject);
    }

    public void MovePlayerUpward(Player player)
    {
        if(player.rtfBody.anchoredPosition.y < maxMoveDistance)
        {
            player.rtfBody.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime);
        }
        else
        {
            player.rtfBody.anchoredPosition = new Vector2(player.rtfBody.anchoredPosition.x, maxMoveDistance);
        }
    }
    public void MovePlayerDownward(Player player)
    {
        if (player.rtfBody.anchoredPosition.y > -maxMoveDistance)
        {
            player.rtfBody.anchoredPosition -= new Vector2(0, moveSpeed * Time.deltaTime);
        }
        else
        {
            player.rtfBody.anchoredPosition = new Vector2(player.rtfBody.anchoredPosition.x, -maxMoveDistance);
        }
    }

    public void GameOver(Players winner)
    {
        switch (winner)
        {
            case Players.Player1:
                player1.score.Value += 1;
                break;
            case Players.Player2:
                player2.score.Value += 1;
                break;
        }

        if(player1.score.Value >= 3)
        {
            AnnounceTheWinner(Players.Player1);
        }
        else if (player2.score.Value >= 3)
        {
            AnnounceTheWinner(Players.Player2);
        }
    }

    public void AnnounceTheWinner(Players winner)
    {
        isGameOver = true;

        resultPage.SetActive(true);
        winnerText.text = string.Format("<color=#FFEE00>{0}</color>",winner.ToString()) + " Win!";
    }
}
