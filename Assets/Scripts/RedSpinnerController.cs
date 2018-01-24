using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSpinnerController : MonoBehaviour {

	public float framesPerSecond = 20f;

	float x = 0f;
	float y = 0f;
	public float z = 0f;

	public int listener = 0;

	float timeStart;
	float timeLength;



	// Use this for initialization
	async void OnEnable () {
		timeStart = Time.time;
		//Debug.Log (timeStart);


	}

	public float LenghtOfActivity() {
		timeLength = Time.time - timeStart;
		//Debug.Log ("i return time "+timeLength);
		return timeLength;
	}

	
	// Update is called once per frame
	void Update () {
		
		ShowSpinner ();
		
	}

	public void AddListener() {
		
		listener++;
		//Debug.Log ("just raised listener to "+listener);
	
	}

	public void DeleteListener() {

		listener--;
		//Debug.Log ("just lowered listener to "+listener);
	}



	async void ShowSpinner() {
		//Debug.Log ("spinner is started");

		this.transform.Rotate (new Vector3 (x, y, 3f));

	}


}
