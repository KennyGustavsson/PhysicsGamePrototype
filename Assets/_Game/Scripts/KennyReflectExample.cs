using UnityEngine;

public class KennyReflectExample : MonoBehaviour{
	public float Mass = 1.0f;
	public float GravityScale = 1.0f;

	private Vector2 Velocity;

	private HingeJoint2D joint;

	// private void Start(){
	// 	AddImpulseForce(new Vector2(5.0f, 0));
	// }

	private void Update(){
		Velocity += Physics2D.gravity * (Time.deltaTime * Time.deltaTime * GravityScale);
		Velocity = CollisionCheck(Velocity, transform.position);

		Vector3 pos = new Vector3(transform.position.x + Velocity.x, transform.position.y + Velocity.y, transform.position.z);
		transform.position = pos;
	}

	private Vector3 CollisionCheck(Vector3 Vel, Vector3 Pos)
	{
		Vector3 Direction = Vel.normalized;
		float Length = Vel.magnitude;

		RaycastHit2D Hit = Physics2D.Raycast(Pos, Direction, Length);
		
		// If colliding
		if (Hit.collider)
		{
			// Reflection
			Vector3 Reflection = Reflect(Direction, Hit.normal);
			
			// Impact impulse
			float Impact = 0.5f * Mass * Mathf.Pow(Vel.magnitude, 2);
			Vel = (Reflection * Impact) / Mass; // New impulse when collide
		}

		return Vel;
	}
	
	private Vector3 Reflect(Vector3 InDirection, Vector3 InNormal)
	{
		return -2.0f * Vector3.Dot(InNormal, InDirection) * InNormal + InDirection;
	}

	public void AddImpulseForce(Vector2 ImpulseForce){
		Velocity += ImpulseForce / Mass;
	}

	public void AddAcceleration(Vector2 AccelerationForce){
		Velocity += AccelerationForce * Time.deltaTime;
	}
}
