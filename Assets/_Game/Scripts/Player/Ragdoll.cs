using UnityEngine;
using UnityEngine.U2D.IK;

public class Ragdoll : MonoBehaviour{
    public bool Ragooooool;
    
    public float CollisionForceToRagDoll;
    [SerializeField] private Unicycle UniCycle;
    [SerializeField] private Rigidbody2D CharacterRigidBody;
    [SerializeField] private WheelJoint2D WheelJoint;
    [SerializeField] private IKManager2D IKManager;
    [SerializeField] private LimbSolver2D[] solvers;
    
    [SerializeField] private Collider2D[] activeColliders;
    [SerializeField] private Rigidbody2D[] activeRigidbodies;
    
    [SerializeField] private Collider2D[] colliders;
    [SerializeField] private HingeJoint2D[] joints;
    [SerializeField] private Rigidbody2D[] rigidbodies;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>();
        joints = GetComponentsInChildren<HingeJoint2D>();
        rigidbodies = GetComponentsInChildren<Rigidbody2D>();
    }

    private void Start() => ToggleRagdoll(false);

    private void Update(){
        if (Ragooooool){
            ToggleRagdoll(true);
            Ragooooool = false;
        }
    }

    public void ToggleRagdoll(bool enableRagdoll)
    {
        IKManager.weight = enableRagdoll ? 0 : 1;
        WheelJoint.enabled = !enableRagdoll;
        CharacterRigidBody.simulated = !enableRagdoll;
        
        if (UniCycle)
        {
            UniCycle.RagDolling = enableRagdoll;
        }

        foreach (var rb in rigidbodies)
        {
            rb.velocity = enableRagdoll ? rb.velocity.normalized : rb.velocity;
            rb.angularVelocity = enableRagdoll ? Mathf.Clamp(rb.angularVelocity, 0.0f, 1.0f) : rb.angularVelocity;
            
            rb.simulated = enableRagdoll;
        }

        foreach (var rb in activeRigidbodies)
        {
            rb.velocity = enableRagdoll ? rb.velocity : Vector2.zero;
            rb.angularVelocity = enableRagdoll ? rb.angularVelocity : 0;
            
            rb.simulated = !enableRagdoll;
        }
        
        foreach (var solver in solvers)
        {
            solver.weight = enableRagdoll ? 0 : 1;
        }
        
        foreach (var col in activeColliders)
        {
            col.enabled = !enableRagdoll;
        }

        foreach(var col in colliders)
        {
            col.enabled = enableRagdoll;
        }

        foreach (var joint in joints)
        {
            joint.enabled = enableRagdoll;
        }
    }
}
