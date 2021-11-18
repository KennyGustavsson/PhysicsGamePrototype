using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTriggerVolume : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] rigidbodies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.gameObject.layer == 6 || other.transform.gameObject.layer == 7)
        {
            foreach (var rb in rigidbodies)
            {
                if (rb)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
    }
}
