using UniRx;
using UnityEngine;

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
    public float moveSpeedMultipler = 1.5f;
    float moveSpeed;
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

        if (player1 == null)
            player1 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if (player2 == null)
            player2 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Player>();

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
    }

    private void Init()
    {
        isGameOver = false;
        maxMoveDistance = canvas.GetComponent<RectTransform>().sizeDelta.y / 2 - player1.rtfBody.sizeDelta.y;
        moveSpeed = Screen.safeArea.height * moveSpeedMultipler;
        lastWinner = Random.Range(0, 2) == 0 ? Players.Player1 : Players.Player2;

        player1.SetPlayer(Players.Player1);
        player2.SetPlayer(Players.Player2);

        player2.score.Subscribe(score => { 
            p2Score.text = score.ToString();
        }).AddTo(p2Score.gameObject);
        
        player1.score.Subscribe(score => {
            p1Score.text = score.ToString();
        }).AddTo(p1Score.gameObject);
    }

    public void MovePlayerUpward(Player player)
    {
        if (player.IsFreezed()) return;

        if(player.rtfBody.anchoredPosition.y < maxMoveDistance)
        {
            player.rtfBody.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime * player.GetSpeedBonus());
        }
        else
        {
            player.rtfBody.anchoredPosition = new Vector2(player.rtfBody.anchoredPosition.x, maxMoveDistance);
        }
    }
    public void MovePlayerDownward(Player player)
    {
        if (player.IsFreezed()) return;

        if (player.rtfBody.anchoredPosition.y > -maxMoveDistance)
        {
            player.rtfBody.anchoredPosition -= new Vector2(0, moveSpeed * Time.deltaTime * player.GetSpeedBonus());
        }
        else
        {
            player.rtfBody.anchoredPosition = new Vector2(player.rtfBody.anchoredPosition.x, -maxMoveDistance);
        }
    }

    public void GameOver(Players winner)
    {
        isGameStart = false;
        ballGenerator.Reset();
        itemGenerator.Reset();
        itemEffectController.Reset();
        player1.Reset();
        player2.Reset();

        switch (winner)
        {
            case Players.Player1:
                player1.score.Value += 1;
                lastWinner = Players.Player1;
                break;
            case Players.Player2:
                player2.score.Value += 1;
                lastWinner = Players.Player2;
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

    public void TriggerItem(Item item)
    {
        var cloneItem = new Item(item.itemType, item.playerSide);
        itemEffectController.GenerateEffectText(cloneItem);
        lastItem = cloneItem;

        Subject<bool> sub = new Subject<bool>();

        switch(item.itemType)
        {
            case ItemType.Freeze:
                if(item.playerSide == Players.Player1)
                {
                    sub = player2.SetFreeze(GameConstants.FREEZE_TIME);
                }
                else
                {
                    sub = player1.SetFreeze(GameConstants.FREEZE_TIME);
                }
                break;
            case ItemType.Turbo:
                if (item.playerSide == Players.Player1)
                {
                    sub = player2.SetTurbo(GameConstants.TURBO_TIME);
                }
                else
                {
                    sub = player1.SetTurbo(GameConstants.TURBO_TIME);
                }
                break;
            case ItemType.DoublePadding:
                if (item.playerSide == Players.Player1)
                {
                    sub = player1.EnableExtraPaddings(GameConstants.DOUBLE_PADDLE_TIME);
                }
                else
                {
                    sub = player2.EnableExtraPaddings(GameConstants.DOUBLE_PADDLE_TIME);
                }
                break;
            case ItemType.Capture:
                if (item.playerSide == Players.Player1)
                {
                    sub = player1.SetCaptureBall(GameConstants.CAPTURE_TIME);
                    sub.Subscribe(_ =>
                    {
                        ballGenerator.ReleaseBall();
                    }).AddTo(this);
                    ballGenerator.GetCurrentBall().AttachTo(player1, new Vector3(20, 0, -1));
                }
                else
                {
                    sub = player2.SetCaptureBall(GameConstants.CAPTURE_TIME);
                    sub.Subscribe(_ =>
                    {
                        ballGenerator.ReleaseBall();
                    }).AddTo(this);
                    ballGenerator.GetCurrentBall().AttachTo(player2, new Vector3(-20, 0, -1));
                }
                break;
        }

        sub.Subscribe(_ => {
            itemEffectController.RemoveEffectText(cloneItem);
            item.DestroyItem();
        }).AddTo(this);
    }
}
