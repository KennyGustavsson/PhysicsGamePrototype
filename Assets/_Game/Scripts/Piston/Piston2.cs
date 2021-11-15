using UnityEngine;

public class Piston2 : MonoBehaviour
{
    [Header("Timings")]
    [SerializeField] private float UpTime = 0.1f;
    [SerializeField] private float DownTime = 1.0f;
    [SerializeField] private float WaitTime = 1.0f;

    [Header("Position")] 
    [SerializeField] private float MaxY = 2.0f;

    [Header("Force")] 
    [SerializeField] private float AddedForce = 50.0f;
    [SerializeField] private Vector2 ForceBoxOffset = Vector2.up;
    [SerializeField] private Vector2 ForceBoxExtend = Vector2.one;

    [Header("Debugging")] 
    [SerializeField] private bool ShowEffectingRadius = false;
    
    private float Accumulator = 0.0f;
    private float Timer = 0.0f;
    private float StartY = 0.0f;
    private bool isGoingUp = true;

    private void Awake()
    {
        StartY = transform.localPosition.y;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (ShowEffectingRadius){
            Vector2 pos = transform.localPosition;
            Gizmos.matrix = transform.parent.localToWorldMatrix;
            
            Vector2[] Box = new Vector2[]
            {
                new Vector2(ForceBoxExtend.x / 2, ForceBoxExtend.y / 2) + pos + ForceBoxOffset, 
                new Vector2(ForceBoxExtend.x / 2, -ForceBoxExtend.y / 2) + pos + ForceBoxOffset, 
                new Vector2(-ForceBoxExtend.x / 2, -ForceBoxExtend.y / 2) + pos + ForceBoxOffset, 
                new Vector2(-ForceBoxExtend.x / 2, ForceBoxExtend.y / 2) + pos + ForceBoxOffset, 
            };

            
            Gizmos.color = Color.red;
            for (int i = 0; i < Box.Length; i++)
            {
                if (i == Box.Length - 1)
                {
                    Gizmos.DrawLine(Box[i], Box[0]);
                }
                else
                {
                    Gizmos.DrawLine(Box[i], Box[i + 1]);
                }
            }
        }
            
    }
#endif
    
    private void Update()
    {
        if (Timer > WaitTime)
        {
            // Apply force
            if(Accumulator == 0) ApplyForce();
            
            Accumulator += isGoingUp ? Time.deltaTime / UpTime : -Time.deltaTime / DownTime;
            Accumulator = Mathf.Clamp(Accumulator, 0, 1);

            float Current = Mathf.Lerp(StartY, MaxY, Accumulator);
            Vector2 Pos = transform.localPosition;
            Pos.y = Current;
            transform.localPosition = Pos;
            
            if (Accumulator == 0 || Accumulator == 1)
            {
                Timer = 0.0f;
                isGoingUp = !isGoingUp;
            }
        }

        Timer += Time.deltaTime;
    }

    private void ApplyForce()
    {
        Vector2 Pos = transform.position;
        var Hits = Physics2D.BoxCastAll(Pos + ForceBoxOffset, ForceBoxExtend, transform.localRotation.eulerAngles.y, Vector2.up, ForceBoxExtend.y);
        
        foreach (var Hit in Hits)
        {
            if (Hit.transform.gameObject.layer == 7)
            {
                Rigidbody2D rb = Hit.rigidbody;

                if (rb)
                {
                    rb.AddForce(new Vector2(0, AddedForce), ForceMode2D.Impulse); 
                }    
            }
        }
    }
}
