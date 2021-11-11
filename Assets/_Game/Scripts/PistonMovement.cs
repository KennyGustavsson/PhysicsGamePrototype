using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D pistonTopRB;
    [SerializeField] private DistanceJoint2D distanceJoint;
    [SerializeField] private float force = 6000f;
    [SerializeField] private float refreshRate = 2f;
    private float timer = 0;
    [SerializeField] private bool startActive = false;

    private void Awake()
    {
        if (startActive)
        {
            timer = refreshRate / 2;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= refreshRate)
        {
            timer = 0;
            StartCoroutine(Push());            
        }
    }

    private IEnumerator Push()
    {
        distanceJoint.enabled = true;
        pistonTopRB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        distanceJoint.enabled = false;
    }
}
