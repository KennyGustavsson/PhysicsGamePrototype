using UnityEngine;

public class SpikeBallForceApplyAtInterval : MonoBehaviour
{
	[Header("Options")]
	[SerializeField] private float TimerInterval = 2.0f;
	[SerializeField] private float Force = 50.0f;
	[SerializeField] private bool ChangeDirection = true;
	[SerializeField] private bool UseLocalDirection = true;

	private Rigidbody2D Rigidbody;
	
	private float Timer = 0.0f;
	private int Direction = 1;

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (Timer > TimerInterval)
		{
			if(UseLocalDirection) Rigidbody.AddForce(transform.right * (Direction * Force), ForceMode2D.Impulse);
			else Rigidbody.AddForce(Vector2.right * (Direction * Force), ForceMode2D.Impulse);

			if(ChangeDirection) Direction *= -1;
			Timer = 0.0f;
		}

		Timer += Time.deltaTime;
	}
}
