using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public RectTransform rtfBody;
    public ReactiveProperty<int> score = new ReactiveProperty<int>(0);
    public List<GameObject> paddings = new List<GameObject>();
    public Image effectGO;

    Players player;

    float freezeTimer = 0;
    float turboTimer = 0;
    float doublePaddingTimer = 0;
    float captureTimer = 0;

    Subject<bool> onItemEffectEnded = new Subject<bool>();

    private void Awake()
    {
        rtfBody = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rtfBody = GetComponent<RectTransform>();
        //score = new ReactiveProperty<int>(0);
    }

    public void SetPlayer(Players player)
    {
        this.player = player;
    }

    public Players GetPlayerSide()
    {
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if(freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            freezeTimer = Mathf.Max(freezeTimer, 0);

            if(freezeTimer <= 0)
            {
                onItemEffectEnded.OnNext(true);
                effectGO.gameObject.SetActive(false);
            }
        }

        if(turboTimer > 0)
        {
            turboTimer -= Time.deltaTime;
            turboTimer = Mathf.Max(turboTimer, 0);

            if(turboTimer <= 0)
            {
                onItemEffectEnded.OnNext(true);
                effectGO.gameObject.SetActive(false);
            }
        }

        if(doublePaddingTimer > 0)
        {
            doublePaddingTimer -= Time.deltaTime;
            doublePaddingTimer = Mathf.Max(doublePaddingTimer, 0);

            if (doublePaddingTimer <= 0)
            {
                onItemEffectEnded.OnNext(true);
                effectGO.gameObject.SetActive(false);
            }
        }

        if(captureTimer > 0)
        {
            captureTimer -= Time.deltaTime;
            captureTimer = Mathf.Max(captureTimer, 0);

            if (captureTimer <= 0)
            {
                onItemEffectEnded.OnNext(true);
                effectGO.gameObject.SetActive(false);
            }
        }
    }

    public void Reset()
    {
        freezeTimer = 0;
        turboTimer = 0;
        doublePaddingTimer = 0;


        effectGO.gameObject.SetActive(false);
        effectGO.color = Color.white;

        DisableExtraPaddings();
    }

    public bool IsFreezed()
    {
        return freezeTimer > 0;
    }

    public float GetSpeedBonus()
    {
        return turboTimer > 0 ? GameConstants.TURBO_EFFECT : 1;
    }

    public Subject<bool> SetFreeze(float freezeTime)
    {
        onItemEffectEnded = new Subject<bool>();
        freezeTimer = freezeTime;

        effectGO.gameObject.SetActive(true);
        effectGO.color = Color.blue;

        return onItemEffectEnded;
    }

    public Subject<bool> SetTurbo(float turboTime)
    {
        onItemEffectEnded = new Subject<bool>();
        turboTimer = turboTime;

        effectGO.gameObject.SetActive(true);
        effectGO.color = Color.yellow;

        return onItemEffectEnded;
    }

    public Subject<bool> EnableExtraPaddings(float time)
    {
        onItemEffectEnded = new Subject<bool>();
        doublePaddingTimer = time;

        foreach (var pad in paddings)
            pad.SetActive(true);

        return onItemEffectEnded;
    }

    public void DisableExtraPaddings()
    {
        foreach (var pad in paddings)
            pad.SetActive(false);
    }

    public Subject<bool> SetCaptureBall(float time)
    {
        onItemEffectEnded = new Subject<bool>();
        captureTimer = time;

        //effectGO.gameObject.SetActive(true);
        //effectGO.color = Color.green;

        return onItemEffectEnded;
    }
}
