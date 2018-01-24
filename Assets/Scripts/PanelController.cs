using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {


	public Text userNameText;



	// Use this for initialization
	public void SetText (string userName) {
	
		userNameText.text = userName;


	}



}
