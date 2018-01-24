using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizItemController : MonoBehaviour {

		public Text quizIdText;
		public Text quizNameText;
		public Text quizDescriptionText;
		public Text quizNumberOfQuestions;


		//	private string quizId;
		//	private string quizName;
		//	private string quizEnrolledUser;


		// Use this for initialization
		void Start () {

		}

	public void SetLabel(int quizId, string quizName, string quizDescription) {

		//quizIdText.text = quizId.ToString();
		quizNameText.text = quizName;
		quizDescriptionText.text = quizDescription;
		//quizNumberOfQuestions.text = quizNumberOfQuizzes.ToString();

		}



	}
