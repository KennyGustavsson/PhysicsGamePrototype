using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unicycle))]
public class TimeRewindUnicycle : MonoBehaviour
{
	[NonSerialized] public bool IsRewindingTime = false;
	private Unicycle Unicycle;
	private TimeManager Manager;

    #region TimeData
    private struct RewindTimeData
	{
		public bool OnGround;
	}
    
	[NonSerialized] private List<RewindTimeData> RewindList = new List<RewindTimeData>();
#endregion

	private void Awake() => Unicycle = GetComponent<Unicycle>();

	private void Start()
	{
		Manager = TimeManager.Instance;
		Manager.TimeRewindUnicycle = this;
	}

	private void OnDestroy()
	{
		Manager.TimeRewindUnicycle = null;
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
        TimeData.OnGround = Unicycle.OnGround;

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

        // Unicycle data
        Unicycle.OnGround = TimeData.OnGround;

        // Remove Last
        RewindList.RemoveAt(RewindList.Count - 1);
    }
}
