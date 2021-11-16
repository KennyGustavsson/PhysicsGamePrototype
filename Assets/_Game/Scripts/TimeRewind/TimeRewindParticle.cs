using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindParticle : MonoBehaviour
{
	private struct RewindTimeData
	{
		public ParticleSystem.Particle[] PArray;
	}
	
	[NonSerialized] public bool IsRewindingTime = false;
	
	private List<RewindTimeData> RewindList = new List<RewindTimeData>();
	private ParticleSystem ParticleSystem;
	private TimeManager Manager;
	
	private void Awake()
	{
		ParticleSystem = GetComponent<ParticleSystem>();
	}

	private void Start()
	{
		Manager = TimeManager.Instance;
		Manager.TimeRewindParticles.Add(this);
	}

	private void OnDestroy()
	{
		Manager.TimeRewindParticles.Remove(this);
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

        // Save trans data
        TimeData.PArray = new ParticleSystem.Particle[ParticleSystem.particleCount];
        ParticleSystem.GetParticles(TimeData.PArray);
        
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
        ParticleSystem.SetParticles(TimeData.PArray);
        
        // Remove Last
        RewindList.RemoveAt(RewindList.Count - 1);
    }
}
