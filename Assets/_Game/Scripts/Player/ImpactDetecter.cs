using UnityEngine;

public class ImpactDetecter : MonoBehaviour
{
    [SerializeField] private Ragdoll PlayerRagdoll;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.relativeVelocity.magnitude);
        
        if (other.relativeVelocity.magnitude > PlayerRagdoll.CollisionForceToRagDoll)
        {
            PlayerRagdoll.ToggleRagdoll(true);
        }
    }
}
