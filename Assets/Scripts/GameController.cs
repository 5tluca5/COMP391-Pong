using UniRx;
using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public enum Players : int
{
    Player1 = 1,
    Player2,
    Player3,
    Player4,
};

public class GameController : MonoBehaviour
{
    public Canvas canvas;
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;
    public BallGenerator ballGenerator;
    List<Player> players = new List<Player>();

    [Header("Score")]
    public TextMesh p1Score;
    public TextMesh p2Score;
    public TextMesh p3Score;
    public TextMesh p4Score;

    [Header("Start page")]
    public GameObject startPage;

    [Header("Result page")]
    public GameObject resultPage;
    public TMPro.TMP_Text winnerText;

    [Header("Game parameter")]
    public float maxMoveDistance;
    public float moveSpeedMultipler = 1.5f;
    public float moveSpeed;
    bool isGameOver = false;
    bool isGameStart = false;

    [Header("Items")]
    public ItemGenerator itemGenerator;
    public ItemEffectController itemEffectController;

    private Players lastWinner;
    private Item lastItem;

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

        players = new List<Player>() { player1, player2, player3, player4 };

        Init();
    }

    private void Update()
    {
        if(isGameStart)
        {
            itemGenerator.UpdateTimer();
        }
    }

    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetGame();
        }

        if(Input.GetKey(KeyCode.Space) && !startPage.activeSelf)
        {
            if(ballGenerator.IsBallAttached())
            {
                ballGenerator.ReleaseBall();
                itemEffectController.RemoveEffectText(lastItem);
            }
            else
            {

                if (ballGenerator.GenerateBall(lastWinner))
                    isGameStart = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ResetGame();
        }
    }

    private void FixedUpdate()
    {

        if (isGameOver) return;

        if(!player1.IsEnabledAI())
        {

            if (Input.GetKey(KeyCode.W))
            {
                MovePlayerVertical(player1, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                MovePlayerVertical(player1, -1);
            }

            //if (Input.GetKey(KeyCode.A))
            //{
            //    MovePlayerHorizontal(player2, -1);
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    MovePlayerHorizontal(player2, 1);
            //}
        }

        if (!player2.IsEnabledAI())
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MovePlayerVertical(player2, 1);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MovePlayerVertical(player2, -1);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MovePlayerVertical(player2, -1);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MovePlayerVertical(player2, 1);
            }
        }

    }
    private void Init()
    {
        isGameOver = false;
        maxMoveDistance = canvas.GetComponent<RectTransform>().sizeDelta.y / 2 - player1.rtfBody.sizeDelta.y * 0.75f;
        moveSpeed = moveSpeed * moveSpeedMultipler;
        lastWinner = Players.Player1;

        player1.score.Value = 0;
        player2.score.Value = 0;
        player3.score.Value = 0;
        player4.score.Value = 0;
        player1.SetPlayer(Players.Player1);
        player2.SetPlayer(Players.Player2);
        player3.SetPlayer(Players.Player3);
        player4.SetPlayer(Players.Player4);

        player4.score.Subscribe(score => {
            p4Score.text = score.ToString();
        }).AddTo(p4Score.gameObject);

        player3.score.Subscribe(score => {
            p3Score.text = score.ToString();
        }).AddTo(p3Score.gameObject);

        player2.score.Subscribe(score => {
            p2Score.text = score.ToString();
        }).AddTo(p2Score.gameObject);

        player1.score.Subscribe(score => {
            p1Score.text = score.ToString();
        }).AddTo(p1Score.gameObject);

        //OnClickPVC();
    }

    private void ResetGame()
    {
        if (!resultPage.activeSelf || !isGameOver) return;

        resultPage.SetActive(false);
        isGameOver = false;

        foreach(var player in players)
        {
            player.score.Value = 0;
        }

        player1.rtfBody.anchoredPosition = new Vector3(player1.rtfBody.anchoredPosition.x, 0);
        player2.rtfBody.anchoredPosition = new Vector3(0, player2.rtfBody.anchoredPosition.y);
        player3.rtfBody.anchoredPosition = new Vector3(player3.rtfBody.anchoredPosition.x, 0);
        player4.rtfBody.anchoredPosition = new Vector3(0, player4.rtfBody.anchoredPosition.y);

    }

    public void MovePlayerVertical(Player player, int direction)
    {
        if (player.IsFreezed()) return;

        player.rtfBody.Translate(new Vector3(0, moveSpeed * Time.deltaTime * player.GetSpeedBonus() * direction));
        //if(player.rtfBody.anchoredPosition.y < maxMoveDistance)
        //{
        //    player.rtfBody.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime * player.GetSpeedBonus());
        //}
        //else
        //{
        //    player.rtfBody.anchoredPosition = new Vector2(player.rtfBody.anchoredPosition.x, maxMoveDistance);
        //}
    }
    public void MovePlayerHorizontal(Player player, int direction)
    {
        if (player.IsFreezed()) return;

        player.rtfBody.Translate(new Vector3(moveSpeed * Time.deltaTime * player.GetSpeedBonus() * direction, 0));

        //if (player.rtfBody.anchoredPosition.y > -maxMoveDistance)
        //{
        //    player.rtfBody.anchoredPosition -= new Vector2(0, moveSpeed * Time.deltaTime * player.GetSpeedBonus());
        //}
        //else
        //{
        //    player.rtfBody.anchoredPosition = new Vector2(player.rtfBody.anchoredPosition.x, -maxMoveDistance);
        //}
    }

    public void GameOver(Players winner)
    {
        isGameStart = false;
        ballGenerator.Reset();
        itemGenerator.Reset();
        itemEffectController.Reset();
        player1.Reset();
        player2.Reset();
        player3.Reset();
        player4.Reset();

        switch (winner)
        {
            case Players.Player1:
                player1.score.Value += 1;
                break;
            case Players.Player2:
                player2.score.Value += 1;
                break;
            case Players.Player3:
                player3.score.Value += 1;
                break;
            case Players.Player4:
                player4.score.Value += 1;
                break;
        }

        lastWinner = (Players)UnityEngine.Random.Range(1, 5);

        if(player1.score.Value >= 3)
        {
            AnnounceTheWinner(Players.Player1);
        }
        else if (player2.score.Value >= 3)
        {
            AnnounceTheWinner(Players.Player2);
        }
        else if (player3.score.Value >= 3)
        {
            AnnounceTheWinner(Players.Player3);
        }
        else if (player4.score.Value >= 3)
        {
            AnnounceTheWinner(Players.Player4);
        }
    }

    public void AnnounceTheWinner(Players winner)
    {
        isGameOver = true;

        resultPage.SetActive(true);
        winnerText.text = string.Format("<color=#FFEE00>{0}</color>",winner.ToString()) + " Lose!";
    }

    public void TriggerItem(Item item)
    {
        var cloneItem = new Item(item.itemType, item.playerSide);
        itemEffectController.GenerateEffectText(cloneItem);
        lastItem = cloneItem;

        Subject<bool> sub = new Subject<bool>();

        switch(item.itemType)
        {
            case ItemType.Freeze:
                foreach(var p in players)
                {
                    if (p.GetPlayerSide() == cloneItem.playerSide) continue;

                    sub = p.SetFreeze(GameConstants.FREEZE_TIME);
                }
                break;
            case ItemType.Turbo:
                foreach (var p in players)
                {
                    if (p.GetPlayerSide() == cloneItem.playerSide) continue;

                    sub = p.SetTurbo(GameConstants.TURBO_TIME);
                }
                break;
            case ItemType.DoublePadding:
                foreach (var p in players)
                {
                    if (p.GetPlayerSide() != cloneItem.playerSide) continue;

                    sub = p.EnableExtraPaddings(GameConstants.DOUBLE_PADDLE_TIME);
                }
                break;
            case ItemType.Capture:

                switch(item.playerSide)
                {
                    case Players.Player1:
                        sub = player1.SetCaptureBall(GameConstants.CAPTURE_TIME);
                        ballGenerator.GetCurrentBall().AttachTo(player1, new Vector3(20, 0, -1));
                        break;
                    case Players.Player2:
                        sub = player2.SetCaptureBall(GameConstants.CAPTURE_TIME);
                        ballGenerator.GetCurrentBall().AttachTo(player2, new Vector3(0, -20, -1));
                        break;
                    case Players.Player3:
                        sub = player3.SetCaptureBall(GameConstants.CAPTURE_TIME);
                        ballGenerator.GetCurrentBall().AttachTo(player3, new Vector3(-20, 0, -1));
                        break;
                    case Players.Player4:
                        sub = player4.SetCaptureBall(GameConstants.CAPTURE_TIME);
                        ballGenerator.GetCurrentBall().AttachTo(player4, new Vector3(0, 20, -1));
                        break;
                }
                sub.Subscribe(_ =>
                {
                    ballGenerator.ReleaseBall();
                }).AddTo(this);

                //if (item.playerSide == Players.Player1)
                //{
                //    sub = player1.SetCaptureBall(GameConstants.CAPTURE_TIME);
                //    sub.Subscribe(_ =>
                //    {
                //        ballGenerator.ReleaseBall();
                //    }).AddTo(this);
                //    ballGenerator.GetCurrentBall().AttachTo(player1, new Vector3(20, 0, -1));
                //}
                //else
                //{
                //    sub = player2.SetCaptureBall(GameConstants.CAPTURE_TIME);
                //    sub.Subscribe(_ =>
                //    {
                //        ballGenerator.ReleaseBall();
                //    }).AddTo(this);
                //    ballGenerator.GetCurrentBall().AttachTo(player2, new Vector3(-20, 0, -1));
                //}
                break;
        }

        sub.Subscribe(_ => {
            itemEffectController.RemoveEffectText(cloneItem);
            item.DestroyItem();
        }).AddTo(this);
    }

    public Ball GetBall()
    {
        return ballGenerator.GetCurrentBall();
    }

    public void OnClickPVC()
    {
        player2.SetAI(true);
        startPage.SetActive(false);
    }

    public void OnClickPVP()
    {
        player2.SetAI(false);
        startPage.SetActive(false);
    }

    public bool IsGameStarted()
    {
        return isGameStart;
    }
    
    public void PlayerJoinGame(Players[] players)
    {
        player1.SetAI(!players.Contains(Players.Player1));
        player2.SetAI(!players.Contains(Players.Player2));
        player3.SetAI(!players.Contains(Players.Player3));
        player4.SetAI(!players.Contains(Players.Player4));

        startPage.SetActive(false);
    }
}
