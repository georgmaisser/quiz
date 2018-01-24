using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class StopOverController : MonoBehaviour {


	public GameObject[] greenLightsPlayer1;
	public GameObject[] redLightsPlayer1;

	public GameObject[] greenLightsPlayer2;
	public GameObject[] redLightsPlayer2;



	// Use this for initialization
	void Start () {
		//SetLightsForPlayer1 ("1,0@4,1@5,2@6,0@8,1@0,2@7,0@2,1@3,2");
		//SetLightsForPlayer2 ("1,1@4,1@5,1@6,1@8,1@0,1@7,1@2,1@3,1");
		//1,0@4,1@5,2@6,0@8,1@0,2@7,0@2,1@3,2
		//1,1@4,1@5,1@6,1@8,1@0,1@7,1@2,1@3,1 red
		//1,2@4,2@5,2@6,2@8,2@0,2@7,2@2,2@3,2 green
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public async Task SetLights(string myrandomizedString, string herrandomizedString) {
		
		Debug.Log ("player one received list "+myrandomizedString);
		Debug.Log ("player two received list "+herrandomizedString);

		SetBackLights ();

		if (string.IsNullOrEmpty (myrandomizedString)) {
			myrandomizedString = "1,0@4,0@5,0@6,0@8,0@0,0@7,0@2,0@3,0";
		}

		if (string.IsNullOrEmpty (herrandomizedString)) {
			herrandomizedString = "1,0@4,0@5,0@6,0@8,0@0,0@7,0@2,0@3,0";
		}

		//Debug.Log ("player one after reset: "+myrandomizedString);
		//Debug.Log ("player two after reset: "+herrandomizedString);

		string[] myRandomString  = myrandomizedString.Split ('@');
		string[] herRandomString  = herrandomizedString.Split ('@');

		int i = 0;

		//Debug.Log (myRandomString.Length);
		//Debug.Log (herRandomString.Length);

		while (i < 9) {
			//Debug.Log (stringItem);
			if (int.Parse (myRandomString[i].Substring (myRandomString[i].Length - 1, 1))  == 2) {
			
				greenLightsPlayer1 [i].SetActive (true);
				redLightsPlayer1 [i].SetActive (false);
				//Debug.Log ("i set light green");
			} else if (int.Parse (myRandomString[i].Substring (myRandomString[i].Length - 1, 1)) == 1) {

				redLightsPlayer1 [i].SetActive (true);
				greenLightsPlayer1 [i].SetActive (false);
				//Debug.Log ("i set light red");
			}

			//Debug.Log (i);
			if (int.Parse (herRandomString[i].Substring (herRandomString[i].Length - 1, 1)) == 2) {

				greenLightsPlayer2 [i].SetActive (true);
				redLightsPlayer2 [i].SetActive (false);
				//Debug.Log ("i set light green");
			} else if (int.Parse (herRandomString[i].Substring (herRandomString[i].Length - 1, 1)) == 1) {

				redLightsPlayer2 [i].SetActive (true);
				greenLightsPlayer2 [i].SetActive (false);
				//Debug.Log ("i set light red");
			}

//			else {
//				redLightsPlayer1 [i].SetActive (false);
//				greenLightsPlayer1 [i].SetActive (false);
//			
//			}
//
			i++;
		}

		//Debug.Log ("finished light");
	}


	void SetBackLights() {
		int i = 0;
		foreach (GameObject lightItem in redLightsPlayer1) {
			lightItem.SetActive (false);
			redLightsPlayer2 [i].SetActive (false);
			greenLightsPlayer1 [i].SetActive (false);
			greenLightsPlayer2 [i].SetActive (false);
			i++;
		}
		//Debug.Log ("lights are set back");
	}



	public void SetLightsForPlayer2(string randomizedString) {
		Debug.Log ("player two received list "+randomizedString);



		if (string.IsNullOrEmpty (randomizedString)) {
			randomizedString = "1,0@4,0@5,0@6,0@8,0@0,0@7,0@2,0@3,0";
		}


		string[] randomString  = randomizedString.Split ('@');

		int i = 0;

		foreach (string stringItem in randomString) {
			//Debug.Log (stringItem);
			if (int.Parse (stringItem.Substring (stringItem.Length - 1, 1)) == 2) {

				greenLightsPlayer2 [i].SetActive (true);
				redLightsPlayer2 [i].SetActive (false);
				//Debug.Log ("i set light green");
			} else if (int.Parse (stringItem.Substring (stringItem.Length - 1, 1)) == 1) {

				redLightsPlayer2 [i].SetActive (true);
				greenLightsPlayer2 [i].SetActive (false);
				//Debug.Log ("i set light red");
			} else {
				redLightsPlayer2 [i].SetActive (false);
				greenLightsPlayer2 [i].SetActive (false);
			}

			i++;
		}


	}




}
