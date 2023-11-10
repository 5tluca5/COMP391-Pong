using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemEffectText : MonoBehaviour
{
    CanvasGroup canvas;
    TMPro.TextMeshProUGUI text;
    public Players player;

    private void Awake()
    {

        canvas = GetComponent<CanvasGroup>();
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {

        Sequence mySequence = DOTween.Sequence();
        mySequence.SetEase(Ease.Linear);
        mySequence.Append(canvas.DOFade(0, 0.2f));
        mySequence.Append(canvas.DOFade(1, 0.2f));
        mySequence.SetLoops(-1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetText(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Capture:
                text.text = "Capture!!";
                break;
            case ItemType.Freeze:
                text.text = "Freeze!!";
                break;
            case ItemType.DoublePadding:
                text.text = "Double\nPadding!!";
                break;
            case ItemType.Turbo:
                text.text = "Turbo!!";
                break;


            default:
                text.text = "??????";
                break;

        }
    }
}
