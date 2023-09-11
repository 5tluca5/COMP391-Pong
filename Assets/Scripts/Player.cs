using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour
{
    public RectTransform rtfBody;
    public ReactiveProperty<int> score = new ReactiveProperty<int>(0);

    // Start is called before the first frame update
    void Start()
    {
        rtfBody = GetComponent<RectTransform>();
        //score = new ReactiveProperty<int>(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
