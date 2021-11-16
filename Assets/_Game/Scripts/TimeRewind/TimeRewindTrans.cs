using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindTrans : MonoBehaviour
{
       private Rigidbody2D Rigidbody;
    private TimeManager Manager;
    [NonSerialized] public bool IsRewindingTime = false;

#region TimeData
    private struct RewindTimeData
    {
        public Quaternion Rot;
        public Vector3 Pos;
        public Vector3 Scale;
    }
    
    private List<RewindTimeData> RewindList = new List<RewindTimeData>();
#endregion

    private void Awake() => Rigidbody = GetComponent<Rigidbody2D>();

    private void Start()
    {
        Manager = TimeManager.Instance;
        Manager.TimeRewindTranses.Add(this);
    }

    private void OnDestroy()
    {
        Manager.TimeRewindTranses.Remove(this);
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

        // Save trans data
        TimeData.Pos = trans.localPosition;
        TimeData.Rot = trans.localRotation;
        TimeData.Scale = trans.localScale;

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

        // Transform time data
        transform.localPosition = TimeData.Pos;
        transform.localRotation = TimeData.Rot;
        transform.localScale = TimeData.Scale;

        // Remove Last
        RewindList.RemoveAt(RewindList.Count - 1);
    }
}
