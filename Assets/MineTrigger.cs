using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] rigidbodies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.gameObject.layer == 6)
        {
            foreach (var rb in rigidbodies)
            {
                rb.simulated = true;
            }
        }
    }
}
