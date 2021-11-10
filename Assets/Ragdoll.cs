using UnityEngine;
using UnityEngine.U2D.IK;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private float CollisionForceToRagDoll;
    [SerializeField] private Unicycle UniCycle;
    
    [SerializeField] private Collider2D[] colliders;
    [SerializeField] private HingeJoint2D[] joints;
    [SerializeField] private Rigidbody2D[] rigidbodies;
    [SerializeField] private LimbSolver2D[] solvers;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>();
        joints = GetComponentsInChildren<HingeJoint2D>();
        rigidbodies = GetComponentsInChildren<Rigidbody2D>();
        solvers = GetComponentsInChildren<LimbSolver2D>();
    }

    private void Start() => ToggleRagdoll(false);

    public void ToggleRagdoll(bool enableRagdoll)
    {
        if (UniCycle)
        {
            UniCycle.RagDolling = enableRagdoll;
        }
        
        foreach(var col in colliders)
        {
            if (col == this.GetComponent<Collider2D>()) continue;
            col.enabled = enableRagdoll;
        }

        foreach (var joint in joints)
        {
            if (joint == this.GetComponent<HingeJoint2D>()) continue;
            joint.enabled = enableRagdoll;
        }

        foreach (var rb in rigidbodies)
        {
            if (rb == this.GetComponent<Rigidbody2D>()) continue;
            rb.simulated = enableRagdoll;
        }

        foreach (var solver in solvers)
        {
            solver.weight = enableRagdoll ? 0 : 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.magnitude > CollisionForceToRagDoll){
            ToggleRagdoll(true);
        }
    }
}
