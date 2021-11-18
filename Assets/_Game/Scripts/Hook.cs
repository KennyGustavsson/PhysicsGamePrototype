using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(CircleCollider2D),typeof(Rigidbody2D))]
public class Hook : MonoBehaviour
{
    public SpriteRenderer renderer;
    public CircleCollider2D collider2D;
    public Rigidbody2D body;
    public Grappel parent;
    public bool IsActive = true;

    private void Reset()
    {
        IsActive = false;
        body.simulated = false;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
        renderer.enabled = false;
        collider2D.enabled = false;
    }
    
    private void Initilazed()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        
        if (!body)
        {
            body = gameObject.AddComponent<Rigidbody2D>();
        }
        if (!collider2D)
        {
            collider2D = gameObject.AddComponent<CircleCollider2D>();
        }
        if (!renderer)
        {
            renderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("Collision" + other.transform.gameObject.name);

        if (other.gameObject.layer == 8)
        {
            if (!parent.RopeAttach)
            {
                parent.AttachRope(other.contacts[0].point);    
            }
            else
            {
                parent.DetachRope();
                parent.AttachRope(other.contacts[0].point);    
            }
        }
        
        Reset();
    }

    public void SpawnHook(Vector3 spawnLocation, Vector2 direction, Grappel spawnParent)
    {
        Initilazed();
        Reset();
        print("Shoot");
        transform.position = spawnLocation;
        parent = spawnParent;
        IsActive = true;
        body.simulated = true;
        body.velocity = Vector2.zero;
        renderer.enabled = true;
        collider2D.enabled = true;
        body.AddForce(direction, ForceMode2D.Impulse);

    }
}
