using System;
using Unity.Mathematics;
using UnityEngine;

public class Unicycle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float Speed = 50f;
    [SerializeField] private float JumpForce = 600.0f;
    [SerializeField] private float MaxAngularVelocity = 360.0f;
    [SerializeField] private GameObject CharacterObject;
    [SerializeField] private float LeanDegrees = 40.0f;
    [SerializeField] private float LeanSpeed = 1.0f;
    
    [Header("GroundCheck")]
    [SerializeField] private float GroundRayCastLength = 2;
    [SerializeField] private int LayerMask = 7;
    
    private Rigidbody2D rb;
    [NonSerialized] public bool RagDolling = false;
    [NonSerialized] public bool OnGround = false;

    private float Accumulator = 0.0f;
    private Quaternion StartRotation = Quaternion.identity;
    private bool LeaningRight = false;
    private bool LeaningLeft = false;


#region Inputs
    [NonSerialized] public bool Jump;
    [NonSerialized] public bool Left;
    [NonSerialized] public bool Right;
    [NonSerialized] public bool Stop;
    [NonSerialized] public bool LeanLeft;
    [NonSerialized] public bool LeanRight;
#endregion

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        var Hit = Physics2D.Raycast(transform.position, Vector3.down, GroundRayCastLength, ~(1<<LayerMask));
        OnGround = Hit.collider;
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

        if (OnGround && Jump)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Jump = false;
        }
        
        Leaning();
    }

    void Leaning()
    {
        float Target = 0.0f;
        
        if (LeanRight)
        {
            Target += LeanDegrees;

            if (!LeaningRight) ResetAccumulator();
            LeaningRight = true;
        }
        else
        {
            if (LeaningRight) ResetAccumulator();
            LeaningRight = false;
        }

        if (LeanLeft)
        {
            Target -= LeanDegrees;
            
            if (!LeaningLeft) ResetAccumulator();
            LeaningLeft = true;
        }
        else
        {
            if (LeaningLeft) ResetAccumulator();
            LeaningLeft = false;
        }

        Accumulator = Mathf.Clamp(Accumulator + Time.deltaTime / LeanSpeed, 0, 1);
        Quaternion TargetRotation = Quaternion.Euler(new Vector3(0, 0, Target));
        Quaternion NewRotation = Quaternion.Slerp(StartRotation, TargetRotation, Accumulator);
        
        CharacterObject.transform.localRotation = NewRotation;
    }

    private void ResetAccumulator()
    {
        Accumulator = 0.0f;
        StartRotation = CharacterObject.transform.localRotation;
    }
}
