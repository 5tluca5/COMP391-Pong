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

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        player1Item = null;
        player2Item = null;
    }

    // Update is called once per frame
    public void UpdateTimer()
    {
        timer += Time.deltaTime;

        if(player1Item && player2Item)
        {
            timer = 0;
            return;
        }

        if(timer >= itemSpawnInterval)
        {
            timer = 0;
            var player1Side = Random.Range(0, 2) == 0;
            var itemType = (ItemType)Random.Range(0, (int)ItemType.ItemTypeTotal);

            if(!player1Item && (player2Item || player1Side))
            {
                var pos = spawnPostions[Random.Range(0, spawnPostions.Count)] * new Vector2(-1, 1);

                player1Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
                player1Item.Init(itemType, Players.Player1);
                player1Item.SubscribeOnDestroy().Subscribe(_ =>
                {
                    if(_)
                    {
                        Destroy(player1Item.gameObject);
                        player1Item = null;
                    }
                }).AddTo(player1Item);
                player1Item.SetSpawnPos(pos);
            }
            else
            {
                var pos = spawnPostions[Random.Range(0, spawnPostions.Count)] * new Vector2(1, 1);

                player2Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
                player2Item.Init(itemType, Players.Player2);
                player2Item.SubscribeOnDestroy().Subscribe(_ =>
                {
                    if (_)
                    {
                        Destroy(player2Item.gameObject);
                        player2Item = null;
                    }
                }).AddTo(player2Item);
                player2Item.SetSpawnPos(pos);
            }
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
    }
}
