using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public Players playerSide;
    public BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void SetColliderSize(Vector2 size)
    {

    }
}
