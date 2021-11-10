using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicycle : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocity = 50f;
    [SerializeField] private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float inputSmoothing;
    public bool RagDolling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //private void Update()
    //{
    //    currentInputVector = Vector2.SmoothDamp(currentInputVector, new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), ref smoothInputVelocity, inputSmoothing);

    //    Vector3 pos = transform.position;
    //    pos += new Vector3(currentInputVector.x, 0f, 0f) * moveSpeed * Time.deltaTime;

    //    transform.position = pos;
    //    transform.eulerAngles -= new Vector3(0, 0, currentInputVector.x * rotationSpeed * Time.deltaTime);
    //}

    private void FixedUpdate()
    {
        if(RagDolling) return;
        
        if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity -= velocity;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity += velocity;

        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.angularVelocity = 0f;

        }
    }
}
