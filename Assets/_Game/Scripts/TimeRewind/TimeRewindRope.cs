using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindRope : MonoBehaviour
{
    private Grappel _grappel;
    private SpriteRenderer renderer;
    private CapsuleCollider2D collider2D;
    private Rigidbody2D Rigidbody;
    private TimeManager Manager;
    [NonSerialized] public bool IsRewindingTime = false;

#region TimeData
    private struct RewindTimeData
    {
        public GameObject wheelParent;
        public List<Vector3> RopePoints;
        public Vector2 AnchorPoint;
        public Vector2 PlayerPos;
        public float distanceJointDistance;
        public bool RopeAttach;
        public bool distanceJointEnabled;
        public bool collider2D;
        public bool lineRendererEnabled;
    }
    
    private List<RewindTimeData> RewindList = new List<RewindTimeData>();
#endregion

    private void Awake()
    {
        _grappel = GetComponent<Grappel>();
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CapsuleCollider2D>();
        Manager = TimeManager.Instance;
        Manager.TimeRewindRopes.Add(this);
    }

    private void OnDestroy()
    {
        Manager.TimeRewindRopes.Remove(this);
    }

    public void SaveRewind()
    {
        // Time rewinding
        if (IsRewindingTime)
        {
            RewindTime();
            return;
        }
        
        RewindTimeData TimeData = new RewindTimeData();
        Transform trans = transform;

        // Save positions data
        TimeData.PlayerPos = _grappel.PlayerPos;
        TimeData.AnchorPoint = _grappel.AnchorPoint;
       
        // save distanceJoint
        TimeData.distanceJointDistance = _grappel._DistanceJoint.distance;
        TimeData.distanceJointEnabled = _grappel._DistanceJoint.enabled;
        // Save RopeAttach
        TimeData.RopeAttach = _grappel.RopeAttach;
        
        
        // Save collider
        TimeData.collider2D = collider2D.enabled;
        // Save Parent
        TimeData.wheelParent = _grappel.wheel.transform.parent.gameObject;
        TimeData.lineRendererEnabled = _grappel._LineRenderer.enabled;
        
        TimeData.RopePoints = new List<Vector3>();

        foreach (var points in _grappel.RopePoints)
        {
            TimeData.RopePoints.Add(points);
        }
        
        // Add time data to list
        RewindList.Add(TimeData);
        
        // If over max remove oldest
        if (RewindList.Count >  Manager.MaxRewindFrames)
        {
            RewindList.RemoveAt(0);
        }
    }

    /// <summary>
    /// Function for rewinding time on object
    /// </summary>
    public void RewindTime()
    {
        if (RewindList.Count == 0) return;
        
        // Get List
        RewindTimeData TimeData = RewindList[^1];

        //  positions data
        _grappel.PlayerPos = TimeData.PlayerPos;
        _grappel.AnchorPoint = TimeData.AnchorPoint; 
        _grappel.RopePoints = TimeData.RopePoints;
        //  distanceJoint
        _grappel._DistanceJoint.distance = TimeData.distanceJointDistance;
        _grappel._DistanceJoint.enabled = TimeData.distanceJointEnabled;
        
        _grappel._LineRenderer.enabled = TimeData.lineRendererEnabled;
        //  RopeAttach
        _grappel.RopeAttach = TimeData.RopeAttach;
        //  collider
        collider2D.enabled = TimeData.collider2D;
        //  Parent
        _grappel.wheel.transform.SetParent(TimeData.wheelParent.transform);
        
        // Remove Last
        RewindList.RemoveAt(RewindList.Count - 1);
    }
}
