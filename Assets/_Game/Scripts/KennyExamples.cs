using System.Collections.Generic;
using UnityEngine;

public class KennyExamples : MonoBehaviour{
	public float GravityTime = 2.0f;
	private float Timer = 0.0f;
	public int ListMaxCount = 100;
	private bool Rewinding = false;
	public bool GravityChanger = false;

	public struct RewindTimeData{
		public Quaternion rot;
		public Vector3 pos;
		public Vector3 scale;
		public Vector2 vel;
		public float angvel;
	}
	
	private List<RewindTimeData> RewindList = new List<RewindTimeData>();
	
	private void Update(){
		if (Input.GetKeyDown(KeyCode.A)){
			Rewinding = true;
		}

		if (Input.GetKeyUp(KeyCode.A)){
			Rewinding = false;
		}

		if (GravityChanger){
			if (Timer > GravityTime){
				Physics2D.gravity = -Physics2D.gravity;
			}
			
			Timer += Time.deltaTime;	
		}
	}

	private void FixedUpdate(){
		if (Rewinding){
			RewindTime();
			return;
		}
		
		RewindTimeData hej = new RewindTimeData();
		hej.pos = transform.position;
		hej.rot = transform.rotation;
		hej.scale = transform.localScale;
		hej.vel = GetComponent<Rigidbody2D>().velocity;
		hej.angvel = GetComponent<Rigidbody2D>().angularVelocity;

		RewindList.Add(hej);

		if (RewindList.Count > ListMaxCount){
			RewindList.RemoveAt(0);
		}
	}

	private void RewindTime(){
		if (RewindList.Count == 0){
			return;
		}
		
		RewindTimeData hej = RewindList[RewindList.Count - 1];

		transform.position = hej.pos;
		transform.rotation = hej.rot;
		transform.localScale = hej.scale;

		GetComponent<Rigidbody2D>().velocity = hej.vel;
		GetComponent<Rigidbody2D>().angularVelocity = hej.angvel;
		
		RewindList.RemoveAt(RewindList.Count - 1);
	}
}
