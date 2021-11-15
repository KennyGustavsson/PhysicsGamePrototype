using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Grappel : MonoBehaviour
{
    public bool DoFuckingThing = true;
    private CapsuleCollider2D Collider2D;
    private Rigidbody2D Body;
    
    public GameObject OldParent;
    public GameObject wheel;
    public GameObject ProjectileType;
    private GameObject HookProjectile;
    
    private Camera mainCamera;
    private LineRenderer _LineRenderer;
    private DistanceJoint2D _DistanceJoint;

    private Vector2 AnchorPoint = Vector2.zero;
    private Vector2 PlayerPos = Vector2.zero;
    private Vector2 CrossHair = Vector2.zero;
    public List<Vector2> RopePoints;
    
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

        //Collider2D.size = new Vector2(3.71f, 12f);
        //Collider2D.enabled = false;
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
                Destroy(HookProjectile);
                DetachRope(); 
            }
            else
            {
                HookProjectile = Instantiate(ProjectileType, PlayerPos, Quaternion.identity);
                HookProjectile.GetComponent<Rigidbody2D>().AddForce(rayDirection * ProjectilelaunceStrength, ForceMode2D.Impulse );
                HookProjectile.GetComponent<Hook>().parent = this;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            
            //DetachRope();
        }
        
        if (Input.GetKeyUp(KeyCode.P))
        {
            Debug.Break();
        }
    }

    public void AttachRope(Vector2 HitPos)
    {
        Collider2D.enabled = true;

        if (DoFuckingThing)
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

        if (DoFuckingThing)
        {
            Rigidbody2D WheelBody = wheel.GetComponent<Rigidbody2D>();
            if (WheelBody != null)
            {
                print(WheelBody);
                WheelBody.simulated = true;
                if (Body != null)
                {
                    WheelBody.velocity = Body.velocity;
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
        int Count = _LineRenderer.positionCount = RopePoints.Count - 1;
        
        RopePoints[0] = PlayerPos;

        for (int i = 1; i < Count ; i++)
        {
            print("index = " + i + " RopePoints.Count = " + (_LineRenderer.positionCount) + " [RopeCount - i] = " + (Count - i));
            
            _LineRenderer.SetPosition(0,RopePoints[0]);
            
            _LineRenderer.SetPosition(i,RopePoints[Count - i]);
        }
        /*
        for (int i = _LineRenderer.positionCount; i > 0; i--)
        {
            //print("index = " + i + " RopePoints.Count = " + (_LineRenderer.positionCount) + "RopePoints[RopeCount - i] = " + RopePoints[_LineRenderer.positionCount - i]);
                
            _LineRenderer.SetPosition(i,RopePoints[i]);
        } */
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