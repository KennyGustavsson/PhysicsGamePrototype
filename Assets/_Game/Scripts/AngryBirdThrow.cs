using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AngryBirdThrow : MonoBehaviour{
	[Header("Options")]
	public float MaxForce = 10.0f;
	
	private Rigidbody2D Rigidbody2D;
	private LineRenderer LineRenderer;
	private Vector3 MouseStartPos;
	private Vector3 MouseEndPos;
	private Vector3 CurrentMousePos; // Visualizer

	private float zVal = 10;

	private void Awake(){
		Rigidbody2D = GetComponent<Rigidbody2D>();
		LineRenderer = GetComponent<LineRenderer>();
		
		LineRenderer.SetPosition(0, Vector3.zero);
		LineRenderer.SetPosition(1, Vector3.zero);
	}

	private void Update(){
		if (Input.GetMouseButtonDown(0)){
			StartThrow();
		}

		if (Input.GetMouseButtonUp(0)){
			EndThrow();
		}

		CurrentMousePos = Input.mousePosition;
		
		if (MouseStartPos == Vector3.zero){
			return;
		}

		Vector3 CurrentWorldPos = Camera.main.ScreenToWorldPoint(CurrentMousePos);
		CurrentWorldPos.z = zVal;

		LineRenderer.SetPosition(1, CurrentWorldPos);
	}

	private void StartThrow(){
		MouseStartPos = Input.mousePosition;
		Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MouseStartPos);
		WorldPos.z = zVal;
		
		LineRenderer.SetPosition(0, WorldPos);
	}

	private void EndThrow(){
		MouseEndPos = Input.mousePosition;

		float Strength = Vector3.Distance(MouseStartPos, MouseEndPos);
		Vector2 Direction = (MouseEndPos - MouseStartPos).normalized;

		Strength = Mathf.Clamp(Strength, 0.0f, MaxForce);
		Rigidbody2D.AddForce(-Direction * Strength, ForceMode2D.Impulse);
		
		Reset();
	}

	private void Reset(){
		MouseStartPos = Vector3.zero;
		MouseEndPos = Vector3.zero;
		
		LineRenderer.SetPosition(0, Vector3.zero);
		LineRenderer.SetPosition(1, Vector3.zero);
	}
}
