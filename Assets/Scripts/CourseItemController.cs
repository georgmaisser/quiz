using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CourseItemController : MonoBehaviour {


	public Text courseIdText;
	public Text courseNameText;
	public Text courseEnrolledUserText;
	public Text courseNumberOfQuizzesText;


//	private string courseId;
//	private string courseName;
//	private string courseEnrolledUser;




	// Use this for initialization
	void Start () {
		
	}
	
	public void SetLabel(int courseId, string courseName, int courseEnrolledUser, int courseNumberOfQuizzes) {
		
		courseIdText.text = courseId.ToString();
		courseNameText.text = courseName;
		courseEnrolledUserText.text = courseEnrolledUser.ToString();
		courseNumberOfQuizzesText.text = courseNumberOfQuizzes.ToString();

	}



}
