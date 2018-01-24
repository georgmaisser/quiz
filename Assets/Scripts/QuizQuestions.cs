using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class QuizQuestions {

	public List<QuizQuestionClass> myQuestions = new List<QuizQuestionClass>();



	public QuizQuestions() {
	
		myQuestions.Clear();
	
	}




	public void AddQuestion(string myQuestion, string myA, string myB, string myC, string myD, string myRight, string myLang) {
				
		QuizQuestionClass newQuestion = new QuizQuestionClass ();

			newQuestion.myQuestion = myQuestion;
			newQuestion.answerA    = myA;
			newQuestion.answerB    = myB;
			newQuestion.answerC    = myC;
			newQuestion.answerD    = myD;
		
			newQuestion.language = myLang;
			newQuestion.rightAnswer = myRight;
		
			newQuestion.succeeded = false;

		myQuestions.Add (newQuestion);
	}

	public void AddDummyQuestion() {

		QuizQuestionClass newQuestion = new QuizQuestionClass ();

		myQuestions.Add (newQuestion);
	
	}


	public void AddQuestionsRandomly(int numberOfQuestions) {
		string possibleAnswers = "ABCD";
		string rightAnswer;
		while (myQuestions.Count < numberOfQuestions) {
			
			rightAnswer = (string)possibleAnswers[Random.Range (0,4)].ToString();


			//Debug.Log (rightAnswer);
			AddQuestion (GenerateString(20), GenerateString(10), GenerateString(10), GenerateString(10), GenerateString(10), rightAnswer, "de_DE");
			
		}
	
	}


	public void printQuestions() {

		foreach (QuizQuestionClass question in myQuestions) {

			Debug.Log ("I have "+myQuestions.Count+" Questions");
			Debug.Log ("Question: "+question.myQuestion);
			Debug.Log ("A:"+question.answerA+" B:"+question.answerB+" C:"+question.answerC+" D:"+question.answerD);
			Debug.Log ("Language:"+question.language+" Success:"+question.succeeded);
		}

	}


	public string GenerateString(int LengthOfString) {
		string randomChars = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string myString = ""; 

		while (myString.Length < LengthOfString) {

			myString = myString + randomChars [Random.Range (0, randomChars.Length)];
		} 

		return myString;

	}


}
