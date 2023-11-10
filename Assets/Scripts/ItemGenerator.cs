using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum ItemType : int
{
    Freeze = 0,
    Turbo,
    DoublePadding,
    Capture,
    ItemTypeTotal
}

public class ItemGenerator : MonoBehaviour
{
    public GameObject itemPrefab;
    public float itemSpawnInterval = 5f;
    public List<Vector3> spawnPostions = new List<Vector3>();

    float timer = 0f;

    Item player1Item = null;
    Item player2Item = null;
    Item player3Item = null;
    Item player4Item = null;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        player1Item = null;
        player2Item = null;
        player3Item = null;
        player4Item = null;
    }

    // Update is called once per frame
    public void UpdateTimer()
    {
        timer += Time.deltaTime;

        if(player1Item && player2Item && player3Item && player4Item)
        {
            timer = 0;
            return;
        }

        if(timer >= itemSpawnInterval)
        {
            timer = 0;
            var playerSide = (Players)Random.Range(1, 5);
            var itemType = (ItemType)Random.Range(0, (int)ItemType.ItemTypeTotal);
            var pos = spawnPostions[((int)playerSide)-1];

            switch(playerSide)
            {
                case Players.Player1:
                    {

                        if (player1Item)
                        {
                            timer = itemSpawnInterval;
                            break;
                        }

                        player1Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
                        player1Item.Init(itemType, Players.Player1);
                        player1Item.SubscribeOnDestroy().Subscribe(_ =>
                        {
                            if (_)
                            {
                                Destroy(player1Item.gameObject);
                                player1Item = null;
                            }
                        }).AddTo(player1Item);
                        player1Item.SetSpawnPos(pos);
                    }
                    break;
                case Players.Player2:
                    {

                        if (player2Item)
                        {
                            timer = itemSpawnInterval;
                            break;
                        }

                        player2Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
                        player2Item.Init(itemType, Players.Player2);
                        player2Item.SubscribeOnDestroy().Subscribe(_ =>
                        {
                            if (_)
                            {
                                Destroy(player2Item.gameObject);
                                player1Item = null;
                            }
                        }).AddTo(player2Item);
                        player2Item.SetSpawnPos(pos);
                    }
                    break;
                case Players.Player3:
                    {

                        if (player3Item)
                        {
                            timer = itemSpawnInterval;
                            break;
                        }

                        player3Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
                        player3Item.Init(itemType, Players.Player3);
                        player3Item.SubscribeOnDestroy().Subscribe(_ =>
                        {
                            if (_)
                            {
                                Destroy(player3Item.gameObject);
                                player3Item = null;
                            }
                        }).AddTo(player3Item);
                        player3Item.SetSpawnPos(pos);
                    }
                    break;
                case Players.Player4:
                    {

                        if (player4Item)
                        {
                            timer = itemSpawnInterval;
                            break;
                        }

                        player4Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
                        player4Item.Init(itemType, Players.Player4);
                        player4Item.SubscribeOnDestroy().Subscribe(_ =>
                        {
                            if (_)
                            {
                                Destroy(player4Item.gameObject);
                                player4Item = null;
                            }
                        }).AddTo(player4Item);
                        player4Item.SetSpawnPos(pos);
                    }
                    break;

            }
            //if (!player1Item && (player2Item || player1Side))
            //{
            //    var pos = spawnPostions[Random.Range(0, spawnPostions.Count)] * new Vector2(-1, 1);

            //player1Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
            //player1Item.Init(itemType, Players.Player1);
            //player1Item.SubscribeOnDestroy().Subscribe(_ =>
            //{
            //    if (_)
            //    {
            //        Destroy(player1Item.gameObject);
            //        player1Item = null;
            //    }
            //}).AddTo(player1Item);
            //player1Item.SetSpawnPos(pos);
            //}
            //else
            //{
            //    var pos = spawnPostions[Random.Range(0, spawnPostions.Count)] * new Vector2(1, 1);

            //    player2Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
            //    player2Item.Init(itemType, Players.Player2);
            //    player2Item.SubscribeOnDestroy().Subscribe(_ =>
            //    {
            //        if (_)
            //        {
            //            Destroy(player2Item.gameObject);
            //            player2Item = null;
            //        }
            //    }).AddTo(player2Item);
            //    player2Item.SetSpawnPos(pos);
            //}
        }
    }

    public void Reset()
    {
        timer = 0f;

        if (player1Item != null)
        {
            Destroy(player1Item.gameObject);
            player1Item = null;
        }

        if (player2Item != null)
        {
            Destroy(player2Item.gameObject);
            player2Item = null;
        }
        if (player3Item != null)
        {
            Destroy(player3Item.gameObject);
            player3Item = null;
        }

        if (player4Item != null)
        {
            Destroy(player4Item.gameObject);
            player4Item = null;
        }
    }
}
