using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DataController : MonoBehaviour {


	public List<MoodleCourse> myListofCourses = new List<MoodleCourse>();
	public List<MoodleDatabase> myListofDatabases = new List<MoodleDatabase>();

	public List<OpenGameClass> myListofGames = new List<OpenGameClass>();

	public List<PairOfInts> myListOfRandomQuestions = new List<PairOfInts> ();

	public int activeCourseId = 0;
	public int activeQuizId = 0;
	//public int myActiveGame.questionNr = 0;

	//public int myActiveGame.randomQuestionNr = 0;

	public int activeDbHostEntryId;
	public int activeDuelDBEntryId;

	// this will go to preferences:
	public int randomQuizSize = 9;
	public int packageSize = 3;

	public OpenGameClass myActiveGame;

	public MoodleQuestion activeQuestion;











	void Start() {

		CheckGameSequence ();




	}



 	void Update() {


	}

	async void CheckGameSequence() {
		int i = 0;
		while (i < 100) {
		
			if ((myActiveGame != null) && (myActiveGame.randomGameSequence.Length > 27)) {
				await new WaitForSeconds (0.5f);
				Debug.Log("raondom Game Sequence to long!");
				Debug.Log (myActiveGame.randomGameSequence);
			}
			i++;
		}
	
	
	}



	public async Task AddMoodleCourse(int id, string fullName, int enrolledUserCount) {

		myListofCourses.Add(new MoodleCourse(id, fullName, enrolledUserCount));
	}


	public async Task AddDataToLastCourseInList(int id, string fullName, int enrolledUserCount) {


		myListofCourses.Add(new MoodleCourse(id, fullName, enrolledUserCount));



	}

	public MoodleCourse MyLastCourseInList() {

		return myListofCourses[myListofCourses.Count-1];
	}

	public async Task CountQuizzesForLastCourseInList() {

		MyLastCourseInList().courseNumberOfQuizzes = MyLastCourseInList().myQuizzes.Count;


	}




	public async Task AddNewQuizToCourseById(int courseId, int quizId, string quizName, string quizDescription, string preferedFeedbackBehaviour) {

		foreach (MoodleCourse courseItem in myListofCourses) {
		
			if (courseItem.courseId == courseId) {
			
				courseItem.AddNewQuiz (quizId, quizName, quizDescription, preferedFeedbackBehaviour);
				break;
			}
		
		
		}

	}



	public MoodleCourse GetCourseByID(int courseId) {


		foreach (MoodleCourse courseItem in myListofCourses) {
			
			if (courseItem.courseId == courseId) {
				return courseItem;
				
			}
		
		}

		return null;
	}

	public int GetCourseIdByQuizId(int quizId) {
	
		foreach (MoodleCourse courseItem in myListofCourses) {

			foreach (MoodleQuiz quizItem in courseItem.myQuizzes) {
			
				if (quizItem.quizID == quizId) {
					return courseItem.courseId;
				}
			
			}
		
		}
	
		return 0;
	}




	public MoodleQuiz GetQuizByCourseByID(int courseId, int quizId) {


		foreach (MoodleCourse courseItem in myListofCourses) {

			if (courseItem.courseId == courseId) {

				foreach (MoodleQuiz quizItem in courseItem.myQuizzes) {
				
					if (quizItem.quizID == quizId) {
					
						return quizItem;
					}
				
				}


			}

		}

		return null;
	}

	public MoodleQuiz GetActiveQuiz() {
		return myListofCourses [activeCourseId].myQuizzes [activeQuizId];
	}

	public int GetActiveQuizId() {
		return myListofCourses [activeCourseId].myQuizzes [activeQuizId].quizID;
	}



	public async Task StartQuizbyCourseAndId(int courseId, int quizId) {
	
		Debug.Log("my "+myActiveGame.randomGameSequence+" her "+myActiveGame.herRandomGameSequence);

		foreach (MoodleCourse courseItem in myListofCourses) {

			if (courseItem.courseId == courseId) {

				activeCourseId = myListofCourses.IndexOf (courseItem);
				Debug.Log ("active course is "+courseItem.courseName+" with id "+courseItem.courseId);

				foreach (MoodleQuiz quizItem in courseItem.myQuizzes) {

					if (quizItem.quizID == quizId) {

						activeQuizId = courseItem.myQuizzes.IndexOf (quizItem);

					}
				}

			}

		}

		//set back successvalue

		myActiveGame.courseId = activeCourseId;
		myActiveGame.quizId = activeQuizId;


		//myListofCourses [activeCourseId].myQuizzes [activeQuizId].SetBackSuccessValues ();

	}



	//active course and quiz are set in StartQuizbyCourseAndId
	public MoodleQuestion ActiveQuestion(bool raise) {

		myActiveGame.questionNr = myListOfRandomQuestions [myActiveGame.randomQuestionNr].a;

		//Debug.Log ("i enter active question with "+myActiveGame.randomQuestionNr+" AQId: "+myActiveGame.questionNr);
		//Debug.Log ("course: "+activeCourseId+" quiz: "+ activeQuizId+" question: "+myActiveGame.questionNr);

		//Debug.Log ("number of questions in course"+myListofCourses[activeCourseId].myQuizzes.Count);

		//every time I retrieve active quesiton, I increase the counter

		if (raise) {
			Debug.Log ("i raise ");
			RaiseActiveQuestion ();
		}
		Debug.Log ("i return active question with "+myActiveGame.randomQuestionNr+" AQId: "+myActiveGame.questionNr);
		return myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions [myActiveGame.questionNr];


	}

	public async Task AddChosenAnswerToQuestion(int buttonId) {

		myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions [myActiveGame.questionNr].chosenQuestionId = buttonId;
	
	}

	public bool RaiseActiveQuestion() {

		myActiveGame.randomQuestionNr++;

		//Debug.Log ("should be 9: "+myListOfRandomQuestions.Count);
		Debug.Log ("randomQuestionNr Counter is "+myActiveGame.randomQuestionNr);

		if (myListOfRandomQuestions.Count > myActiveGame.randomQuestionNr) {
			myActiveGame.questionNr = myListOfRandomQuestions [myActiveGame.randomQuestionNr].a;
			Debug.Log ("active question id after random: " + myActiveGame.questionNr);
			return true;
		} else {
			return false;
		}
	}


	public async Task PrintActiveQuestion(){
		MoodleQuestion printQuestion = myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions [myActiveGame.questionNr];

		Debug.Log (printQuestion.quizQuestion);
		foreach (string answerItem in printQuestion.quizAnswers) {
			Debug.Log (answerItem);
		}

	}

	public int GetDBId(string dbName) {
		foreach (MoodleDatabase dbItem in myListofDatabases) {
			if (dbItem.databaseName.ToString().Contains(dbName) && dbItem.courseId == myListofCourses[activeCourseId].courseId) {
				Debug.Log ("i return "+dbItem.databaseName+" of course id "+dbItem.courseId+" and id "+dbItem.databaseId);
				return dbItem.databaseId;
			}
		}
		return 0;
	}


	public int GetDBIdByCourseId(string dbName, int courseId) {
		foreach (MoodleDatabase dbItem in myListofDatabases) {
			if (dbItem.databaseName.ToString().Contains(dbName) && dbItem.courseId == courseId) {
				Debug.Log ("i return "+dbItem.databaseName+" of course id "+dbItem.courseId+" and id "+dbItem.databaseId);
				return dbItem.databaseId;
			}
		}
		return 0;
	}


	public MoodleDatabase GetDbByName(string dbName) {
		foreach (MoodleDatabase dbItem in myListofDatabases) {
			if (dbItem.databaseName.ToString ().Contains (dbName) && dbItem.courseId == myListofCourses [activeCourseId].courseId) {
				Debug.Log ("i return "+dbItem.databaseName+" of course id "+dbItem.courseId+" and id "+dbItem.databaseId);
				return dbItem;
			}
		}
		return null;
	}

	public MoodleDatabase GetDbByNameAndByCourseId(string dbName, int courseId) {
		foreach (MoodleDatabase dbItem in myListofDatabases) {
			if (dbItem.databaseName.ToString ().Contains (dbName) && dbItem.courseId == courseId) {
				Debug.Log ("i return "+dbItem.databaseName+" of course id "+dbItem.courseId+" and id "+dbItem.databaseId);
				return dbItem;
			}
		}
		return null;
	}

	public void PrintListOfDB() {
		foreach (MoodleDatabase dbItem in myListofDatabases) {
			dbItem.PrintDatabase (false);
		}
	}



	public MoodleDbRecord GetRecordZeorOfDB(string dbName) {
		Debug.Log ("i am in record getting");
		//MoodleDbRecord newRecord;
		foreach (MoodleDatabase dbItem in myListofDatabases) {
			Debug.Log ("i run through list of databases "+dbItem.databaseName+" courseid"+dbItem.courseId+"="+myListofCourses[activeCourseId].courseId);
			if (dbItem.databaseName.ToString().Contains(dbName)) {
				Debug.Log ("found database");
				return dbItem.dBRecords[0];
			}
		}
		Debug.Log ("didn't find the multiplyer database for chosen quiz");
		return null;
	}



	public async Task AddRecordToDatabase(
		int dbId, int entryId,
		string data0Name, string data0Value,
		string data1Name, string data1Value,
		string data2Name, string data2Value,
		string data3Name, string data3Value,
		string data4Name, string data4Value,
		string data5Name, string data5Value)
		{
		
		foreach (MoodleDatabase dbItem in myListofDatabases) {
		
			if (dbItem.databaseId == dbId) {
			
				dbItem.dBRecords.Add (new MoodleDbRecord ());
			
			}
		
		
		}
		
		
		

	}



	public async Task SaveInformationToQuiz(int courseId, int quizId, int attemptId, int attemptUniqueId,int numberOfQuestions, int numberOfPages) {
	
		foreach (MoodleCourse courseItem in myListofCourses) {

			if (courseItem.courseId == courseId) {

				foreach (MoodleQuiz quizItem in courseItem.myQuizzes) {

					if (quizItem.quizID == quizId) {

						quizItem.attemptID = attemptId;
						quizItem.numberOfQuestions = numberOfQuestions;
						quizItem.numberOfPages = numberOfPages;
						quizItem.attemptUniqueId = attemptUniqueId;
					}

				}
			}

		}
	
	}

	public async Task SaveResultToActiveQuestion(bool result) {
		Debug.Log ("i am in save result to active question");
		myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions [myActiveGame.questionNr].successValue = result;

		//for convenience we store the result in the list of random questions:
		int i;
		//in case of success, we use 2
		if (result) {
			i = 2;
		//1 means, that we failed (0 is not yet tried)
		} else {
			i = 1;
		}

		//Debug.Log ("i is "+i+" key1: "+myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions [myActiveGame.questionNr].questionNr+" key2 "+myListOfRandomQuestions[myActiveGame.randomQuestionNr].a);

		myListOfRandomQuestions[myActiveGame.randomQuestionNr].b = i;



	}



	public int GetNumberOfSuccessfulQuestionsOfActiveCourse() {
		int i = 0;
		foreach (MoodleQuestion questionItem in myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions) {
			if (questionItem.successValue) {
				i++;
			}
		}
		return i;
	}

	public int ThisWasTheLastQuestion() {
		Debug.Log ("last question?");
		//Debug.Log (myActiveGame.questionNr+" > "+ myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions.Count);

		if (myActiveGame.questionNr == myListOfRandomQuestions[myListOfRandomQuestions.Count-1].a) {
			Debug.Log ("this was the last question, I set value to 1");
			return 1;
		} else {
			return 0;
		}



	}


	public async Task AddQuestionsToQuiz(int courseId, int quizId, MoodleQuestion newQuestion) {

		//Debug.Log ("no i add question to quiz");
		//Debug.Log ("course: "+courseId+" quizid "+quizId);

		foreach (MoodleCourse courseItem in myListofCourses) {

			if (courseItem.courseId == courseId) {

				foreach (MoodleQuiz quizItem in courseItem.myQuizzes) {

					if (quizItem.quizID == quizId) {

						quizItem.AddQuestion (newQuestion);

					}

				}


			}

		}

	}



	public bool CourseByIDHasMoreThanOneQuiz(int courseId) {


		foreach (MoodleCourse courseItem in myListofCourses) {

			if (courseItem.courseId == courseId && courseItem.myQuizzes.Count > 1) {
				Debug.Log ("active course has more than one quiz");
				return true;
			}

		}

		Debug.Log ("active course has only one quiz");
		return false;

	}


	public async Task SetNumberOfQuestionsForCourseByID(int courseId, int quizId) {

		foreach (MoodleCourse courseItem in myListofCourses) {

			if (courseItem.courseId == courseId) {

				foreach (MoodleQuiz quizItem in courseItem.myQuizzes) {
					
					
					
				}

			}

		}




	}

//	public Dictionary<int,int> GetDictionaryWithRandomizedQuestionsAndAnswers() {
//		Dictionary<int,int> newDict = new Dictionary<int, int> ();
//			
//		foreach (PairOfInts listItem in myListOfRandomQuestions) {
//			newDict.Add(listItem.a, myListofCourses[activeCourseId].myQuizzes[activeQuizId].GetSuccessOfQuestionById(listItem.a));
//		}
//		return newDict;
//	}
//

	public async Task RandomizeQuestions() {
		//Debug.Log ("randomize questions");
		List<MoodleQuestion> listToPickFrom = new List<MoodleQuestion> ();

		//we have to delete question
		myListOfRandomQuestions.Clear ();
		listToPickFrom.Clear ();

		//listToPickFrom = myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions;
		foreach (MoodleQuestion listItem in myListofCourses [activeCourseId].myQuizzes[activeQuizId].myListOfQuestions) {

			listToPickFrom.Add (listItem);
			Debug.Log ("question "+listItem.questionNr);
		}



		int i;
		int minValue = listToPickFrom.Count - randomQuizSize;

		if (minValue < 0) {
			minValue = 0;
		}

		//Debug.Log ("start while with "+minValue);
		while (listToPickFrom.Count > minValue) {
			i = Random.Range (0, listToPickFrom.Count - 1);
			myListOfRandomQuestions.Add (new PairOfInts(listToPickFrom[i].questionNr-1));
			listToPickFrom.RemoveAt (i);
			//Debug.Log (listToPickFrom.Count);
			if (listToPickFrom.Count < 1) {
				break;
			}

		}

		foreach (PairOfInts listItem in myListOfRandomQuestions) {
		
			Debug.Log (listItem.a+" "+listItem.b);
		
		}

			
		//Debug.Log ("finish randomize questions");
	}


	public async Task ReadRandomQuestionsString(string randomQuestions) {

		Debug.Log ("readrandomquestionsString"+randomQuestions);

		string[] listOfStrings;
		string[] pairOfInts;

		myListOfRandomQuestions.Clear ();

		listOfStrings = randomQuestions.Split ('@');

		foreach (string listItem in listOfStrings) {
			pairOfInts = listItem.Split(',');

			//Debug.Log (pairOfInts[0]+" "+pairOfInts[1]);
			PairOfInts newPair = new PairOfInts (int.Parse( pairOfInts[0]), int.Parse(pairOfInts[1]));

			if (newPair.b != 0) {
				myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions [newPair.a].answered = true;
			}


			if (newPair.b == 2) {
				myListofCourses [activeCourseId].myQuizzes [activeQuizId].myListOfQuestions [newPair.a].successValue = true;
			}

			myListOfRandomQuestions.Add(newPair);
			
		}
	}

	public async Task SetRandomQuestionNr() {
	
		int i = 0;
		foreach (PairOfInts listItem in myListOfRandomQuestions) {
			if (listItem.b == 0) {
				break;
			} else {
				i++;
			}
		
		}	
		Debug.Log ("set randomquestionNr to "+i);
		myActiveGame.randomQuestionNr = i;
	}


	public string ResetRandomGameSequence(string randomQuestions) {
		string[] listOfStrings;
		string newString = "";

		myListOfRandomQuestions.Clear ();

		listOfStrings = randomQuestions.Split ('@');

		foreach (string listItem in listOfStrings) {

			newString += listItem.Substring (0, listItem.Length - 1) + "0@";

		}
		newString = newString.Substring(0,newString.Length-1);
		Debug.Log("after resetting: "+newString);
		return newString;
	}


	public async Task DeleteAllQuestionsofQuiz(int courseId, int quizId) {

		foreach (MoodleCourse courseItem in myListofCourses) {

			if (courseItem.courseId == courseId) {

				foreach (MoodleQuiz quizItem in courseItem.myQuizzes) {

					if (quizItem.quizID == quizId) {
						
						Debug.Log ("I deleted all questions "+quizItem.myListOfQuestions.Count);
						quizItem.myListOfQuestions.Clear ();
					}

				}


			}

		}



	}


}


public class PairOfInts {

	public int a;
	public int b;

	public PairOfInts() {
	
	
	}

	//if we have one value, we make it the key
	public PairOfInts(int c) {
		a = c;
		b = 0;
	}

	//if we have two values, we assume we want to add a value to a key
	//therefore, we check if it is the right key

	public PairOfInts(int c, int d) {

		a = c;
		b = d;
	}

	public async Task AddValueToKey(int c, int d) {

		if (a == c) {
			b = d;
			Debug.Log ("matching in addvaluetokey worked");
		} else {
			Debug.Log ("wrong matching in pairofInts class");
		}

	}

}