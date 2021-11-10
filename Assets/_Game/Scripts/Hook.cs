using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{
    
    [NonSerialized] public Grappel Parent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.layer);

        int objectLayer = other.gameObject.layer;
        int grappelLayer = LayerMask.GetMask(Parent.GrappelLayer.ToString());// Parent.GrappelLayer.value;
       
        print("objectLayer = " +objectLayer + " grappelLayer = " + grappelLayer);
        
        if (objectLayer == grappelLayer)
        {
            Parent.AttachRope(transform.position);
            print("Right Layer");
            Destroy(gameObject);
        }
        
        
        
    }
}
