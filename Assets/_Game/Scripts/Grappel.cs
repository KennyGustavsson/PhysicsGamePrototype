using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grappel : MonoBehaviour
{
    [FormerlySerializedAs("DoFuckingThing")] public bool Magic = true;
    [HideInInspector] public CapsuleCollider2D Collider2D;
    [HideInInspector] public Rigidbody2D Body;
    
    public GameObject OldParent;
    public GameObject wheel;
    public GameObject ProjectileType;
    [HideInInspector] public Hook HookProjectile;
    
    private Camera mainCamera;
    [HideInInspector] public LineRenderer _LineRenderer;
    [HideInInspector] public DistanceJoint2D _DistanceJoint;

    [HideInInspector] public Vector2 AnchorPoint = Vector2.zero;
    [HideInInspector] public Vector2 PlayerPos = Vector2.zero;
    [HideInInspector] public Vector2 CrossHair = Vector2.zero;
    
    [HideInInspector] public int PlayerIndex;
    [HideInInspector] public int AnchorIndex;
    
    public List<Vector3> RopePoints;
    
    public float MoveForce = 10f;
    public float ProjectilelaunceStrength = 100f;
    public float jumpForwardStrength = 400f;
    public float jumpUpStrength = 400f;
    //public float realInStrength = 0f;

    public LayerMask WallLayer;
    public bool RopeAttach = false;
    
    //RealIn
    public float speed = 0.01f;
    private float current = 0;
    private float  target = 1;
    private bool RealIn;
    
    void Awake()
    {
        _LineRenderer = GetComponent<LineRenderer>();
        _DistanceJoint = GetComponent<DistanceJoint2D>();
        Body = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<CapsuleCollider2D>();
        mainCamera = Camera.main;
        
        if (!_LineRenderer)
        {
            _LineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        if (!_DistanceJoint)
        {
            _DistanceJoint = gameObject.AddComponent<DistanceJoint2D>();
        }
        if (!Body)
        {
            Body = gameObject.AddComponent<Rigidbody2D>();
        }
        if (!Collider2D)
        {
            Collider2D = gameObject.AddComponent<CapsuleCollider2D>();
        }

        Collider2D.offset = new Vector2(0f, 5.7f);
        Collider2D.size = new Vector2(3.71f, 12f);
        Collider2D.enabled = false;
        _DistanceJoint.enabled = false;
        _LineRenderer.enabled = false;
        _LineRenderer.positionCount = 0;
    }

    void Update()
    {
        CrossHair = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        
        PlayerPos = transform.position;

        LauncheRope();
        CheackRay();
        RenderRope();
        
        if (Input.GetButtonDown("Jump"))
        {
            current = 0f;
            RealIn = true;
        }
            
        if (Input.GetButtonUp("Jump"))
        {
            RealIn = false;
        }
        
        if (RopeAttach)
        {
            if (Input.GetKey(KeyCode.D))
            {
                Body.AddForce(new Vector2(MoveForce,0));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Body.AddForce(new Vector2(-MoveForce,0));
            }

            if (RealIn)
            {
                current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, AnchorPoint, current);
            }
        }
    }

    void LauncheRope()
    {
        Vector2 rayDirection = (CrossHair - PlayerPos).normalized;

        Debug.DrawLine(PlayerPos,PlayerPos + rayDirection);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //RopePoints.Clear();
            
            if (RopeAttach)
            {
                HookProjectile.IsActive = false;
                DetachRope(); 
            }
            else
            {
                Vector2 dirStrength = rayDirection * ProjectilelaunceStrength;

                if (HookProjectile == null)
                {
                    var proj = Instantiate(ProjectileType);
                    HookProjectile = proj.GetComponent<Hook>();
                    HookProjectile.SpawnHook(PlayerPos,Body.velocity + dirStrength, this);
                }
                else
                {
                    HookProjectile.SpawnHook(PlayerPos,Body.velocity + dirStrength, this); 
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.P))
        {
            Debug.Break();
        }
    }

    public void AttachRope(Vector2 HitPos)
    {
        Collider2D.enabled = true;

        if (Magic)
        {
            OldParent = wheel.transform.root.gameObject;
            wheel.transform.SetParent(gameObject.transform);
            Rigidbody2D WheelBody = wheel.GetComponent<Rigidbody2D>();
            WheelBody.simulated = false;
        }

        //--------------------------//
        RopePoints.Add(PlayerPos);
        RopePoints.Add(HitPos);

        AnchorPoint = RopePoints[RopePoints.Count - 1];
        
        RopeAttach = true;
        _DistanceJoint.distance = Vector2.Distance(PlayerPos, AnchorPoint);
        _DistanceJoint.connectedAnchor = AnchorPoint;
        _DistanceJoint.enableCollision = true;
        _DistanceJoint.enabled = true;
    }
    
    void UpdateAnchor(Vector2 HitPos)
    {
        if (!RopePoints.Contains(HitPos))
        {
            RopePoints.Add(HitPos);
        }

        AnchorPoint = RopePoints[RopePoints.Count - 1];

        RopeAttach = true;

        _DistanceJoint.distance = Vector2.Distance(PlayerPos, AnchorPoint);
        _DistanceJoint.connectedAnchor = AnchorPoint;
        _DistanceJoint.enableCollision = true;
        _DistanceJoint.enabled = true;
    }

    public void DetachRope()
    {
        Collider2D.enabled = false;

        if (Magic)
        {
            Rigidbody2D WheelBody = wheel.GetComponent<Rigidbody2D>();
            if (WheelBody != null)
            {
                print(WheelBody);
                WheelBody.simulated = true;
                if (Body != null)
                {
                    WheelBody.velocity = Vector2.zero;
                    WheelBody.angularVelocity = 0;
                    Vector2 wheelVelocity = (Body.velocity.normalized * jumpForwardStrength) +
                                            (Vector2.up * jumpUpStrength);
                    WheelBody.AddForce(wheelVelocity, ForceMode2D.Impulse);
                }
                else
                {
                    print("body is null");
                }
            }
            wheel.transform.SetParent(OldParent.transform);  
        }

        RopePoints.Clear();
        RopeAttach = false;
        _DistanceJoint.enabled = RopeAttach;
        _LineRenderer.enabled = RopeAttach;
        _LineRenderer.positionCount = 0;
    }

    void RenderRope()
    {
        if (!RopeAttach)
        {
            return;
        }
        
        _LineRenderer.enabled = true;
        _LineRenderer.positionCount = RopePoints.Count;
        
        RopePoints[0] = PlayerPos;
        
        List<Vector3> points = new List<Vector3>();

        foreach (var point in RopePoints)
        {
            points.Add(point);
        }
        
        Vector3 player = points[0];
        points.RemoveAt(0);
        points.Add(player);
        _LineRenderer.SetPositions(points.ToArray());
    }

    private void OnDrawGizmos()
    {
        if (RopePoints.Count > 0)
        {
            int Count = RopePoints.Count - 1;
            
            for (int i = 0; i < Count ; i++)
            {
                Gizmos.DrawSphere(RopePoints[i], 0.2f);
                Gizmos.DrawLine(RopePoints[i],RopePoints[Count - i]);
            }
        }
    }

    void CheackRay()
    {
        if (!RopeAttach)
        {
            return;
        }
        
        AnchorPoint = RopePoints[RopePoints.Count - 1];

        float Distance = Vector2.Distance(PlayerPos, AnchorPoint);
        Vector2 RayDirection = (AnchorPoint - PlayerPos).normalized;
        Vector2 RayLength = RayDirection * Distance;
        
        RaycastHit2D hit = Physics2D.Raycast(PlayerPos, RayLength, Distance,WallLayer);
        
        if (hit.collider != null)
        {
            print("hit");
            
            if (AnchorPoint != hit.point)
            {
                UpdateAnchor(hit.point);
                
                print("add hit.point to rope Positions");
            }
        }
    }
}