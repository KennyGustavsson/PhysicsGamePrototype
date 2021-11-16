using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ragdoll))]
public class TimeRewindRagdoll : MonoBehaviour
{
	[NonSerialized] public bool IsRewindingTime = false;
	private Ragdoll Ragdoll;
	private TimeManager Manager;
	
	#region TimeData
	private struct RewindTimeData
	{
		public float LimbSolverWeight0;
		public float LimbSolverWeight1;
		public float LimbSolverWeight2;
		public float LimbSolverWeight3;

		public float IKManagerWeight;
		
		public bool RagdollActive;
		public bool WheelJointEnabled;
	}
    
	private List<RewindTimeData> RewindList = new List<RewindTimeData>();
	#endregion

	private void Awake() => Ragdoll = GetComponent<Ragdoll>();

	private void Start()
	{
		Manager = TimeManager.Instance;
		Manager.TimeRewindRagdoll = this;
	}

	private void OnDestroy()
	{
		Manager.TimeRewindRagdoll = null;
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
		TimeData.RagdollActive = Ragdoll.RagdollActive;
		TimeData.WheelJointEnabled = Ragdoll.WheelJoint.enabled;
		
		// Save weights
		TimeData.LimbSolverWeight0 = Ragdoll.solvers[0].weight;
		TimeData.LimbSolverWeight1 = Ragdoll.solvers[1].weight;
		TimeData.LimbSolverWeight2 = Ragdoll.solvers[2].weight;
		TimeData.LimbSolverWeight3 = Ragdoll.solvers[3].weight;

		TimeData.IKManagerWeight = Ragdoll.IKManager.weight;

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
		Ragdoll.RagdollActive = TimeData.RagdollActive;
		Ragdoll.WheelJoint.enabled = TimeData.WheelJointEnabled;
		
		// Save weights
		Ragdoll.solvers[0].weight = TimeData.LimbSolverWeight0;
		Ragdoll.solvers[1].weight = TimeData.LimbSolverWeight1;
		Ragdoll.solvers[2].weight = TimeData.LimbSolverWeight2;
		Ragdoll.solvers[3].weight = TimeData.LimbSolverWeight3;

		Ragdoll.IKManager.weight = TimeData.IKManagerWeight;

		// Remove Last
		RewindList.RemoveAt(RewindList.Count - 1);
	}
}
