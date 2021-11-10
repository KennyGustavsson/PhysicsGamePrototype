using Unity.Mathematics;
using UnityEngine;

public class JumpKing : MonoBehaviour
{
    public float Mass = 1.0f;
    private Rigidbody2D _body;

    private bool ChargeJump = false;
    public float JumpCharge = 0f;
    private bool Jump = false;
    
    private Vector2 PlayerPos;
    public float Multiplayer = 1;
    public float movementStrength = 100;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 JumpDirection = Vector2.zero;
    
    public LayerMask WallLayer;
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        PlayerPos = transform.position;
        
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

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        renderer.color = new Color(JumpCharge / Multiplayer, 0,0);
        
        //_body.velocity = CollisionCheck(_body.velocity, PlayerPos);
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
        _body.velocity = Reflect(_body.velocity, other.contacts[0].normal);
    }

    public Vector3 Reflect(Vector3 InDirection, Vector3 InNormal)
    {
        return -2.0f * Vector3.Dot(InNormal, InDirection) * InNormal + InDirection;
    }

    Vector2 CollisionCheck(Vector2 Vel, Vector2 Pos)
    {
        float Length = Vel.magnitude;
        Vector2 dir = Vel.normalized;
        
        RaycastHit2D hit = Physics2D.Raycast(Pos, dir, Length, WallLayer);

        Debug.DrawLine(Pos,Pos + dir);
        
        if (hit.collider != null)
        {
            Vector2 reflectedVector = Reflect(dir, hit.normal);
            float Impact = 0.5f * Mass * Mathf.Pow(Vel.magnitude, 2);

            Vel = (reflectedVector * Impact) / Mass; // New impulse when collide
        }

        return Vel;
    }

}
