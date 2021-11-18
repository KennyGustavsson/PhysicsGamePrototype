using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindDistanceJoint : MonoBehaviour
{
    [NonSerialized] public bool IsRewindingTime = false;
    private DistanceJoint2D _DistanceJoint;
    
    private TimeManager Manager;
    
    private struct RewindTimeData
    {
        public bool Enabled;
        public Vector2 AnchorPoint;
    }
    
    private List<RewindTimeData> RewindList = new List<RewindTimeData>();

    private void Awake()
    {
        _DistanceJoint = GetComponent<DistanceJoint2D>();
    }

    private void Start()
    {
        Manager = TimeManager.Instance;
        Manager.TimeRewindDistanceJoint.Add(this);
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
        
        // Save data
        TimeData.Enabled = _DistanceJoint.enabled;
        TimeData.AnchorPoint = _DistanceJoint.connectedAnchor;

        // Add time data to list
        RewindList.Add(TimeData);
        
        // If over max remove oldest
        if (RewindList.Count >  Manager.MaxRewindFrames)
        {
            RewindList.RemoveAt(0);
        }
    }
    
    public void RewindTime()
    {
        if (RewindList.Count == 0) return;
        
        // Get List
        RewindTimeData TimeData = RewindList[^1];

        // Unicycle data
        _DistanceJoint.enabled = TimeData.Enabled;
        _DistanceJoint.connectedAnchor = TimeData.AnchorPoint;

        // Remove Last
        RewindList.RemoveAt(RewindList.Count - 1);
    }
}
