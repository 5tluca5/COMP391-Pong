using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    public float endValue = 0;
    public float moveTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        var curY = transform.localPosition.y;
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetEase(Ease.Linear);
        mySequence.Append(transform.DOLocalMoveY(endValue, moveTime));
        //mySequence.AppendInterval(0.5f);
        mySequence.Append(transform.DOLocalMoveY(curY, moveTime));
        //mySequence.AppendInterval(0.5f);
        mySequence.SetLoops(-1);
        //transform.DOLocalMoveY(-50f, 0.5f).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
