using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Runtime.CompilerServices;

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

    private Players lastLoser;
    public BallGenerator ballGenerator;

    [Header("Game parameter")]
    public float maxMoveDistance;
    public float moveSpeed;

    static GameController instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static GameController Instance()
    {
        if(instance == null)
        {
            instance = new GameController();
        }

        return instance;
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
        maxMoveDistance = canvas.GetComponent<RectTransform>().sizeDelta.y / 2 - 50;
        lastLoser = Random.Range(0, 2) == 0 ? Players.Player1 : Players.Player2;
    }

    public void MovePlayerUpward(Player player)
    {
        if(player.rtfBody.anchoredPosition.y < maxMoveDistance)
        {
            player.rtfBody.anchoredPosition += new Vector2(0, moveSpeed);
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
            player.rtfBody.anchoredPosition -= new Vector2(0, moveSpeed);
        }
        else
        {
            player.rtfBody.anchoredPosition = new Vector2(player.rtfBody.anchoredPosition.x, -maxMoveDistance);
        }
    }
}
