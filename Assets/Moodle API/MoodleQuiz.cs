using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodleQuiz {


	public int quizID;
	public string quizName;
	public string quizDescription;
	public string preferedFeedbackBehaviour;

	public int attemptID;
	public int attemptUniqueId;
	public int numberOfQuestions;
	public int numberOfPages;

	public List<MoodleQuestion> myListOfQuestions = new List<MoodleQuestion>();


	public MoodleQuiz() {

	
	}

	public MoodleQuiz(int newId, string newName, string newDescription, string newPreferedFeedbackBehaviour) {

		quizID = newId;
		quizName = newName;
		quizDescription = newDescription;
		preferedFeedbackBehaviour = newPreferedFeedbackBehaviour;

		numberOfQuestions = 1;

	}

	public void AddQuestion(MoodleQuestion newQuestion) {

		myListOfQuestions.Add(newQuestion);


	}

	public int GetSuccessOfQuestionById(int questionId) {
		foreach (MoodleQuestion questionItem in myListOfQuestions) {
			if (questionItem.questionNr == questionId && questionItem.successValue) {
				return 1;
			}
		}
		return 0;
	}


	public void SetBackSuccessValues() {
		Debug.Log ("deleting success values");
		foreach (MoodleQuestion listItem in myListOfQuestions) {
			//Debug.Log ("nr "+listItem.questionNr+" successvalue "+listItem.successValue);
			listItem.successValue = false;
		
		}


	}



	public void PrintQuiz() {


		Debug.Log ("id: "+quizID+", nQ: "+numberOfQuestions+", nP: "+numberOfPages+", attemptId: "+attemptID+" pFB: "+preferedFeedbackBehaviour);
		foreach (MoodleQuestion questionItem in myListOfQuestions) {

			Debug.Log("Question: "+questionItem.questionNr+" "+questionItem.quizQuestion);

			foreach (string listItem in questionItem.quizAnswers) {

				Debug.Log ("Answer: "+listItem);
			
			}
		}

	}

}
