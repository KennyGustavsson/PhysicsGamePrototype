using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
	[Header("Explosion Effect Prefab")]
    [SerializeField] public GameObject Explosion;

	[Header("Explosion Options")]
    [SerializeField] private float ExplosionRadius = 10.0f;
    [SerializeField] private float ExplosionForce = 100.0f;
    [SerializeField] private float BleedingEffectMultiplier = 0.33f;

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
		}

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
			    rb.AddForceAtPosition(Vector2.one * ExplosionForce, Pos); 
		    }
	    }
        
	    Destroy(gameObject);
    }
}