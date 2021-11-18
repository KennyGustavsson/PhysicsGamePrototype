using UnityEngine;

public class ImpactDetecter : MonoBehaviour
{
    [SerializeField] private Ragdoll PlayerRagdoll;
    private ParticleSystem particleSystem;
    private float rateOverTime;
    private float inheritedVelocityMultiplier;
    [SerializeField] private float bloodVelocityMultiplier = 0.01f;
    [SerializeField] private bool isRagdollImpact;

    [Header("Debugging")]
    [SerializeField] private bool PrintOwnNameOnCollision;
    [SerializeField] private bool PrintOtherNameOnCollision;

    private void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        rateOverTime = particleSystem.emission.rateOverTime.constant;
        inheritedVelocityMultiplier = particleSystem.inheritVelocity.curveMultiplier;
    }

    private void Update()
    {
        if (!isRagdollImpact)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (PrintOwnNameOnCollision) Debug.Log(other.relativeVelocity.magnitude + " My name : " + this.gameObject.name);
        if (PrintOtherNameOnCollision) Debug.Log(other.relativeVelocity.magnitude + " Other name : " + other.gameObject.name);

        if (other.relativeVelocity.magnitude > PlayerRagdoll.CollisionForceToRagDoll)
        {
            Collision(other.relativeVelocity.magnitude);
        }
    }

    public void Collision(float collisionForce)
    {
        var emission = particleSystem.emission;
        emission.rateOverTime = rateOverTime * collisionForce;

        var inheritedVelocity = particleSystem.inheritVelocity;
        inheritedVelocity.curveMultiplier = inheritedVelocityMultiplier * collisionForce * bloodVelocityMultiplier;

        particleSystem.Play();

        if (!PlayerRagdoll.RagdollActive)
        {
            PlayerRagdoll.ToggleRagdoll(true);
        }
    }
}
