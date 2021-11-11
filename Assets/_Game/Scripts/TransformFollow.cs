using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    [SerializeField] private Transform unicycle;
    [SerializeField] private float offset;

    private void Start()
    {
        offset = Vector2.Distance(unicycle.position, transform.position);
    }

    private void Update()
    {
        transform.position = new Vector3(unicycle.transform.position.x, unicycle.transform.position.y - offset, 0f);
        
    }
}
