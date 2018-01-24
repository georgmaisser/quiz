using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodleCourse {


	public string courseName;
	public int courseId;
	public int enrolledUserCount;
	public int courseNumberOfQuizzes;

	public int progress;
	public float startDate;
	public float endDate;

	public List<MoodleQuiz> myQuizzes = new List<MoodleQuiz>();


	public MoodleCourse() {


		courseName = "TestKurs";
		courseId = 0;
		courseNumberOfQuizzes = 0;

		enrolledUserCount = 0;
		progress = 0;
		startDate = 0f;
		endDate = 0f;


	}

	public MoodleCourse(int id, string fullName, int userCount) {


		courseName = fullName;
		courseId = id;

		enrolledUserCount = userCount;
		progress = 0;
		startDate = 0f;
		endDate = 0f;


	}

	public void AddNewQuiz(int quizId, string quizName, string quizDescription, string preferedFeedbackBehaviour) {
	
		myQuizzes.Add (new MoodleQuiz (quizId, quizName, quizDescription, preferedFeedbackBehaviour));	
	
	
	}


	public void PrintListOfQuizzes() {
	
		foreach (MoodleQuiz quizItem in myQuizzes) {
		
			Debug.Log (quizItem.quizID+" "+quizItem.quizName+" "+quizItem.quizDescription);

		}
	
	
	
	}


}
