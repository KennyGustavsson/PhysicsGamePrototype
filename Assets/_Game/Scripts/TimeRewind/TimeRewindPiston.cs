using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindPiston : MonoBehaviour
{
    private Piston2 Piston;
    private TimeManager Manager;
    [NonSerialized] public bool IsRewindingTime = false;

#region TimeData
    private struct RewindTimeData
    {
        public float Accumulator;
        public float Timer;
        public bool isGoingUp;
    }
    
    private List<RewindTimeData> RewindList = new List<RewindTimeData>();
#endregion

    private void Awake() => Piston = GetComponent<Piston2>();

    private void Start()
    {
        Manager = TimeManager.Instance;
        Manager.TimeRewindPistons.Add(this);
    }

    private void OnDestroy()
    {
        Manager.TimeRewindPistons.Remove(this);
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
        TimeData.Accumulator = Piston.Accumulator;
        TimeData.Timer = Piston.Timer;
        TimeData.isGoingUp = Piston.isGoingUp;

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
        Piston.Accumulator = TimeData.Accumulator;
        Piston.Timer = TimeData.Timer;
        Piston.isGoingUp = TimeData.isGoingUp;

        // Remove Last
        RewindList.RemoveAt(RewindList.Count - 1);
    }
}
