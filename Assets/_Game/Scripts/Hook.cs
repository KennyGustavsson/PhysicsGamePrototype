using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Grappel parent;

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("Collision" + other.transform.gameObject.name);

        if (other.transform.gameObject.layer == (1<<parent.WallLayer))
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
        
        Destroy(gameObject);
    }
}
