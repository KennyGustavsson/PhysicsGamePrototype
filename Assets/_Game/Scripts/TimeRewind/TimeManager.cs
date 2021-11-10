using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	// TODO make time rewind work with Ragdoll and Unicycle
	
    public static TimeManager Instance;
    public int MaxRewindFrames = 200;
    [NonSerialized] public List<TimeRewind> TimeRewinds = new List<TimeRewind>();
    [NonSerialized] public List<TimeRewindJoint> TimeRewindJoints = new List<TimeRewindJoint>();
    [NonSerialized] public List<TimeRewindCollider> TimeRewindColliders = new List<TimeRewindCollider>();
    [NonSerialized] public TimeRewindUnicycle TimeRewindUnicycle;
    [NonSerialized] public TimeRewindRagdoll TimeRewindRagdoll;
    
    public bool RewindingTime = false;
    private bool RewindingBoolSet = false;

    private void Awake()
    {
	    if (Instance) Destroy(this);
	    else Instance = this;
    }

    private void FixedUpdate()
    {
	    if(TimeRewinds.Count == 0) return;
	    
	    if (!RewindingTime)
	    {
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
    }

    /// <summary>
    /// Sets if currently should rewind time
    /// </summary>
    /// <param name="Active"></param>
    public void SetTimeRewind(bool Active)
    {
	    RewindingTime = Active;
    }
}
