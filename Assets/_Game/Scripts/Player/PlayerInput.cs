using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	private Unicycle Unicycle;
	private TimeManager TimeManager;

	private void Awake()
	{
		Unicycle = GetComponent<Unicycle>();
		TimeManager = GetComponent<TimeManager>();
	}

	private void Update()
	{
		Unicycle.Left = Input.GetKey(KeyCode.A);
		Unicycle.Right = Input.GetKey(KeyCode.D);
		Unicycle.LeanLeft = Input.GetKey(KeyCode.D);
		Unicycle.LeanRight = Input.GetKey(KeyCode.A);
		Unicycle.Crouching = Input.GetKey(KeyCode.LeftControl);
		TimeManager.RewindTimeInput = Input.GetKey(KeyCode.R);

		if(Input.GetKeyDown(KeyCode.Space)) KeySpace();
		if(Input.GetKeyDown(KeyCode.LeftShift)) Unicycle.Stop = true;
		if(Input.GetMouseButtonDown(0)) ButtonMouse0();
		if(Input.GetMouseButtonDown(1)) ButtonMouse1();
	}

	private void KeySpace()
	{
		Unicycle.Jump = true;
	}

	private void ButtonMouse0()
	{
		
	}

	private void ButtonMouse1()
	{
		
	}
}
