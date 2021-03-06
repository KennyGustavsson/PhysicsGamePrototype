using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static TimeManager Instance;
    public int MaxRewindFrames = 200;
    public int RewindTimeMultiplier = 2;
    [NonSerialized] public List<TimeRewind> TimeRewinds = new List<TimeRewind>();
    [NonSerialized] public List<TimeRewindJoint> TimeRewindJoints = new List<TimeRewindJoint>();
    [NonSerialized] public List<TimeRewindCollider> TimeRewindColliders = new List<TimeRewindCollider>();
    [NonSerialized] public List<TimeRewindTrans> TimeRewindTranses = new List<TimeRewindTrans>();
    [NonSerialized] public List<TimeRewindParticle> TimeRewindParticles = new List<TimeRewindParticle>();
	[NonSerialized] public List<TimeRewindDistanceJoint> TimeRewindDistanceJoint = new List<TimeRewindDistanceJoint>();
	[NonSerialized] public List<TimeRewindPlatform> TimeRewindPlatforms = new List<TimeRewindPlatform>();
	[NonSerialized] public List<TimeRewindPiston> TimeRewindPistons = new List<TimeRewindPiston>();
	[NonSerialized] public List<TimeRewindRopeProjectile> TimeRewindRopeProjectiles = new List<TimeRewindRopeProjectile>();
	[NonSerialized] public List<TimeRewindRope> TimeRewindRopes = new List<TimeRewindRope>();
	[NonSerialized] public TimeRewindUnicycle TimeRewindUnicycle;
    [NonSerialized] public TimeRewindRagdoll TimeRewindRagdoll;
	public event Action PermaDeadEvent;
    private Ragdoll Ragdoll;

	[NonSerialized] public int FrameCounter = 0;
	public int DeathFrameCounter = 0;
	public bool PermaDead = false;
	private bool DeathEventTriggered = false;
    
    public bool RewindingTime = false;
    public bool RewindTimeInput = false;
    private bool RewindingBoolSet = false;

    private void Awake()
    {
	    if (Instance) Destroy(this);
	    else Instance = this;

	    Ragdoll = transform.root.GetComponentInChildren<Ragdoll>();
    }

    private void FixedUpdate()
    {
	    DeathCheck();
	    
	    if(TimeRewinds.Count == 0) return;

	    if (RewindTimeInput && FrameCounter > 0) RewindingTime = true;
	    else RewindingTime = false;

	    if (!RewindingTime)
	    {
		    SaveData();
		    RewindingBoolSet = true;
		    
		    return;
	    }

	    RewindTime();
	    RewindingBoolSet = false;
    }

    private void DeathCheck()
    {
        if (TimeRewindRagdoll.RewindList.Count > 0)
        {
            if (TimeRewindRagdoll.RewindList[0].RagdollActive && !DeathEventTriggered)
            {
                PermaDeadEvent?.Invoke();
                DeathEventTriggered = true;
                PermaDead = true;
            }
        }
    }

    private void SaveData()
    {
	    foreach (var RewindObject in TimeRewinds)
	    {
		    if (!RewindingBoolSet)
			    RewindObject.IsRewindingTime = false;
		    
		    RewindObject.SaveRewind();
	    }

	    foreach (var Joint in TimeRewindJoints)
	    {
		    if (!RewindingBoolSet)
			    Joint.IsRewindingTime = false;
		    
		    Joint.SaveRewind();
	    }

	    foreach (var Collider in TimeRewindColliders)
	    {
		    if (!RewindingBoolSet)
			    Collider.IsRewindingTime = false;
		    
		    Collider.SaveRewind();
	    }
			    
	    foreach (var Trans in TimeRewindTranses)
	    {
		    if (!RewindingBoolSet)
			    Trans.IsRewindingTime = false;
		    
		    Trans.SaveRewind();
	    }

	    foreach (var ParticleSystem in TimeRewindParticles)
	    {
		    if (!RewindingBoolSet)
			    ParticleSystem.IsRewindingTime = false;
		    
		    ParticleSystem.SaveRewind();
	    }
			    
	    foreach (var Platform in TimeRewindPlatforms)
	    {
		    if (!RewindingBoolSet)
			    Platform.IsRewindingTime = false;
		    
		    Platform.SaveRewind();
	    }
			    
	    foreach (var DistanceJoint in TimeRewindDistanceJoint)
	    {
		    if (!RewindingBoolSet)
			    DistanceJoint.IsRewindingTime = false;
		    
		    DistanceJoint.SaveRewind();
	    }

	    foreach (var Piston in TimeRewindPistons)
	    {
		    if (!RewindingBoolSet)
			    Piston.IsRewindingTime = false;
		    
		    Piston.SaveRewind();
	    }
	    
	    foreach (var projectile in TimeRewindRopeProjectiles)
	    {
		    if (!RewindingBoolSet)
			    projectile.IsRewindingTime = false;
		    
		    projectile.SaveRewind();
	    }
	    
	    foreach (var rope in TimeRewindRopes)
	    {
		    if (!RewindingBoolSet)
			    rope.IsRewindingTime = false;
		    
		    rope.SaveRewind();
	    }
	    
	    
	    

	    if (TimeRewindUnicycle)
	    {
		    if (!RewindingBoolSet)
			    TimeRewindRagdoll.IsRewindingTime = false;
		    
		    TimeRewindUnicycle.SaveRewind();
	    }		    

	    if (TimeRewindRagdoll)
	    {
		    if (!RewindingBoolSet)
			    TimeRewindRagdoll.IsRewindingTime = false;
		    
		    TimeRewindRagdoll.SaveRewind();
	    }

	    FrameCounter = Mathf.Clamp(FrameCounter + 1, 0, MaxRewindFrames);
    }

    private void RewindTime()
    {
	    for (int i = 0; i < RewindTimeMultiplier; i++)
	    {
		    // Rewind time
		    foreach (var RewindObject in TimeRewinds)
		    {
			    RewindObject.IsRewindingTime = true;
			    RewindObject.RewindTime();
		    }

		    foreach (var Joint in TimeRewindJoints)
		    {
			    Joint.IsRewindingTime = true;
			    Joint.RewindTime();
		    }

		    foreach (var Collider in TimeRewindColliders)
		    {
			    Collider.IsRewindingTime = true;
			    Collider.RewindTime();
		    }
	    
		    foreach (var Trans in TimeRewindTranses)
		    {
			    Trans.IsRewindingTime = true;
			    Trans.RewindTime();
		    }
	    
		    foreach (var ParticleSystem in TimeRewindParticles)
		    {
			    ParticleSystem.IsRewindingTime = true;
			    ParticleSystem.RewindTime();
		    }
	    
		    foreach (var Platform in TimeRewindPlatforms)
		    {
			    Platform.IsRewindingTime = true;
			    Platform.RewindTime();
		    }

		    foreach (var DistanceJoint in TimeRewindDistanceJoint)
		    {
			    DistanceJoint.IsRewindingTime = true;
			    DistanceJoint.RewindTime();
		    }

		    foreach (var Piston in TimeRewindPistons)
		    {
			    Piston.IsRewindingTime = true;
			    Piston.RewindTime();
		    }
		    
		    foreach (var projectile in TimeRewindRopeProjectiles)
		    {
			    projectile.IsRewindingTime = true;
			    projectile.RewindTime();
		    }
		    
		    foreach (var rope in TimeRewindRopes)
		    {
			    rope.IsRewindingTime = true;
			    rope.RewindTime();
		    }
	    
		    if (TimeRewindUnicycle)
		    {
			    TimeRewindUnicycle.IsRewindingTime = true;
			    TimeRewindUnicycle.RewindTime();
		    }

		    if (TimeRewindRagdoll)
		    {
			    TimeRewindRagdoll.IsRewindingTime = true;
			    TimeRewindRagdoll.RewindTime();
		    }
	    
		    FrameCounter = Mathf.Clamp(FrameCounter - 1, 0, MaxRewindFrames);   
	    }
    }
}
