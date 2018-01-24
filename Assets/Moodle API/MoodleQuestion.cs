using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodleQuestion {


	public string quizQuestion;
	public int questionNr;
	public int pageNr;
	public string sequenceCheckValue;
	public string submitValue;

	public bool successValue;

	public int attemptId;
	public int attemptUniqueId;

	public int chosenQuestionId;

	public bool answered;



	//public List<NameValueSet> myNameValueSet = new List<NameValueSet> ();
	public List<string> quizAnswers = new List<string>();
	public List<string> quizExplanations = new List<string>();



	public MoodleQuestion() {

		answered = false;

	
	}

	public MoodleQuestion(string question) {

		quizQuestion = question;
		answered = false;
	}

	public void PrintQuestion() {

		Debug.Log("I print question");
		Debug.Log("q: "+quizQuestion+" scV: "+sequenceCheckValue+" sV: "+submitValue+" qNr:"+questionNr+" a: "+answered);
		foreach (string answerItem in quizAnswers) {
			Debug.Log(answerItem);
		}

	}


}
