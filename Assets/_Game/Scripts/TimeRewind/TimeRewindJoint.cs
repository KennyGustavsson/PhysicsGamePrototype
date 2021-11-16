using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindJoint : MonoBehaviour
{
	[NonSerialized] public bool IsRewindingTime = false;
	private HingeJoint2D HingeJoint;
	private TimeManager Manager;
	
	#region TimeData
	private struct RewindTimeData
	{
		public bool Enabled;
	}
    
	private List<RewindTimeData> RewindList = new List<RewindTimeData>();
	#endregion

	private void Awake() => HingeJoint = GetComponent<HingeJoint2D>();

	private void Start()
	{
		Manager = TimeManager.Instance;
		Manager.TimeRewindJoints.Add(this);
	}

	private void OnDestroy()
	{
		Manager.TimeRewindJoints.Remove(this);
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
		TimeData.Enabled = HingeJoint.enabled;

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
		HingeJoint.enabled = TimeData.Enabled;

		// Remove Last
		RewindList.RemoveAt(RewindList.Count - 1);
	}
}
