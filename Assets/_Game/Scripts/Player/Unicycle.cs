using System;
using UnityEngine;

public class Unicycle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float Speed = 50f;
    [SerializeField] private float JumpForce = 600.0f;
    [SerializeField] private float MaxAngularVelocity = 360.0f;
    [SerializeField] private GameObject CharacterObject;
    [SerializeField] private GameObject AvatarObject;
    [SerializeField] private GameObject Spine2Object;
    [SerializeField] private float AirControl = 250.0f;
    
    [Header("Leaning")]
    [SerializeField] private float LeanDegrees = 40.0f;
    [SerializeField] private float LeanSpeed = 1.0f;

    [Header("Crouching")] 
    [SerializeField] private float CrouchAmount = -1.0f;
    [SerializeField] private float CrouchSpeed = 0.2f;
    [SerializeField] private float CrouchLeanDegrees = 20.0f;
    [SerializeField] private float CrouchSpineLeanDegrees = -50.0f;
    
    [Header("GroundCheck")]
    [SerializeField] private float GroundRayCastLength = 2;
    [SerializeField] private int LayerMask = 7;
    
    private Rigidbody2D rb;
    private Ragdoll Ragdoll;
    [NonSerialized] public bool RagDolling = false;
    [NonSerialized] public bool OnGround = false;

    private float LeaningAccumulator = 0.0f;
    private Quaternion StartRotation = Quaternion.identity;
    private bool LeaningRight = false;
    private bool LeaningLeft = false;

    private float CrouchingAccumulator = 0.0f;
    private float StartYPos = 0.0f;
    private bool LeaningCrouch = false;
    
#region Inputs
    [NonSerialized] public bool Jump;
    [NonSerialized] public bool Left;
    [NonSerialized] public bool Right;
    [NonSerialized] public bool Stop;
    [NonSerialized] public bool LeanLeft;
    [NonSerialized] public bool LeanRight;
    [NonSerialized] public bool Crouching;
#endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartYPos = AvatarObject.transform.localPosition.y;

        Ragdoll = transform.root.GetComponentInChildren<Ragdoll>();
    }

    private void Update()
    {
        var Hit = Physics2D.Raycast(transform.position, Vector3.down, GroundRayCastLength, ~(1<<LayerMask));
        OnGround = Hit.collider;
    }

    private void FixedUpdate()
    {
        RagDolling = Ragdoll.RagdollActive;
        if(RagDolling) return;
        
        if (Right)
        {
            rb.angularVelocity -= Speed * Time.deltaTime;
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
            
            if(!OnGround)
                rb.AddForce(new Vector2(AirControl, 0), ForceMode2D.Force);
        }

        if (Left)
        {
            rb.angularVelocity += Speed * Time.deltaTime;
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
            
            if(!OnGround)
                rb.AddForce(new Vector2(-AirControl, 0), ForceMode2D.Force);
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
        Crouch();
    }

    void Crouch()
    {
        if (Crouching)
        {
            CrouchingAccumulator = Mathf.Clamp(CrouchingAccumulator + Time.deltaTime / CrouchSpeed, 0, 1);
        }
        else
        {
            CrouchingAccumulator = Mathf.Clamp(CrouchingAccumulator - Time.deltaTime / CrouchSpeed, 0, 1);
        }

        // Position
        float NewY = Mathf.Lerp(0, CrouchAmount, CrouchingAccumulator);
        AvatarObject.transform.localPosition = new Vector3(0, StartYPos + NewY, 0);
        
        // Spine Rotation
        Quaternion TargetRotation = Quaternion.Euler(new Vector3(0, 0, CrouchSpineLeanDegrees));
        Quaternion NewRotation = Quaternion.Slerp(Quaternion.identity, TargetRotation, CrouchingAccumulator);
        Spine2Object.transform.localRotation = NewRotation;
    }
    
    void Leaning()
    {
        float Target = 0.0f;

        if (Crouching)
        {
            Target -= CrouchLeanDegrees;
            
            
            if(!LeaningCrouch) ResetAccumulator();
            LeaningCrouch = true;
        }
        else
        {
            if(!LeaningCrouch) ResetAccumulator();
            LeaningCrouch = false;
        }
        
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

        LeaningAccumulator = Mathf.Clamp(LeaningAccumulator + Time.deltaTime / LeanSpeed, 0, 1);
        Quaternion TargetRotation = Quaternion.Euler(new Vector3(0, 0, Target));
        Quaternion NewRotation = Quaternion.Slerp(StartRotation, TargetRotation, LeaningAccumulator);
        CharacterObject.transform.localRotation = NewRotation;
    }

    private void ResetAccumulator()
    {
        LeaningAccumulator = 0.0f;
        StartRotation = CharacterObject.transform.localRotation;
    }
}
