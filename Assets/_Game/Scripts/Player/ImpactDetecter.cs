using UnityEngine;

public class ImpactDetecter : MonoBehaviour
{
    [SerializeField] private Ragdoll PlayerRagdoll;

    private void Update()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {              
        if (other.relativeVelocity.magnitude > PlayerRagdoll.CollisionForceToRagDoll)
        {
            PlayerRagdoll.ToggleRagdoll(true);
        }
    }
}
