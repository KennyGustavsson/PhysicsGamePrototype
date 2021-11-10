using System;
using UnityEngine;

public class Unicycle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float Speed = 50f;
    [SerializeField] private float JumpForce = 600.0f;
    [SerializeField] private float MaxAngularVelocity = 360.0f;
    
    [Header("GroundCheck")]
    [SerializeField] private GameObject Body;
    [SerializeField] private float GroundRayCastLength = 2;
    [SerializeField] private int LayerMask = 7;
    
    [NonSerialized] public bool RagDolling = false;
    private Rigidbody2D rb;
    [NonSerialized] public bool OnGround = false;
    
#region Inputs
    private bool Jump;
    private bool Left;
    private bool Right;
    private bool Stop;
#endregion

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        var Hit = Physics2D.Raycast(transform.position, -Body.transform.right, GroundRayCastLength, ~(1<<LayerMask));
        OnGround = Hit.collider;
        
        if (OnGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump = true;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Stop = true;
        }
        
        Right = Input.GetKey(KeyCode.D);
        Left = Input.GetKey(KeyCode.A);
    }

    private void FixedUpdate()
    {
        if(RagDolling) return;
        
        if (Right)
        {
            rb.angularVelocity -= Speed * Time.deltaTime;
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
        }

        if (Left)
        {
            rb.angularVelocity += Speed * Time.deltaTime;
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
        }

        if (Stop)
        {
            rb.angularVelocity = 0f;
            Stop = false;
        }

        if (Jump)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Jump = false;
        }
    }
}
