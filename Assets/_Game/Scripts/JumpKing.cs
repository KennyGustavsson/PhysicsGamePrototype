using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.IK;
using UnityEngine;

public class JumpKing : MonoBehaviour
{
    private Rigidbody2D _body;
    private bool ChargeJump = false;
    public float JumpCharge = 0f;
    private bool Jump = false;
    private Vector2 CrossHair;
    private Vector2 PlayerPos;
    private bool IsGroundeed = false;

    public float Multiplayer = 1;
    
    public float movementStrength = 100;
    private Vector2 movementInput = Vector2.zero;
    
    private Vector2 JumpDirection = Vector2.zero;

    public float ReflectMultiplayer = 1;
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            movementInput.x = movementStrength;
            JumpDirection.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementInput.x = -movementStrength;
            JumpDirection.x = -1;
        }
        else
        {
            //_body.velocity = new Vector2(0, _body.velocity.y);
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            ChargeJump = true;
        }
        
        if (Input.GetButtonUp("Jump"))
        {
            ChargeJump = false;
            Jump = true;
        }

        if (ChargeJump)
        {
            JumpCharge += Time.deltaTime * Multiplayer;
        }
        
        CrossHair = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        PlayerPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector2 JumpDir = (JumpDirection + Vector2.up).normalized;

        _body.AddForce(movementInput);
        
        movementInput = Vector2.zero;
        
        if (Jump)
        {
            _body.AddForce(JumpDir * JumpCharge, ForceMode2D.Impulse);
            JumpCharge = 0;
            Jump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _body.velocity = Reflect(_body.velocity * ReflectMultiplayer, other.contacts[0].normal);
    }

    public Vector3 Reflect(Vector3 InDirection, Vector3 InNormal)
    {
        return -2.0f * Vector3.Dot(InNormal, InDirection) * InNormal + InDirection;
    }

    Vector2 CollisionCheck(Vector3 Vel, Vector3 Pos)
    {
        Vector2 dir = _body.velocity.normalized;
        float Length = dir.magnitude;
        
        RaycastHit2D hit = Physics2D.Raycast(PlayerPos, dir, Length);

        if (hit.collider != null)
        {
            Vector2 reflectedVector = Reflect(_body.velocity, hit.normal);

            Vel = reflectedVector;
        }
        
        
        return Vel;
    }

}
