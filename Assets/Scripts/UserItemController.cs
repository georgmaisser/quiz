using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItemController : MonoBehaviour {

	public Text userIdText;
	public Text userUserNameText;
	public Text userFullNameText;

	// Use this for initialization
	void Start () {

	}

	public void SetLabel(int userId, string newUserName, string newFullNameText) {

		//userIdText.text = userId.ToString();
		userUserNameText.text = newUserName;
		//userFullNameText.text = newFullNameText;
		//quizNumberOfQuestions.text = quizNumberOfQuizzes.ToString();

	}

}