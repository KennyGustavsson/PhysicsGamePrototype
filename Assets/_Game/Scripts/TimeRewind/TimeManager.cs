using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static TimeManager Instance;
    public int MaxRewindFrames = 200;
    [NonSerialized] public List<TimeRewind> TimeRewinds = new List<TimeRewind>();
    [NonSerialized] public List<TimeRewindJoint> TimeRewindJoints = new List<TimeRewindJoint>();
    [NonSerialized] public List<TimeRewindCollider> TimeRewindColliders = new List<TimeRewindCollider>();
    [NonSerialized] public List<TimeRewindTrans> TimeRewindTranses = new List<TimeRewindTrans>();
    [NonSerialized] public List<TimeRewindParticle> TimeRewindParticles = new List<TimeRewindParticle>();
    [NonSerialized] public List<TimeRewindDistanceJoint> TimeRewindDistanceJoint = new List<TimeRewindDistanceJoint>();
    [NonSerialized] public TimeRewindUnicycle TimeRewindUnicycle;
    [NonSerialized] public TimeRewindRagdoll TimeRewindRagdoll;

    private int FrameCounter = 0;
    
    public bool RewindingTime = false;
    public bool RewindTimeInput = false;
    private bool RewindingBoolSet = false;

    private void Awake()
    {
	    if (Instance) Destroy(this);
	    else Instance = this;
    }

    private void FixedUpdate()
    {
	    if(TimeRewinds.Count == 0) return;

	    if (RewindTimeInput && FrameCounter > 0) RewindingTime = true;
	    else RewindingTime = false;

	    if (!RewindingTime)
	    {
		    FrameCounter = Mathf.Clamp(FrameCounter + 1, 0, MaxRewindFrames);
		    
		    if (!RewindingBoolSet)
		    {
			    // Set rewind objects rewind bool
			    foreach (var RewindObject in TimeRewinds)
			    {
				    if(RewindObject)
						RewindObject.IsRewindingTime = false;
			    }

			    foreach (var Joint in TimeRewindJoints)
			    {
				    if (Joint)
					    Joint.IsRewindingTime = false;
			    }

			    foreach (var Collider in TimeRewindColliders)
			    {
				    if (Collider)
					    Collider.IsRewindingTime = false;
			    }
			    
			    foreach (var Trans in TimeRewindTranses)
			    {
				    if (Trans)
					    Trans.IsRewindingTime = false;
			    }

			    foreach (var ParticleSystem in TimeRewindParticles)
			    {
				    if (ParticleSystem)
					    ParticleSystem.IsRewindingTime = false;
			    }
			    
			    if(TimeRewindUnicycle)
				    TimeRewindUnicycle.IsRewindingTime = false;
				    
			    if(TimeRewindRagdoll) 
				    TimeRewindRagdoll.IsRewindingTime = false;
		    }

		    RewindingBoolSet = true;
		    return;
	    }
	    
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

	    RewindingBoolSet = false;
	    
	    FrameCounter = Mathf.Clamp(FrameCounter - 1, 0, MaxRewindFrames);
    }
}
