using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using TMPro;

public class Item : MonoBehaviour
{
    public Item() { }
    public Item(ItemType type, Players player)
    {
        this.itemType = type;
        this.playerSide = player;
    }

    public ItemType itemType;
    public Players playerSide;
    public TextMeshProUGUI text;

    Subject<bool> onDestroy = new Subject<bool>();

    // Start is called before the first frame update
    void Start()
    {
        SetText();

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DORotate(new Vector3(0, 180, 0), 1f));
        mySequence.Append(transform.DORotate(new Vector3(0, 360, 0), 1f));
        mySequence.SetLoops(-1);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(ItemType type, Players player)
    {
        this.itemType = type;
        this.playerSide = player;
    }

    public void SetSpawnPos(Vector3 pos)
    {
        this.transform.localPosition = pos;
    }

    void SetText()
    {
        switch(itemType)
        {
            case ItemType.Capture:
                text.text = "C";
                break;
            case ItemType.Freeze:
                text.text = "F";
                break;
            case ItemType.DoublePadding:
                text.text = "D";
                break;
            case ItemType.Turbo:
                text.text = "T";
                break;


            default:
                text.text = "?";
                break;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            GameController.Instance.TriggerItem(this);
            onDestroy.OnNext(true);
        }
    }

    public Subject<bool> SubscribeOnDestroy()
    {
        return onDestroy;
    }
}
