using UnityEngine;

public class ForceOnCollision : MonoBehaviour
{
	[Header("Options")]
	[SerializeField] private float AddedForce = 50.0f;
	[SerializeField] private float KillMagnitude = 1.0f;
    [SerializeField] private float EffectingRadius = 1.0f;
    [SerializeField] private float NonVelocityBloodValue = 1000f;
	[SerializeField] private bool ScaleForceWithMagnitude = true;
	[SerializeField] private bool OnlyOnPlayer = true;

    [Header("Debugging")] 
    [SerializeField] private bool ShowEffectingRadius = false;

	private float ForceApplyCooldown = 1f;
	private float Timer = 0.0f;
	private bool IsCoolingDown;

	private Rigidbody2D Rigidbody;

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        if (Timer > ForceApplyCooldown)
        {
			IsCoolingDown = false;
			Timer = 0.0f;
        }

		Timer += Time.deltaTime;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {   
        if(ShowEffectingRadius)
            Gizmos.DrawSphere(transform.position, EffectingRadius);
    }
#endif
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.rigidbody) return;

        // Rag dolling
        if (other.transform.gameObject.layer == 6 || other.transform.gameObject.layer == 7)
        {
            if (!IsCoolingDown)
            {
                if (other.relativeVelocity.magnitude > KillMagnitude)
                {
                    Ragdoll rd = other.transform.root.GetComponentInChildren<Ragdoll>();
                    
                    if(rd.IsRewinding) return;
                    
                    if (!rd.RagdollActive)
                    {
                        rd.ToggleRagdoll(true);
                    }

                    if (other.transform.gameObject.layer == 6)
                    {
                        var impactComponents = other.transform.root.GetComponentsInChildren<ImpactDetecter>();

                        if (Rigidbody)
                        {
                            foreach (var impactComps in impactComponents)
                            {
                                impactComps.Collision(Rigidbody.velocity.magnitude);
                            }                            
                        }
                        else
                        {
                            foreach (var impactComps in impactComponents)
                            {
                                impactComps.Collision(NonVelocityBloodValue);
                            }
                        }
                    }
                }

                if (OnlyOnPlayer)
                {
                    switch (ScaleForceWithMagnitude)
                    {
                        case true:
                            AddForceScaledMagnitude(other);
                            break;

                        case false:
                            AddForce(other);
                            break;
                    }                    
                }
            }
        }

		if(OnlyOnPlayer)
        {
            IsCoolingDown = true;
            return;
        }

        if (!IsCoolingDown)
        {
            switch (ScaleForceWithMagnitude)
            {
                case true:
                    AddForceScaledMagnitude(other);
                    break;

                case false:
                    AddForce(other);
                    break;
            }
        }

        IsCoolingDown = true;
    }

    private void AddForceScaledMagnitude(Collision2D other)
    {
        var Hits = Physics2D.CircleCastAll(other.GetContact(0).point, EffectingRadius, Vector2.up);

        foreach (var Hit in Hits)
        {
            if (Hit.transform.gameObject != this)
            {
                Rigidbody2D rb = Hit.rigidbody;

                if (rb)
                {
                    rb.AddForceAtPosition(Vector2.one * EffectingRadius, other.GetContact(0).point);

                    Vector2 Direction = (Hit.point - other.GetContact(0).point).normalized;
                    rb.AddForce(Rigidbody.velocity.normalized * AddedForce * other.relativeVelocity.magnitude, ForceMode2D.Impulse); 
                }   
            }
        }
    }

    private void AddForce(Collision2D other)
    {
        var Hits = Physics2D.CircleCastAll(other.GetContact(0).point, EffectingRadius, Vector2.up);

        foreach (var Hit in Hits)
        {
            if (Hit.transform.gameObject != this)
            {
                Rigidbody2D rb = Hit.rigidbody;

                if (rb)
                {
                    rb.AddForceAtPosition(Vector2.one * EffectingRadius, other.GetContact(0).point);

                    Vector2 Direction = (Hit.point - other.GetContact(0).point).normalized;
                    rb.AddForce(Rigidbody.velocity.normalized * AddedForce, ForceMode2D.Impulse); 
                }   
            }
        }
    }
}
