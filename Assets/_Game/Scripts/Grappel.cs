using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grappel : MonoBehaviour
{
    private Rigidbody2D Body;
    public GameObject OldParent;
    public GameObject wheel;
    public Camera mainCamera;
    public LineRenderer _LineRenderer;
    public DistanceJoint2D _DistanceJoint;

    public Vector2 PlayerPoint = Vector2.zero;
    private Vector2 AnchorPoint = Vector2.zero;
    public List<Vector2> RopePoints;
    
    //public Vector2 PlayerPos = Vector2.zero;
    public Vector2 CrossHair = Vector2.zero;
    public float RobeDistance = 100f;

    public LayerMask GrappelLayer;
    public bool RopeAttach = false;

    public Vector2 rayDirection;

    public GameObject ProjectileType;
    private GameObject projectile;

    public float MoveForce = 10f;

    public float LaunchStrength = 1f;
    
    void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        _LineRenderer = GetComponent<LineRenderer>();
        _DistanceJoint = GetComponent<DistanceJoint2D>();
        mainCamera = Camera.main;
        
        if (!_LineRenderer)
        {
            _LineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        
        if (!Body)
        {
            Body = gameObject.AddComponent<Rigidbody2D>();
        }
        
        if (!_DistanceJoint)
        {
            _DistanceJoint = gameObject.AddComponent<DistanceJoint2D>();
        }
        
        _DistanceJoint.enabled = false;
        _LineRenderer.enabled = false;
    }

    void Update()
    {
        CrossHair = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        
        PlayerPoint = transform.position;

        Ray();
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

    void Ray()
    {
        rayDirection = (CrossHair - PlayerPoint).normalized;

        Debug.DrawLine(PlayerPoint,PlayerPoint + rayDirection);
       
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!RopeAttach)
            {
                projectile = Instantiate(ProjectileType,PlayerPoint + rayDirection, Quaternion.identity);
                Hook hook = projectile.AddComponent<Hook>();
                hook.Parent = this;

                Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
                
                projectileBody.AddForce(rayDirection * LaunchStrength, ForceMode2D.Impulse);

                /*
                RaycastHit2D hit = Physics2D.Raycast(PlayerPos, rayDirection, RobeDistance,GrappelLayer);
            
                if (hit.collider != null)
                {
                    AttachRope(hit.point);
                }                 
                */
            }
            else
            {

                DetachRope();
            }
        }
        /*
         else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
           DetachRope();
        }
        */
    }

    public void AttachRope(Vector2 HitPos)
    {
        OldParent = wheel.transform.root.gameObject;
                
        wheel.transform.SetParent(gameObject.transform);

        Rigidbody2D WheelBody = wheel.GetComponent<Rigidbody2D>();

        WheelBody.simulated = false;
        //--------------------------//
        RopePoints.Add(PlayerPoint);
        RopePoints.Add(HitPos);

        PlayerPoint = RopePoints[0];
        AnchorPoint = RopePoints[RopePoints.Count - 1];
        
        RopeAttach = true;
        _DistanceJoint.distance = Vector2.Distance(PlayerPoint, AnchorPoint);
        _DistanceJoint.connectedAnchor = AnchorPoint;
        _DistanceJoint.enableCollision = true;
        _DistanceJoint.enabled = true;
    }

    public void DetachRope()
    {
        Rigidbody2D WheelBody = wheel.GetComponent<Rigidbody2D>();

        WheelBody.simulated = true;

        WheelBody.velocity = Body.velocity;
                
        wheel.transform.SetParent(OldParent.transform);
        Destroy(projectile);
        
        
        RopePoints.Clear();
        RopeAttach = false;
        _DistanceJoint.enabled = false;
        _LineRenderer.enabled = false;
    }

    void RenderRope()
    {
        if (!RopeAttach)
        {
            return;
        }
        
        _LineRenderer.enabled = true;

        if (RopePoints.Count > 0)
        {
            _LineRenderer.positionCount = RopePoints.Count;
            
            for (int i = 0; i < RopePoints.Count; i++)
            {
                //print("index = " + i + " RopePoints.Count = " + (RopePoints.Count - 1));
                
                RopePoints[0] = PlayerPoint;
            
                _LineRenderer.SetPosition(i,RopePoints[i]);
                
                Debug.DrawLine(RopePoints[i],RopePoints[i]);
            } 
        }
    }
}
