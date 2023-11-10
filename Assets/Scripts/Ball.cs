using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public RectTransform rtfBody;
    Rigidbody2D body;
    Vector3 lastVelocity;

    public int collideCount = 0;

    Transform parent;
    Players attachedPlayer;

    private void Awake()
    {
        rtfBody = GetComponent<RectTransform>();
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        parent = transform.parent;
    }
    // Start is called before the first frame update
    void Start()
    {
        //body = GetComponent<Rigidbody2D>();
        //body.gravityScale = 0f;
        //body.AddForce(new Vector2(-490, -10));
    }

    public void AddForce(Vector2 force)
    {
        transform.parent = parent;
        body.AddForce(force);
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = body.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float bonus = 1;
        if(collision.gameObject.CompareTag("Player"))
        {
            collideCount++;

            if(collideCount % 5 == 0)
            {
                bonus = 1 + (collideCount / 50f);
            }
        }

        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        body.velocity = direction * Mathf.Max(speed * bonus, 0);

        Debug.Log(string.Format("speed: {0}, bonus: {1}, count: {2}", speed, bonus, collideCount));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var wall = other.GetComponent<SideWall>();

        if (wall != null)
        {
            GameController.Instance.GameOver(wall.playerSide);

            Destroy(gameObject, 0.05f);
        }

    }

    public void AttachTo(Player player, Vector3 offset)
    {
        transform.parent = player.transform;
        transform.localPosition = offset;
        body.velocity = Vector3.zero;

        attachedPlayer = player.GetPlayerSide();
    }

    public bool IsAttaching()
    {
        return transform.parent.gameObject.CompareTag("Player");
    }

    public Players GetAttachedPlayerSide()
    {
        return attachedPlayer;
    }
}
