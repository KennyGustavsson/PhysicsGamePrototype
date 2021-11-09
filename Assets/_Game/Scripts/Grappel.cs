using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappel : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer _LineRenderer;
    public DistanceJoint2D _DistanceJoint;

    public Vector2 PlayerPoint = Vector2.zero;
    private Vector2 AnchorPoint = Vector2.zero;
    public List<Vector2> RopePoints;
    
    public Vector2 PlayerPos = Vector2.zero;
    public Vector2 CrossHair = Vector2.zero;
    public float RobeDistance = 100f;

    public LayerMask WallLayer;
    public bool RopeAttach = false;

    public Vector2 rayDirection;
    
    void Awake()
    {
        _LineRenderer = GetComponent<LineRenderer>();
        _DistanceJoint = GetComponent<DistanceJoint2D>();
        mainCamera = Camera.main;
        
        if (!_LineRenderer)
        {
            _LineRenderer = gameObject.AddComponent<LineRenderer>();
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
        
        PlayerPos = transform.position;

        Ray();
        RenderRope();
    }

    void Ray()
    {
        rayDirection = (CrossHair - PlayerPos).normalized;

        Debug.DrawLine(PlayerPos,PlayerPos + rayDirection);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(PlayerPos, rayDirection, RobeDistance,WallLayer);
            
            if (hit.collider != null)
            {
                AttachRope(hit.point);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DetachRope();
        }
    }

    void AttachRope(Vector2 HitPos)
    {
        RopePoints.Add(PlayerPos);
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
                print("index = " + i + " RopePoints.Count = " + (RopePoints.Count - 1));
                
                RopePoints[0] = PlayerPos;
            
                _LineRenderer.SetPosition(i,RopePoints[i]);
                
                Debug.DrawLine(RopePoints[i],RopePoints[i]);
            } 
        }
    }
}
