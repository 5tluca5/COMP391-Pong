using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemEffectController : MonoBehaviour
{
    public GameObject itemEffectTextPrefab;
    public List<Vector3> spawnPostions = new List<Vector3>();
    List<ItemEffectText> texts = new List<ItemEffectText>();

    public void GenerateEffectText(Item item)
    {
        RemoveEffectText(item);
        var iet = Instantiate(itemEffectTextPrefab, transform).GetComponent<ItemEffectText>();
        iet.SetText(item.itemType);
        iet.player = item.playerSide;
        iet.GetComponent<RectTransform>().anchoredPosition = spawnPostions[((int)item.playerSide) - 1];
        texts.Add(iet);
    }

    public void RemoveEffectText(Item item)
    {
        if (texts.Exists(x => x.player == item.playerSide))
        {
            var text = texts.First(x => x.player == item.playerSide);
            Destroy(text.gameObject);
            texts.Remove(text);
        }
    }

    public void Reset()
    {
       foreach(var text in texts)
        {
            Destroy(text.gameObject);
        }

        texts.Clear();
    }
}
