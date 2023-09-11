using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public RectTransform rtfBody;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        rtfBody = GetComponent<RectTransform>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
