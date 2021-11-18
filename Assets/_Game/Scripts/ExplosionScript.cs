using System;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
	[Header("Explosion Effect Prefab")]
    [SerializeField] public GameObject Explosion;

	[Header("Explosion Options")]
    [SerializeField] private float ExplosionRadius = 10.0f;
    [SerializeField] private float ExplosionForce = 100.0f;
    [SerializeField] private float BleedingEffectMultiplier = 0.33f;
	[SerializeField] private bool OnlyOnPlayer = false;

    [Header("Debugging")]
    [SerializeField] private bool VisualizeExplosionSphere = true;
    [SerializeField] private int CircleResolution = 32;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
	    if (VisualizeExplosionSphere)
	    {
		    Vector3[] points = new Vector3[CircleResolution];
		    float DeltaTheta = 6.28318530718f / CircleResolution;
		    float Theta = 0.0f;

		    Vector2 Pos = transform.position;
		    
		    for (int i = 0; i < CircleResolution; i++)
		    {
				points[i] = new Vector3(Pos.x + (ExplosionRadius * Mathf.Cos(Theta)), Pos.y + (ExplosionRadius * Mathf.Sin(Theta)));
				Theta += DeltaTheta;
		    }

		    for (int i = 0; i < CircleResolution; i++)
		    {
			    Gizmos.DrawLine(points[i], i == CircleResolution - 1 ? points[0] : points[i + 1]);
		    }
	    }
    }
#endif

	private void OnCollisionEnter2D(Collision2D other)
    {
	    // Rag dolling
	    if (other.transform.gameObject.layer == 6 || other.transform.gameObject.layer == 7)
	    {
		    Ragdoll rd = other.transform.root.GetComponentInChildren<Ragdoll>();
			if (!rd.RagdollActive)
			{
				rd.ToggleRagdoll(true);
			}

			if (other.transform.gameObject.layer == 6)
			{
				var impactComponents = other.transform.root.GetComponentsInChildren<ImpactDetecter>();
				foreach (var impactComps in impactComponents)
				{
					impactComps.Collision(ExplosionForce * BleedingEffectMultiplier);
				}
			}
			Explode();
		}

		if (OnlyOnPlayer) return;

		Explode();
    }

	private void Explode()
    {
		Vector2 Pos = transform.position;

		// Instantiate explosion
		Instantiate(Explosion, Pos, Quaternion.identity);
		this.gameObject.GetComponent<SpriteRenderer>().sprite = null;

		// Add force
		var Hits = Physics2D.CircleCastAll(Pos, ExplosionRadius, Vector2.up);

		foreach (var Hit in Hits)
		{
			Rigidbody2D rb = Hit.rigidbody;

			if (rb)
			{
				Vector2 Direction = (Hit.point - Pos).normalized;
				rb.AddForce(Direction * ExplosionForce, ForceMode2D.Impulse);
			}
		}

		Destroy(gameObject);
	}
}