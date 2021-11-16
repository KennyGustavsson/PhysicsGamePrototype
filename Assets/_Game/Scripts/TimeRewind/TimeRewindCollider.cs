using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindCollider : MonoBehaviour
{
	[NonSerialized] public bool IsRewindingTime = false;
	private Collider2D Collider;
	private TimeManager Manager;
	
	#region TimeData
	private struct RewindTimeData
	{
		public bool Enabled;
	}
    
	private List<RewindTimeData> RewindList = new List<RewindTimeData>();
	#endregion

	private void Awake() => Collider = GetComponent<Collider2D>();

	private void Start()
	{
		Manager = TimeManager.Instance;
		Manager.TimeRewindColliders.Add(this);
	}

	private void OnDestroy()
	{
		Manager.TimeRewindColliders.Remove(this);
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
		TimeData.Enabled = Collider.enabled;

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
		Collider.enabled = TimeData.Enabled;

		// Remove Last
		RewindList.RemoveAt(RewindList.Count - 1);
	}
}
