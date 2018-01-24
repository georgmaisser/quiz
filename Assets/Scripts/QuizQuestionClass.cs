using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizQuestionClass {

	//given Information
	public string myQuestion;
	public string answerA;
	public string answerB;
	public string answerC;
	public string answerD;
	public string rightAnswer;
	public string language;
	public float timeToAnswer;

	//time stamps
	public float questionDisplayed;
	public float timeNeeded;


	//Information from User
	public int givenAnswer;

	//Konklusion
	public bool succeeded;



	public QuizQuestionClass() {

		myQuestion = "Do you really want to try?";
		answerA    = "Sure";
		answerB    = "Dunno";
		answerC    = "I dont think so";
		answerD    = "Bring it on";

		language = "en_US";

		succeeded = false;

		timeToAnswer = 0f;
		questionDisplayed = 0f;
		timeNeeded = 0f;

		rightAnswer = "?";
		
	}

//	void NewQuestion(string myQuestion, string myA, string myB, string myC, string myD, int myRight, string myLang) {
//		
//		myQuestion = myQuestion;
//		answerA    = myA;
//		answerB    = myB;
//		answerC    = myC;
//		answerD    = myD;
//
//		language = myLang;
//
//		succeeded = false;
//
//	}
		



}
