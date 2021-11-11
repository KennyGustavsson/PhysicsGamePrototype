using UnityEngine;

public class ForceOnCollision : MonoBehaviour
{
	[Header("Options")]
	[SerializeField] private float AddedForce = 50.0f;
	[SerializeField] private float KillMagnitude = 1.0f;
	[SerializeField] private bool ScaleForceWithMagnitude = true;

	private Rigidbody2D Rigidbody;

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.rigidbody) return;
		
		// Rag dolling
		if (other.transform.gameObject.layer == 6 || other.transform.gameObject.layer == 7)
		{
			if (other.relativeVelocity.magnitude > KillMagnitude)
			{
				Ragdoll rd = other.transform.root.GetComponentInChildren<Ragdoll>();
				rd.ToggleRagdoll(true);	
			}
		}

		switch (ScaleForceWithMagnitude)
		{
			case true:
				other.rigidbody.AddForce(Rigidbody.velocity.normalized * AddedForce * other.relativeVelocity.magnitude, ForceMode2D.Impulse);
				break;
				
			case false:
				other.rigidbody.AddForce(Rigidbody.velocity.normalized * AddedForce, ForceMode2D.Impulse);
				break;
		}
	}
}
