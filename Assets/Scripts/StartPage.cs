using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class StartPage : MonoBehaviour
{
    public StartPageCell[] startPageCells;
    public GameObject startButton;

    public void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            SetPlayerActive(0);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetPlayerActive(1);
        }
    }

    void SetPlayerActive(int num)
    {
        if (startPageCells[num].IsActive()) return;

        startPageCells[num].SetActive(true);

        startButton.SetActive(true);
    }

    public void OnClickStartButton()
    {
        var playerList = startPageCells.Where(x => x.isActive).Select(x => x.playerNum).ToArray();

        GameController.Instance.PlayerJoinGame(playerList);
    }
}
