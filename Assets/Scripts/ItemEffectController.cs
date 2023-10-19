using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectController : MonoBehaviour
{
    public GameObject itemEffectTextPrefab;
    List<ItemEffectText> texts = new List<ItemEffectText>();

    public void GenerateEffectText(Item item)
    {
        RemoveEffectText(item);
        var iet = Instantiate(itemEffectTextPrefab, transform).GetComponent<ItemEffectText>();
        iet.SetText(item.itemType);
        iet.GetComponent<RectTransform>().anchoredPosition = new Vector3(490 * (item.playerSide == Players.Player1 ? -1 : 1), -167, 0);
        texts.Add(iet);
    }

    public void RemoveEffectText(Item item)
    {
        texts.ForEach(x =>
        {
            if ((x.transform.localPosition.x <= 0 && item.playerSide == Players.Player1) || (x.transform.localPosition.y >= 0 && item.playerSide == Players.Player2))
            {
                Destroy(x.gameObject);
                texts.Remove(x);
            }
        });
    }
}
