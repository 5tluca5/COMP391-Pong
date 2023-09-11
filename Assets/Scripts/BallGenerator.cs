using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameController gameController;

    private void Start()
    {
        
    }
    public void GenerateBall(Players playerSide)
    {
        if (transform.childCount > 0) return;

        var ball = GameObject.Instantiate(ballPrefab, transform).GetComponent<Ball>();
        ball.rtfBody.anchoredPosition = GenerateSpawnSpot(playerSide);
        ball.AddForce(GenerateForce(playerSide));
        
    }

    Vector2 GenerateForce(Players playerSide)
    {
        Vector2 force = Vector2.zero;
        var randomUpward = Random.Range(0, 2) == 0;

        if(playerSide == Players.Player1)
        {
            force = new Vector2(-250, 250 * (randomUpward ? 1 : -1));
        }
        else
        {
            force = new Vector2(250, 250 * (randomUpward ? 1 : -1));

        }

        return force;
    }

    Vector2 GenerateSpawnSpot(Players playerSide)
    {
        Vector2 spot = Vector2.zero;

        spot = new Vector2(10 * (playerSide == Players.Player1 ? -1 : 1), Random.Range(-gameController.maxMoveDistance, gameController.maxMoveDistance));

        return spot;
    }
}
