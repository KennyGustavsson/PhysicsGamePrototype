using System.Collections;
using UnityEngine;

public class PistonMovement : MonoBehaviour
{
    private Rigidbody2D pistonTopRB;
    private DistanceJoint2D distanceJoint;
    [SerializeField] private float force = 3000f;
    [SerializeField] private float interval = 4f;
    [SerializeField] private bool startActive = false;
    private float timer = 0;

    private WaitForSeconds Wait;

    private void Awake()
    {
        pistonTopRB = GetComponentInChildren<Rigidbody2D>();
        distanceJoint = GetComponentInChildren<DistanceJoint2D>();
        Wait = new WaitForSeconds(1f);
    }

    private void Start()
    {
        if (startActive)
        {
            timer = interval / 2;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0;
            StartCoroutine(Push());            
        }
    }

    private IEnumerator Push()
    {        
        distanceJoint.enabled = true;
        pistonTopRB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        yield return Wait;
        distanceJoint.enabled = false;
    }
}
