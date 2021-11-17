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
    public Hook HookProjectile;
    
    private Camera mainCamera;
    [HideInInspector] public LineRenderer _LineRenderer;
    [HideInInspector] public DistanceJoint2D _DistanceJoint;

    [HideInInspector] public Vector2 AnchorPoint = Vector2.zero;
    [HideInInspector] public Vector2 PlayerPos = Vector2.zero;
    [HideInInspector] public Vector2 CrossHair = Vector2.zero;
    public List<Vector3> RopePoints;
    
    public float MoveForce = 10f;
    public float ProjectilelaunceStrength = 100f;

    public LayerMask WallLayer;
    public bool RopeAttach = false;
    
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

        Collider2D.size = new Vector2(3.71f, 12f);
        Collider2D.offset = new Vector2(0, 5.7f);
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
        }
    }

    void LauncheRope()
    {
        Vector2 rayDirection = (CrossHair - PlayerPos).normalized;

        Debug.DrawLine(PlayerPos,PlayerPos + rayDirection);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (RopeAttach)
            {
                HookProjectile.IsActive = false;
                DetachRope(); 
            }
            else
            {
                if (HookProjectile == null)
                {
                    var proj = Instantiate(ProjectileType);
                    HookProjectile = proj.GetComponent<Hook>();
                    HookProjectile.SpawnHook(PlayerPos,rayDirection * ProjectilelaunceStrength, this);
                }
                else
                {
                    HookProjectile.SpawnHook(PlayerPos,rayDirection * ProjectilelaunceStrength, this); 
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

        //PlayerPoint = RopePoints[0];
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

            //RopePoints.Reverse();
        }

        AnchorPoint = RopePoints[RopePoints.Count - 1];

        RopeAttach = true;

        _DistanceJoint.distance = Vector2.Distance(PlayerPos, AnchorPoint);
        _DistanceJoint.connectedAnchor = AnchorPoint;
        _DistanceJoint.enableCollision = true;
        _DistanceJoint.enabled = true;

        /*
        int RopeCount = RopePoints.Count - 1;
        
        (float dist, int id) closest = ( float.MaxValue, -1 );
        for( int i = 0; i < RopeCount; i++ ) {
            float dist = SqDist( HitPos, RopePoints[i] );
            if( dist < closest.dist )
                closest = ( dist, i );
        }
        
        AnchorPoint = RopePoints[closest.id];*/
        
    }
    
    static float SqDist( Vector2 a, Vector2 b ) {
        float dx = b.x - a.x; // microoptimization. this is much faster than (a-b).sqrMagnitude.
        float dy = b.y - a.y; // we avoid a Vector2 subtract op, constructor, and property access this way 
        return dx * dx + dy * dy;
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
                    WheelBody.AddForce((Body.velocity.normalized + Vector2.up) * 400, ForceMode2D.Impulse);
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
        int Count = _LineRenderer.positionCount = RopePoints.Count;
        
        RopePoints[0] = PlayerPos;
        
        for (int i = 0; i < Count ; i++)
        {
            print("index = " + i + " RopePoints.Count = " + (_LineRenderer.positionCount) + " [RopeCount - i] = " + (Count - i));
        }

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

        ///PlayerPoint = RopePoints[0];
        AnchorPoint = RopePoints[RopePoints.Count - 1];

        float Distance = Vector2.Distance(PlayerPos, AnchorPoint);
        Vector2 RayDirection = (AnchorPoint - PlayerPos).normalized;
        Vector2 RayLength = RayDirection * Distance;
        
        //Debug.DrawLine(PlayerPos,PlayerPos + RayLength, Color.red);
        
        RaycastHit2D hit = Physics2D.Raycast(PlayerPos, RayLength, Distance,WallLayer);
        
        if (hit.collider != null)
        {
            print("hit");
            
            if (AnchorPoint != hit.point)
            {
                UpdateAnchor(hit.point);
                
                print("add hit.point to rope Positions");
                
                //RopePoints.Add(hit.point);
                //_DistanceJoint.distance = Distance;
            }
            
        }
    }

    Vector2 SetRope(int RopeIndex)
    {
        if ((RopeIndex - 1) != -1 )
        {
            
        }
        
        return Vector2.zero;
    }
    
    List<Vector2> ReverseList(List<Vector2> ToReverse)
    {
        for (int i = 0; i < ToReverse.Count - 1; i++)
        {
            Vector2 temp = ToReverse[i];
            ToReverse[i] = ToReverse[i + 1];
            ToReverse[i + 1] = temp;
        }

        return ToReverse;
    }
}