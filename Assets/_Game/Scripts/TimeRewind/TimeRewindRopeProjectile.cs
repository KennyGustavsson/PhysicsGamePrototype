using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindRopeProjectile : MonoBehaviour
{
    private Hook hook;
    private SpriteRenderer renderer;
    private CircleCollider2D collider2D;
    private Rigidbody2D Rigidbody;
    private TimeManager Manager;
    [NonSerialized] public bool IsRewindingTime = false;

#region TimeData
    private struct RewindTimeData
    {
        public Vector3 Pos;
        public Vector2 Vel;
        public float AngVel;
        public bool Simulated;
        public bool renderer;
        public bool collider2D;
    }
    
    private List<RewindTimeData> RewindList = new List<RewindTimeData>();
#endregion

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        hook = GetComponent<Hook>();
    }

    private void Start()
    {
        Manager = TimeManager.Instance;
        Manager.TimeRewindRopeProjectiles.Add(this);
    }

    private void OnDestroy()
    {
        Manager.TimeRewindRopeProjectiles.Remove(this);
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
        
        // Save rigidbody data
        TimeData.Vel = Rigidbody.velocity;
        TimeData.AngVel = Rigidbody.angularVelocity;
        TimeData.Simulated = Rigidbody.simulated;
        
        // Save Collider data
        TimeData.collider2D = collider2D.enabled;
        // Save Renderer data
        TimeData.renderer = renderer.enabled;
        
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

        // Rigidbody time data
        Rigidbody.velocity = TimeData.Vel;
        Rigidbody.angularVelocity = TimeData.AngVel;
        Rigidbody.simulated = TimeData.Simulated;
        
        // Save Collider data
        collider2D.enabled = TimeData.collider2D; 
        // Save Renderer data
        renderer.enabled = TimeData.renderer;
        
        // Remove Last
        RewindList.RemoveAt(RewindList.Count - 1);
    }

}
