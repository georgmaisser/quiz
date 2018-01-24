using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class OpenGameClass {

	public int gameId;
	public int courseId;
	public int status;

	public int questionNr = 0;
	public int randomQuestionNr = 0;


	public string link_Id;
	public int challengerId;
	public int subjectId;
	public int quizId;

	public string randomGameSequence = "";
	public string herRandomGameSequence = "";

	public int duelEntryId;
	public int dbHotEntryId;

	public bool singlePlayerMode;

	public int herTimeModified = 0;
	public int myTimeModified = 0;

	public string buttonName;


	public OpenGameClass() {


	}

	public OpenGameClass(int newChallengerId, int newSubjectId, int newQuizId, int newStatus, int newDuelEntryId, string newLinkId) {

		challengerId = newChallengerId;
		subjectId = newSubjectId;
		quizId = newQuizId;
		status = newStatus;
		duelEntryId = newDuelEntryId;
		link_Id = newLinkId;

	}

	public async Task UpdateInformationFromDuelDB(OpenGameClass newGame) {
//		Debug.Log ("try to update "+link_Id+" with "+newGame.link_Id);
		if (newGame != null && link_Id.Contains(newGame.link_Id)) {

			if (herTimeModified < newGame.herTimeModified) {
//				Debug.Log("i update her sequence");

				herRandomGameSequence = newGame.herRandomGameSequence;
			}
			if (myTimeModified < newGame.myTimeModified) {
//				Debug.Log("i update my sequence");

				randomGameSequence = newGame.randomGameSequence;
			}
			if (newGame.duelEntryId != 0) {
				duelEntryId = newGame.duelEntryId;
			}

			if (subjectId == 0) {
				//Debug.Log ("i update subject");
				subjectId = newGame.subjectId;
			}

			if (challengerId == 0) {
				//Debug.Log ("i update challenger");
				challengerId = newGame.challengerId;
			}

			//we upate only if status is 1
			if (newGame.status == 1) {
				status = newGame.status;
			}



		}
		else {
//			Debug.Log("linkID didn't match, I did nothing");

		}

	}

	public void UnloadEverything() {
		
		challengerId = 0;
		subjectId = 0;
		quizId = 0;
		status = 0;
		duelEntryId = 0;
		dbHotEntryId = 0;
		link_Id = "";
		courseId = 0;
		questionNr = 0;
		randomQuestionNr = 0;
		randomGameSequence = "";
		herRandomGameSequence = "";
		myTimeModified = 0;
		herTimeModified = 0;

	
	
	}



	public void PrintGame() {

		Debug.Log(" "+challengerId+" "+subjectId+" "+quizId+" "+status+" "+duelEntryId+" "+link_Id);
		Debug.Log (" "+randomQuestionNr+" "+questionNr);

		if (randomGameSequence.Length > 0) {
			Debug.Log ("my sequence " + randomGameSequence);
		}
		if (herRandomGameSequence.Length > 0) {
			Debug.Log ("her sequence " + herRandomGameSequence);
		}
	}


}
