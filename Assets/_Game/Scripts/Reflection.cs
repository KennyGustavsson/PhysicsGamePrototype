using UnityEngine;

[ExecuteAlways]
public class Reflection : MonoBehaviour
{
	[Header("Uses gizmos so you can see it directly in editor")]
	[Header("Simulation Options")]
	[Range(1, 100)] public int MaxSegmentCount = 300;
	[Range(0, 1)] public float BallSimulation = 0.0f;
	public float SimulationTime = 0.0f; 
	
	[Header("Ball Options")]
	public Vector3 ImpulseForce = Vector3.zero;
	public float Mass = 10.0f;
	
	private Vector3[] ballSegments;
	private int numBallSegments = 0;
	
	private void OnDrawGizmos()
	{
		// Just to get a understanding for the time.
		SimulationTime = MaxSegmentCount * Time.fixedDeltaTime;
        
		SimulateBallPath();
		SimulateBall();
		
#if UNITY_EDITOR
		// Ensure continuous Update calls.
		if (!Application.isPlaying)
		{
			UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
			UnityEditor.SceneView.RepaintAll();
		}
#endif
	}
	
	private void SimulateBallPath()
	{
		float timestep = Time.fixedDeltaTime; // Time step is the default fixed delta time that Unity uses for physics.

		Vector3 velocity = ImpulseForce / Mass;
		Vector3 gravity = Physics.gravity * timestep * timestep;
		Vector3 position = Vector3.zero;
 
		if (ballSegments == null || ballSegments.Length != MaxSegmentCount)
		{
			ballSegments = new Vector3[MaxSegmentCount];
		}
		int numSegments = 0;
 
		ballSegments[0] = position;
		numSegments++;
 
		for (int i = 1; i < MaxSegmentCount; i++)
		{
			velocity += gravity;
			velocity = CollisionCheck(velocity, position);
			position += velocity;
 
			ballSegments[i] = position;
			numSegments++;
		}
 
		for (int i = 0; i < numSegments - 1; i++)
		{
			Gizmos.DrawLine(ballSegments[i], ballSegments[i + 1]);
		}
	}
	
	private Vector3 CollisionCheck(Vector2 Vel, Vector2 Pos)
	{
		
		Vector2 Direction = Vel.normalized;
		float Length = Vel.magnitude;

		RaycastHit2D Hit = Physics2D.Raycast(Pos, Direction, Length);
		
		// If colliding
		if (Hit.collider != null)
		{
			// Reflection
			Vector3 Reflection = Reflect(Direction, Hit.normal);
			
			// Impact impulse
			float Impact = 0.5f * Mass * Mathf.Pow(Vel.magnitude, 2);
			Vel = (Reflection * Impact) / Mass; // New impulse when collide
		}

		return Vel;
	}
	
	public Vector3 Reflect(Vector3 InDirection, Vector3 InNormal)
	{
		return -2.0f * Vector3.Dot(InNormal, InDirection) * InNormal + InDirection;
	}
	
	private void SimulateBall()
	{
		int i = Mathf.RoundToInt(BallSimulation * (ballSegments.Length - 2));
		float t = BallSimulation;

		Vector3 position = Vector3.Lerp(ballSegments[i], ballSegments[i + 1], t);
		Gizmos.DrawSphere(position, 0.1f);
	}
}
