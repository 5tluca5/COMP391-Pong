using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPageCell : MonoBehaviour
{
    public Players playerNum;
    public Text playerTag;
    public GameObject joinedGO;

    public bool isActive = false;

    public void SetActive(bool active)
    {
        isActive = active;
        playerTag.text = active ? "P" + (int)playerNum : "CPU";
        joinedGO.SetActive(active);
    }

    public bool IsActive()
    {
        return isActive;
    }
}
