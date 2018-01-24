using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameController : MonoBehaviour {

	public GameObject registerPage;
	public GameObject listOfCoursesPage;
	public GameObject listofQuizzesPage;
	public GameObject startPage;
	public GameObject questions;
	public GameObject endPage;
	public GameObject stopOverPage;

	public Text questionText;
	public Text goodByeText;
	public Text stopOverButtonText; 

	public Text enterName;
	public Text enterPW;

	public Button buttonA;
	public Button buttonB;
	public Button buttonC;
	public Button buttonD;


	public GameObject step1_red;
	public GameObject step2_red;
	public GameObject step3_red;
	public GameObject step1_green;
	public GameObject step2_green;
	public GameObject step3_green;

	public GameObject moodleObject;


	public GameObject CourseContentPanel;
	public GameObject CourseListItem;

	public GameObject QuizContentPanel;
	public GameObject QuizListItem; //yellow

	public GameObject OpenGamesButton;   //red
	public GameObject FinishedGamesButton; //blue

	public GameObject UserContentPanel;
	public GameObject UserListItem;

	public GameObject WaitPanel;
	public GameObject NoConnectionPanel;

	public GameObject QuestionListItem;

	GameObject newCourseButton;
	GameObject newQuizButton;
	GameObject newUserButton;


	public GameObject RedTab;
	public GameObject YellowTab;
	public GameObject BlueTab;
	public GameObject GreenTab;

	public GameObject redSpinner;

	public bool unfinishedPackage = true;

	int activePanelId= 1;

	//int challengedUserId = 0;

	// this will go to preferences:
	const int randomQuizSize = 9;
	const int packageSize = 3;


	//public static List<MoodleQuestion> moodleListOfQuestions = new List<MoodleQuestion> ();

	public List<MoodleUser> myListOfUsers = new List<MoodleUser>();

	public DataController dataController;
	MoodleUser activeUser;


	QuizQuestions myQuestions = new QuizQuestions();


	//Table of Content

	//1. Initiatialization (do we have user Token in playerprefs? If so, we skip registration)

	//2. PlayerPrefs Operations

	//3. 

	//4. 





	//1. Initiatialization
	void Start () {
		//ConnectToMoodle ();

		//PlayerPrefs.DeleteAll ();

		//Debug.Log (PlayerPrefs.GetInt("userid"));

		SwitchPanel (activePanelId);

		//we check if we find a user token in player prefs
		//if so, we can skip the registration process


		if (LoadUserFromPlayerPrefs ()) {
				
			ProceedWithTokenToListOfCourses (activeUser.userToken, activeUser.userId);

		}



	}


	//##############################

	// Here are the Steps from one panel to the next

	//##############################

	//
	//here we take the provided token (either from registration or playerprefs) and load the list of courses
	//then we create the buttons
	//and switch to the next panel
	async Task ProceedWithTokenToListOfCourses(string userToken, int userId) {
		Debug.Log ("ProceedWithTokenToListOfCourses");

		await FetchListOfCourses (userToken,userId);

		await BuildListOfDatabasesForAllCourses ();

		foreach (MoodleCourse courseItem in dataController.myListofCourses) {
		
			await FetchUserByCourseId (courseItem.courseId);
		}





		//we call this because we don't show the list of courses anymore
		await ProceedFromListOfCoursesToListOfQuizzes ();


		//we skip this because we want to go directly to the page of quizzes
		//CreateButtonsForListOfCourses ();
		//GoToNexPanel ();



	
	}

	//this is normally called from a click on the course button

	//change: this is called directly, we skip the list of courses
	async Task ProceedFromListOfCoursesToListOfQuizzes() {
		Debug.Log ("ProceedFromListOfCoursesToListOfQuizzes");	

		//CreateOpenGameTab ();

		//first we check if we are challenged:
		foreach (MoodleCourse courseItem in dataController.myListofCourses) {
			Debug.Log ("check if challenged for "+courseItem.courseId);

			//CheckIfICanContinueOpenGame
			//true for being challenged
			await CreateButtonsForUnfinishedGames (courseItem.courseId, true);
		}

		//then we check if we have challenged somebody
		foreach (MoodleCourse courseItem in dataController.myListofCourses) {
			Debug.Log ("CreateButtonsForCourse "+courseItem.courseId);
			//CheckIfICanContinueOpenGame (courseItem.courseId);
			//false for beeing the challenger
			await CreateButtonsForUnfinishedGames (courseItem.courseId, false);
		}




		//now we create the list of quizzes
		foreach (MoodleCourse courseItem in dataController.myListofCourses) {

			await CreateButtonsForListOfQuizes (courseItem.courseId);
		
		}

		ShowWaitingPanel (false);
		//we move two panels ahead
		activePanelId++;
		GoToNexPanel ();


//		} else {
//
//			Debug.Log ("only one quiz in course, I start right away, but no logic for it yet!");
//
//			// I can swith two panels
//			activePanelId++;
//			GoToNexPanel ();
//
//		}
	}

//this is normally called from a click on the quiz button
//we have to fetch all the questions and the structure of the course here, so we will have to work with threads for...
//... better performance

	async Task ProceedFromListofQuizzesToTheFirstQuestion(int courseId, int quizId, string buttonName) {
		Debug.Log ("ProceedFromListofQuizzesToTheFirstQuestion");

		int numberOfQuestions = await FetchQuestionsForQuiz (courseId, quizId);

		//create list of users

		//FetchUserByCourseId(courseId);


		if (numberOfQuestions >= dataController.randomQuizSize) {

			CreateButtonsForListOfUsers (courseId, quizId);

			GoToNexPanel ();

			//i think we need this
			dataController.myActiveGame = null;
			dataController.myActiveGame = new OpenGameClass ();

			//we define the game as singlePlayer
			dataController.myActiveGame.singlePlayerMode = true;

			dataController.myActiveGame.buttonName = buttonName;

			//here we set the right course in the instance
			await dataController.StartQuizbyCourseAndId (courseId, quizId);

			//Debug.Log ("i print active quiz");
			//dataController.GetActiveQuiz ().PrintQuiz ();


			//we only do it if we are not in challenge:
			dataController.RandomizeQuestions ();

			//we want to save the gamesequence as string in active game
			dataController.myActiveGame.randomGameSequence = ListOfIntToJsonString (dataController.myListOfRandomQuestions);

			await RetrieveNextQuestion (true);
		} else {
			Debug.Log ("not enough questions to play "+numberOfQuestions);
		}

	
	}

	async Task ProceedFromChallengeToFirstQuestion(int courseId, int quizId) {

		Debug.Log ("ProceedFromChallengeToFirstQuestion");

		await FetchQuestionsForQuiz (courseId, quizId);


		//here we set the right course in the instance
		await dataController.StartQuizbyCourseAndId(courseId,quizId);

		//we define the game as multiplayer
		dataController.myActiveGame.singlePlayerMode = false;

		Debug.Log("my "+dataController.myActiveGame.randomGameSequence+" her "+dataController.myActiveGame.herRandomGameSequence);


//		Debug.Log ("print game");
//		dataController.myActiveGame.PrintGame ();

		//load random questinewons
		//here we
		await dataController.ReadRandomQuestionsString (dataController.myActiveGame.randomGameSequence);

		await dataController.SetRandomQuestionNr ();

		//here we check we have the right sequence in our storrage
		Debug.Log(ListOfIntToJsonString (dataController.myListOfRandomQuestions));

		await RetrieveNextQuestion (true);

		//we skip one panel, we don't need the startButton here. Or do we?
		ShowWaitingPanel(false);
		//we go to "stoppover"
		SwitchPanel (7);

		if (IsItMyTurn (dataController.myActiveGame.randomGameSequence, dataController.myActiveGame.herRandomGameSequence)) {

			stopOverButtonText.text = "go to the questions";
			
		} else {
		
			stopOverButtonText.text = "going back to quizzes";
		
		}


		SetBackRedAndGreen ();

		Debug.Log("my "+dataController.myActiveGame.randomGameSequence+" her "+dataController.myActiveGame.herRandomGameSequence);

		Debug.Log ("now I set the lights for game "+dataController.myActiveGame.link_Id);
		await stopOverPage.GetComponent<StopOverController> ().SetLights (dataController.myActiveGame.randomGameSequence, dataController.myActiveGame.herRandomGameSequence);
		//stopOverPage.GetComponent<StopOverController> ().SetLightsForPlayer2 (dataController.myActiveGame.herRandomGameSequence);

		Debug.Log ("after setlights");
	}


	void ProceedFromOnePackageOfQuestionsToStopOver() {




	}

	void ProceedFromOneStopOverToNextPackageOfQuestions() {



	}



	//##############################
	///Multiplayerlogic
	///##############################
	/// 
	///1. defy somebody with chosen quiz
	///  -write to DuelDB
	/// -write to controlDBHost with duelDBentry
	///
	/// 2. you are defied by somebody
	/// - check if there is a challenge
	/// - accept challenge
	/// - load random sequence
	/// - write to controlDBExtent
	/// - play game


	//I can get quizid and courseId elsewhere
	// I call this with a click on the user button

	async Task CreateChallenge(int challengedUserId, int courseId) {
		Debug.Log ("CreateChallenge");
		/// First I write to duelEntryDB and thus retrieve an id
		/// then I write to controlDB_Host
		/// then I go to next panel, everything is ready, I have the challenge


		MoodleDatabase controlDbHostDb = dataController.GetDbByNameAndByCourseId ("ControlDB_Host", courseId);

		//we have to fetch the duelEntryId by creating an entry in the duelDB database
		int duelEntryId;


		string randomizedQuestions = ListOfIntToJsonString (dataController.myListOfRandomQuestions);

		Debug.Log ("randomized questions "+randomizedQuestions);

		string hashString = CreateHash(activeUser.userId.ToString()+"X"+challengedUserId.ToString()+"X"+dataController.GetActiveQuizId().ToString()+randomizedQuestions);

		duelEntryId = await WriteNewLineToDuelEntryDB (randomizedQuestions, hashString, 0, courseId);

		dataController.myActiveGame.duelEntryId = duelEntryId;
		dataController.myActiveGame.challengerId = challengedUserId;
		dataController.myActiveGame.quizId = dataController.GetActiveQuizId ();
		dataController.myActiveGame.link_Id = hashString;
		dataController.myActiveGame.randomGameSequence = randomizedQuestions;
		dataController.myActiveGame.status = 0;

		dataController.myActiveGame.singlePlayerMode = false;

		Debug.Log("json");
		//Write line to ControlDB_Host
		string newString = await moodleObject.GetComponent<MoodleAPI>().TransmitChallengeToControlDB_Host(
			activeUser.userToken, controlDbHostDb.databaseId,
			//Challenger_ID  |  
			controlDbHostDb.dbField["Challenger_ID"].ToString(), activeUser.userId.ToString(),
			//Subject_ID  |  
			controlDbHostDb.dbField["Subject_ID"].ToString(), challengedUserId.ToString(),
			//Quiz_ID  |  
			controlDbHostDb.dbField["Quiz_ID"].ToString(), dataController.GetActiveQuizId().ToString(),
			//Status  |  
			controlDbHostDb.dbField["Status"].ToString(), "0",
			//Duel_Entry_ID  |  
			controlDbHostDb.dbField["Duel_Entry_ID"].ToString(), duelEntryId.ToString(),
			//Link_ID  |
			controlDbHostDb.dbField["Link_ID"].ToString(), hashString);
		
		JSONObject encodedString = new JSONObject(newString);

		Debug.Log (encodedString);

		Debug.Log (encodedString["newentryid"]);

		//dataController.activeDbHostEntryId = int.Parse (encodedString ["newentryid"].ToString ());

		//check if we nee a Tab


		CreateButtonForChallenge (
			dataController.GetCourseIdByQuizId(dataController.GetActiveQuizId()), 
					dataController.GetActiveQuizId(),
					RetrieveNameById(challengedUserId), 
					duelEntryId, 
					hashString, 
			challengedUserId, 
					0, 
					false);

		GoToNexPanel ();
		
	}

	void CreateOpenGameTab() {

		GameObject newTab = Instantiate<GameObject> (RedTab, QuizContentPanel.transform, false);
		newTab.transform.SetSiblingIndex (0);

	}
		
	void CreateEndedGamesTab() {

		GameObject newTab = Instantiate<GameObject> (GreenTab, QuizContentPanel.transform, false);
		newTab.transform.SetAsLastSibling();
	}

	void CreateAvailableQuizzesTab(int position) {

		GameObject newTab = Instantiate<GameObject> (YellowTab, QuizContentPanel.transform, false);
		newTab.transform.SetSiblingIndex (position);
	}



	async Task CreateButtonsForUnfinishedGames(int courseId, bool asSubject) {
		Debug.Log ("CreateButtonsForUnfinishedGames");

		string newString;
		JSONObject encodedString;

		dataController.myListofGames.Clear ();

		dataController.myListofGames = await ListOfGamesInControlHostDB (courseId, asSubject);

		foreach (OpenGameClass gameItem in dataController.myListofGames) {
			Debug.Log ("i run through games "+gameItem.link_Id);
				

//			if (gameItem.status == 0) {
				if (0 == 0) {

				Debug.Log ("my "+gameItem.randomGameSequence+" her "+gameItem.herRandomGameSequence);

				//we fetch more information on this game. is it finished already?
				OpenGameClass newGame = await FetchDataFromDuellDB (gameItem.quizId, gameItem.duelEntryId, gameItem.link_Id, courseId);
				if (newGame == null) {
					Debug.Log ("i skip this entry "+gameItem.link_Id);
					continue;
				};
				await newGame.UpdateInformationFromDuelDB (gameItem);
				Debug.Log ("my "+newGame.randomGameSequence+" her "+newGame.herRandomGameSequence);

				/// I have all the data, her string, my string, status
				/// I now want to now if I have finished all the questions
				/// 
				/// - am I finished? if not, I create a button
				/// - if I am finished, i do nothing, because my adversary has to put status to 1 (finished);
				/// (remember: I am here in "subject" mode, so the db host entry is NOT mine.



				if (gameItem.status == 0) {

					//if I am finished and she is finished
					//I put status to 1
					if (AmIFinished (newGame.randomGameSequence) && AmIFinished(newGame.herRandomGameSequence)) {
						int dbStatusField = dataController.GetDbByNameAndByCourseId("ControlDB_Host", courseId).dbField["Status"];
						Debug.Log("everybody is finished, i set status to 1 with field "+dbStatusField);
						newString = await moodleObject.GetComponent<MoodleAPI> ().TransmitAndUpdateAnswerSet1 (
							activeUser.userToken, gameItem.dbHotEntryId,
							dbStatusField.ToString (), "1");
						encodedString = new JSONObject (newString);
						Debug.Log ("after writing");
						newGame.status = 1;

						//continue;
						//I go on, because I create the finishedGameButton
					}
					else if (AmIFinished (newGame.randomGameSequence)) {
						Debug.Log ("I have finished my questiosn so I treat this button as if status was 1");
						newGame.status = 1;
					}


				}
					

				int userId;
				if (asSubject) {
					userId = newGame.challengerId;
				} else {
					userId = newGame.subjectId;
				}
				Debug.Log ("id "+userId+" subject "+asSubject);


				ReactOnChallengeAndCreateButtons (newGame.quizId, userId, newGame.duelEntryId, newGame.link_Id, newGame.status, asSubject);
			}
		}
	}

	async Task <List<OpenGameClass>> ListOfGamesInControlHostDB(int courseId, bool asSubject) {
		Debug.Log ("FetchDataFromControlHostDB");
		List<OpenGameClass> listOfGames = new List<OpenGameClass> ();

		MoodleDatabase controlDbHostDb = dataController.GetDbByNameAndByCourseId ("ControlDB_Host", courseId);
		string dataField;

		if (controlDbHostDb != null) {

			if (asSubject) {
				dataField = "Subject_ID";
			} else {
				dataField = "Challenger_ID";
			}


			Debug.Log("json");

			//Fetch Data from dbhost

			string newString = await moodleObject.GetComponent<MoodleAPI> ().CheckIfChallengedByUserId (
				                   activeUser.userToken, controlDbHostDb.databaseId,
				                   controlDbHostDb.dbField [dataField].ToString (), activeUser.userId.ToString ());

			JSONObject encodedString = new JSONObject (newString);

			foreach (JSONObject listItem in encodedString["entries"].list) {
				//Debug.Log ("item in entries");

				//we create a new Game, but only save it, if the status is right
				OpenGameClass newGame = new OpenGameClass();

				newGame.dbHotEntryId = int.Parse(listItem ["id"].ToString());
				foreach (JSONObject recordItem in listItem["contents"].list) {
					//					Debug.Log ("item in contents");
					//					Debug.Log (recordItem ["fieldid"]);
					//					Debug.Log (recordItem ["fieldid"].ToString().Length);
					string field = recordItem ["fieldid"].ToString (); 
					string content = recordItem ["content"].ToString ().Replace ("\"", "");
					//.Substring (1, recordItem ["fieldid"].ToString ().Length - 2);
					//					Debug.Log (field);

					if (int.Parse (field) == controlDbHostDb.dbField ["Challenger_ID"]) {

						newGame.challengerId = int.Parse (content);
					}
					if (int.Parse (field) == controlDbHostDb.dbField ["Subject_ID"]) {
						newGame.subjectId = int.Parse (content);
						Debug.Log ("subject id should be not null! "+content);
					}
					if (int.Parse (field) == controlDbHostDb.dbField ["Quiz_ID"]) {

						newGame.quizId = int.Parse (content);
					}
					if (int.Parse (field) == controlDbHostDb.dbField ["Status"]) {

						newGame.status = int.Parse (content);
					}
					if (int.Parse (field) == controlDbHostDb.dbField ["Duel_Entry_ID"]) {

						newGame.duelEntryId = int.Parse (content);
					}
					if (int.Parse (field) == controlDbHostDb.dbField ["Link_ID"]) {

						newGame.link_Id = content;
					}

				}
				Debug.Log ("here it is where i print game after fetching from dbhost");
				newGame.PrintGame ();
				listOfGames.Add (newGame);

			}
		}


		return listOfGames;
	}




	async Task<int> WriteNewLineToDuelEntryDB(string randomizedQuestions, string hashString, int status, int courseId) {
		Debug.Log ("WriteNewLineToDuelEntryDB");
		MoodleDatabase duelDB = dataController.GetDbByNameAndByCourseId ("DuelDB", courseId);
		//we have to fetch the duelEntryId by creating an entry in the duelDB database

		JSONObject encodedString;
		Debug.Log("json");
		Debug.Log ("i write random game sequence "+randomizedQuestions);
		//Write line to Duel_Entry
		string newString = await moodleObject.GetComponent<MoodleAPI>().TransmitChallengeToDuel_Entry(
			activeUser.userToken, duelDB.databaseId,
			//Link_ID  |  
			duelDB.dbField["Answers_Set"].ToString(), randomizedQuestions,
			//Status  |  
			duelDB.dbField["Link_ID"].ToString(), hashString,
			//Duel_Entriy_ID  |  
			duelDB.dbField["Status"].ToString(), status.ToString());
		encodedString = new JSONObject(newString);

		return int.Parse(encodedString["newentryid"].ToString());

	}


	void ReactOnChallengeAndCreateButtons(int quizId, int userId, int duelEntryId, string linkId, int status, bool asSubject) {

		//if I find myself as challenged in database
		//check the status
		//if finished -> do nothing
		//if open -> create button to allow to restart quiz
		Debug.Log("ReactOnChallengeAndCreateButtons "+linkId);
//		Debug.Log (quizId);
//		Debug.Log (userId);
//		Debug.Log (duelEntryId);
//		Debug.Log (linkId);
		string nameOfUser;

		MoodleUser adversaireUser = myListOfUsers.Where (x => x.userId == userId).SingleOrDefault ();

		if (adversaireUser != null) {
			if (!string.IsNullOrEmpty (adversaireUser.userName)) {
				nameOfUser = adversaireUser.userName;
			} else {
				nameOfUser = adversaireUser.fullName;
			}

		} else {
			nameOfUser = "anonymous";
		}

		Debug.Log (nameOfUser);
		CreateButtonForChallenge (dataController.GetCourseIdByQuizId(quizId), quizId,nameOfUser, duelEntryId, linkId, userId, status, asSubject);

	}




	public string CreateHash(string strToEncrypt) {
		Debug.Log ("create hash with string "+strToEncrypt);
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		//we only take half the length
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}
	
	string ListOfIntToJsonString(List<PairOfInts> newList) {
		StringBuilder newString = new StringBuilder ();

		//newString.Append("[");
		foreach (PairOfInts listItem in newList) {
			newString.Append (listItem.a.ToString ()+","+listItem.b.ToString ()+"@");
		}
		//delete last @
		newString.Remove (newString.Length - 1, 1);
		//newString.Append ("]");

		//Debug.Log (newString);
		return newString.ToString();
	}




	//##############################

	// Player Prefs Functions

	//##############################

	void SetPlayerPrefs(string userToken, int userId, string userName) {
		PlayerPrefs.SetString ("userToken", userToken);
		PlayerPrefs.SetInt ("userid", userId);
		PlayerPrefs.SetString ("userName", userName);

		Debug.Log("just wrote playerPrefs");
	}

	bool LoadUserFromPlayerPrefs() {

		string userToken = PlayerPrefs.GetString ("userToken");
		int userId = PlayerPrefs.GetInt ("userid");
		string userName = PlayerPrefs.GetString ("userName");

		if (userToken.Length > 1) {
			//we have user

			activeUser = new MoodleUser (userId, userName,userToken);
			Debug.Log ("created active user from PlayerPrefs");
			Debug.Log (activeUser.userName);
			Debug.Log (activeUser.userToken);
			Debug.Log (activeUser.userId);

			return true;

		} else {
			//we don't have a user
			//we proceed to fetch it from Moodle
			Debug.Log ("not yet registered");
			return false;
		}

	}


	//this is a button on panel 2
	public void UnloadUserFromPlayerPrefsAndGoToRegister() {

		PlayerPrefs.SetString ("userToken", "");
		PlayerPrefs.SetInt ("userid", 0);
		PlayerPrefs.SetString ("userName", "");

		Debug.Log("just deleted playerPrefs");

		//we have to delete all the loades quizzes

		dataController.myActiveGame = null;



		foreach (Transform child in QuizContentPanel.transform) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in UserContentPanel.transform) {
			GameObject.Destroy(child.gameObject);
		}

		SwitchPanel (1);

	}







//	#####################
//  Here I fetch Json Strings from the Server and transform them in my Data Format
//	#####################

	public void ClickOnGetTokenAndLoadSet() {
	
		ShowWaitingPanel (true);

		GetTokenAndLoadSet ();
	}


	// this is used in the registration process only and called from the button
	public async Task GetTokenAndLoadSet() {
		Debug.Log ("GetTokenAndLoadSet");

		string userToken;
		int userId;

		//waitPanel wird eingeblendet, solange wir auf eine Abfrage warten.

		userToken = await FetchToken (enterName.text, enterPW.text);

		if (userToken != "") {
			userId = await FetchUserId (userToken);

			Debug.Log ("my retrieved id " + userId);

			SetPlayerPrefs (userToken, userId, enterName.text);

			activeUser = new MoodleUser (userId, enterName.text, userToken);

			//we have to call this from here
			//I don't see how we could do it differently
			//apart from a listener, and think performancewise this is the better choice here

			await ProceedWithTokenToListOfCourses (userToken, userId);
		} else {
			ShowWaitingPanel (false);
		}
	}

	async Task<string> FetchToken(string userName, string userPassword) {
		string userToken;
		Debug.Log ("i load token");
		Debug.Log("json");
		string newString = await moodleObject.GetComponent<MoodleAPI> ().FetchUserToken (userName, userPassword);
		JSONObject encodedString = new JSONObject (newString);

		if (encodedString ["token"] != null) {

			userToken = encodedString ["token"].ToString ();

			userToken = userToken.Substring (1, userToken.Length - 2);

			return userToken;
		} else {
			return "";
		}

	}



	public async Task<int> FetchUserId(string userToken) {
		int i;
		Debug.Log("json");
		string newString = await moodleObject.GetComponent<MoodleAPI>().FetchCoreInfo (userToken);
		JSONObject encodedString = new JSONObject (newString);

		i = int.Parse( encodedString ["userid"].ToString ());

		return i;
	}

//	public void FetchListOfCourses(string userToken, int userId) {
//		Debug.Log ("FetchListOfCourses");
//		//when we call this, we want to make sure the list is clean:
//		dataController.myListofCourses.Clear ();
//		Debug.Log("json");
//		JSONObject encodedString = new JSONObject(moodleObject.GetComponent<MoodleAPI>().FetchUserCourses(userToken,userId));
//
//		//here we create from the json Object our own data structure
//		foreach (JSONObject courseItem in encodedString.list) {
//			
//			dataController.AddMoodleCourse (int.Parse(courseItem["id"].ToString()), 
//				clean(courseItem["fullname"].ToString()), 
//				int.Parse(courseItem["enrolledusercount"].ToString()));
//
//			//we have to fetch the information how many quizzes we have in the course
//			//StartCoroutine(FetchQuizzesByCourseId(dataController.MyLastCourseInList ().courseId));
//			FetchQuizzesByCourseId(dataController.MyLastCourseInList ().courseId);
//
//			dataController.CountQuizzesForLastCourseInList ();
//				
//
//		}
//
//	}


	public async Task FetchListOfCourses(string userToken, int userId) {

		dataController.myListofCourses.Clear ();
		Debug.Log("json");

		//ShowWaitingPanel (true);

		string encodedString = await moodleObject.GetComponent<MoodleAPI> ().FetchUserCourses (userToken, userId);

		//ShowWaitingPanel (false);

		//Debug.Log (encodedString);
		await BuildListOfCourses (encodedString);
	}


	async Task BuildListOfCourses(string theString) {

		JSONObject encodedString = new JSONObject (theString);

		//here we create from the json Object our own data structure
		foreach (JSONObject courseItem in encodedString.list) {

			await dataController.AddMoodleCourse (int.Parse(courseItem["id"].ToString()), 
				clean(courseItem["fullname"].ToString()), 
				int.Parse(courseItem["enrolledusercount"].ToString()));

			//we have to fetch the information how many quizzes we have in the course
			//StartCoroutine(FetchQuizzesByCourseId(dataController.MyLastCourseInList ().courseId));
			await FetchQuizzesByCourseId(dataController.MyLastCourseInList ().courseId);

			await dataController.CountQuizzesForLastCourseInList ();
		}

		Debug.Log ("I have built the courses");

	}





	//we return the number of Quizes found in Course 
	//BUT we also store all the information we got in the course in Question
	//So, every course with quizzes stores a list of quizzes with Id, Name and Description

	async Task FetchQuizzesByCourseId(int activeCourseId) {
		Debug.Log ("FetchQuizzesByCourseId");
		Debug.Log("json");
		string newString = await moodleObject.GetComponent<MoodleAPI> ().FetchQuizzesinCourse (activeUser.userToken, activeCourseId);
		JSONObject encodedString =  new JSONObject(newString);

		JSONObject quizArray = encodedString ["quizzes"];

		//Debug.Log (encodedString.ToString());

		foreach (JSONObject quizItem in quizArray.list) {
			//only if we have the right behaviour
			if (quizItem ["preferredbehaviour"].ToString ().Contains ("immediatefeedback") 
				&& quizItem ["canredoquestions"].ToString ().Contains ("1")) {

				await dataController.AddNewQuizToCourseById (activeCourseId, int.Parse (quizItem ["id"].ToString ()), clean (quizItem ["name"].ToString ()), clean (quizItem ["intro"].ToString ()), quizItem ["preferredbehaviour"].ToString ());
			} else {
				
				Debug.Log ("i dont add quiz "+quizItem ["name"].ToString ());
				Debug.Log ("canredoquestions: "+quizItem ["canredoquestions"].ToString ());
				Debug.Log ("preferredbehaviour: "+quizItem ["preferredbehaviour"].ToString ());

			}

		}
	}


	//we add the number of questions for every quiz
	//but we also build the list of question here

	async Task<int> FetchQuestionsForQuiz(int courseId, int quizId) {
		Debug.Log ("FetchQuestionsForQuiz");
		//first we assume we don't have an attempt and try to receive a new id
		//if we get an error, we get the id for the quiz in question
		//if we dont, we get an attempt Id
		//we read the number of questions and return them


	
		//Debug.Log("my "+dataController.myActiveGame.randomGameSequence+" her "+dataController.myActiveGame.herRandomGameSequence);

		Debug.Log("json");
		string newString = await moodleObject.GetComponent<MoodleAPI>().StartNewAttemptIdByQuiz(activeUser.userToken, quizId);
		JSONObject encodedString = new JSONObject(newString);
		int attemptId = 0;
		int attemptUniqueId = 0;
		int numberOfQuestions = 0;
		int numberOfPages = 1;
		int firstQuestionNr = 1;

		int pageNr = 0;


		//Debug.Log("my "+dataController.myActiveGame.randomGameSequence+" her "+dataController.myActiveGame.herRandomGameSequence);
		Debug.Log(encodedString);
		Debug.Log("json");
		//if we have an ongoing attempt, we can go on and ask the server for the ID
		if (encodedString ["exception"]) {
			newString = await moodleObject.GetComponent<MoodleAPI> ().FetchAttemptIdByQuiz (activeUser.userToken, quizId);
			encodedString = new JSONObject(newString);
			Debug.Log(encodedString);
		}

		//we have to get to the relevant information by parsing JSON
		if (encodedString ["attempt"]) {
			encodedString = encodedString ["attempt"];
		}
		Debug.Log(encodedString);
		//here we check if we have more than one attempt going on
		//we have to make sure not to use an attempt Id which is already "finished"
		if (encodedString ["attempts"]) {

			foreach (JSONObject attemptItem in encodedString ["attempts"].list) {

				if (attemptItem ["state"].ToString().Contains("inprogress")) {
					encodedString = attemptItem;
					//Debug.Log ("I broke with "+attemptItem["id"]);

					//

					break;
				}

			}
		}
			
		if (encodedString ["id"] && (int.Parse(encodedString ["quiz"].ToString()) == quizId)) {
				numberOfQuestions = ParserReturnNumberOfQuestions (encodedString ["layout"].ToString ());
				numberOfPages = ParserReturnNumberOfPages (encodedString ["layout"].ToString ());
				attemptId = int.Parse (encodedString ["id"].ToString ());
				attemptUniqueId = int.Parse (encodedString ["uniqueid"].ToString ());
				//Debug.Log ("number of Pages: "+numberOfPages);

			await dataController.DeleteAllQuestionsofQuiz (courseId, quizId);


			//we might have to repeat if we have more than one page
			while (pageNr < numberOfPages) {
				//Debug.Log ("while pageNR "+pageNr+" number of pages "+numberOfPages);
				//Here I build the Questions
				await BuildListOfQuestionsForQuizId (courseId, quizId, attemptId, attemptUniqueId, pageNr);

				//we increase Pagenumber because we are in "while pageNr < numberOfPages"
				pageNr++;
			}
			
		} else {
			Debug.Log ("we have problem with the answer of the server at pagenr "+pageNr);
		}

		//once we ran through all pages, we have the information we need to write down the information
		//[its easier to get this information from parsing of layout field, see above]
		//numberOfQuestions = dataController.GetQuizByCourseByID(courseId,quizId).myListOfQuestions.Count;

		await dataController.SaveInformationToQuiz (courseId, quizId, attemptId, attemptUniqueId, numberOfQuestions, numberOfPages);

		Debug.Log ("i got all the questions");

		return numberOfQuestions;
	}

	async Task FetchUserByCourseId(int activeCourseId) {
		Debug.Log ("FetchUserByCourseId");
		Debug.Log("json");

		string newString = await moodleObject.GetComponent<MoodleAPI> ().FetchUsersInCourse (activeUser.userToken, activeCourseId);
		JSONObject encodedString =  new JSONObject(newString);

		MoodleUser newUser;
		int userId;
		string userName;
		//Debug.Log (encodedString.ToString());

		foreach (JSONObject userItem in encodedString.list) {
			//only if we have the right behaviour
				userId = int.Parse (userItem ["id"].ToString ());
			//Debug.Log (userItem ["fullname"].ToString ());


			userName = clean (userItem ["fullname"].ToString ());
			if (userId != activeUser.userId && !myListOfUsers.Exists(x => x.userId == userId)) {

				myListOfUsers.Add (new MoodleUser (int.Parse (userItem ["id"].ToString ()), userName, int.Parse (userItem ["lastaccess"].ToString ())));
			}
		}


	}



	///Fetch Database linkes stuff (this is for Multiplayer)
	/// 
	/// 
	/// 

	async Task FetchListOfDatabasesByCourseId(int courseId) {
		Debug.Log("json");

		string newString = await moodleObject.GetComponent<MoodleAPI> ().FetchDatabasesInCourse (activeUser.userToken, courseId);

		JSONObject encodedString =  new JSONObject(newString);

		string fieldName;

		Debug.Log (encodedString);


		foreach (JSONObject databaseItem in encodedString["databases"].list) {

			//we only proceed if the database has the name we need
			if (databaseItem ["name"].ToString ().Contains ("ControlDB_Host") || databaseItem ["name"].ToString ().Contains ("ControlDB_Extend") || databaseItem ["name"].ToString ().Contains ("DuelDB")) {
				//Debug.Log ("found " + databaseItem ["name"].ToString ());
			} else {
				//if we don't find the right name, we don't touch the database
				//Debug.Log("we skip database "+databaseItem ["name"].ToString ());
				continue;
			}


			MoodleDatabase newDatabase = new MoodleDatabase (int.Parse (databaseItem ["id"].ToString ()), int.Parse (databaseItem ["course"].ToString ()), databaseItem ["name"].ToString ());

			//not very elegant, but we have to fetch the fields for every database, and here it is most efficient
			Debug.Log("json");

			newString = await moodleObject.GetComponent<MoodleAPI> ().FetchFieldsInDatabases (activeUser.userToken, newDatabase.databaseId);

			JSONObject encodedString2 =  new JSONObject(newString);

			//Debug.Log ("my fields: "+encodedString2);

			foreach (JSONObject fieldItem in encodedString2["fields"].list) {
				//Debug.Log ("i am in foreach");
//				newDatabase.AddFieldToRecordZero (
//									int.Parse(fieldItem["id"].ToString()), 													
//									int.Parse(fieldItem["dataid"].ToString()),													
//									fieldItem["name"].ToString(), 
//									fieldItem["type"].ToString());

				fieldName = fieldItem ["name"].ToString ();
				fieldName = fieldName.Substring(1, fieldName.Length - 2);
				newDatabase.dbField.Add (fieldName, int.Parse (fieldItem ["id"].ToString ()));

//				Debug.Log (int.Parse (fieldItem ["id"].ToString ()));
//				Debug.Log (int.Parse (fieldItem ["dataid"].ToString ()));
//				Debug.Log (fieldItem["name"].ToString());
//				Debug.Log (fieldItem["type"].ToString());

			}


			dataController.myListofDatabases.Add (newDatabase);

			//Debug.Log ("i Print Database");
			//newDatabase.PrintDatabase ();

		}

		//yield return new WaitForSeconds (0.1f);
		//Debug.Log ("done building databases");

	}










	// Here I build Stuff with Data I fetched via Json
	/// <summary>
	/// Builds the list of quizes for course.
	/// </summary>
	/// <param name="courseId">Course identifier.</param>
	/// 
	/// 
	/// 
	async Task BuildListOfDatabasesForAllCourses() {

		foreach (MoodleCourse courseItem in dataController.myListofCourses) {
		
			await FetchListOfDatabasesByCourseId (courseItem.courseId);
		
		}
		Debug.Log ("i startet buidling process for all courses");
	}


	async Task BuildListOfQuizesForCourse(int courseId) {

		foreach (MoodleQuiz quizItem in dataController.GetCourseByID(courseId).myQuizzes) {
			Debug.Log ("i fetch question");
			quizItem.numberOfQuestions = await FetchQuestionsForQuiz (courseId, quizItem.quizID);

		}	


	}


	async Task BuildListOfQuestionsForQuizId(int courseId, int quizId, int attemptId, int attemptUniqueId, int pageNr) {



		Debug.Log ("i build list of questions for course: "+courseId+" quizid "+quizId+" attempt "+attemptId+" pagenr"+pageNr);

		int cursorStartPosition;
		int cursorEndPosition;
		int checkPosition;

		int questionNr = 0;
		int numberOfQuestions;
		int answerNr = 0;
		int numberOfAnswers;

		//Dictionary<string,string> nameValueSet = new Dictionary<string, string> ();




		List<NameValueSet> myNameValueSet = new List<NameValueSet> ();

		string cutString;
		MoodleQuestion newQuestion;


		JSONObject questionsArray;


		//Debug.Log("my "+dataController.myActiveGame.randomGameSequence+" her "+dataController.myActiveGame.herRandomGameSequence);

		Debug.Log("json");

		string newString = await moodleObject.GetComponent<MoodleAPI>().FetchQuizQuestionsByPage(activeUser.userToken, attemptId, pageNr);

		Debug.Log ("got the string");
		Debug.Log (newString);

		JSONObject encodedString = new JSONObject(newString);

		encodedString = encodedString["questions"];

		//Debug.Log (encodedString);
		//
		numberOfQuestions = encodedString.list.Count;

		//Debug.Log("questionNr: "+questionNr+" numberOfQuestions: "+numberOfQuestions);


		foreach (JSONObject questionItem in encodedString.list) {
			//questionNr = int.Parse (encodedString.list [0] ["slot"].ToString ());
			questionNr = int.Parse (questionItem["slot"].ToString ());
			//Debug.Log ("QuestionNr is "+questionNr);

			NameValueSet checkNameValueSet = new NameValueSet ("quizQuestion", "<div class=\\\"qtext\\\">", "<\\/div>");

			newQuestion = new MoodleQuestion (ParseAndReturnValueFromString (checkNameValueSet, questionItem.ToString ()));

			newQuestion.questionNr = questionNr;
			newQuestion.attemptId = attemptId;
			newQuestion.pageNr = pageNr;
			newQuestion.attemptUniqueId = attemptUniqueId;

			newQuestion.answered = !questionItem.ToString().Contains("<div class=\\\"state\\\">Unvollst\\u00e4ndig<\\/div>");

			newQuestion.successValue = questionItem.ToString().Contains("\"state\":\"gradedright\"");

			//Debug.Log("my succes value is "+newQuestion.successValue);

			checkNameValueSet = new NameValueSet ("sequencecheck", "name=\\\"q" + attemptUniqueId + ":" + questionNr + "_:sequencecheck\\\" value=\\\"", "\\\" \\/>");
			newQuestion.sequenceCheckValue = ParseAndReturnValueFromString (checkNameValueSet, questionItem.ToString ());

			numberOfAnswers = (questionItem.ToString ().Length - questionItem.ToString ().Replace ("answernumber", "").Length) / "answernumber".Length;

			checkNameValueSet = new NameValueSet ("q"+attemptUniqueId+":"+questionNr+"_-submit", "q"+attemptUniqueId+":"+questionNr+"_-submit\\\" value=\\\"", "\\\" class=\\\"submit btn\\\" \\/>");

			newQuestion.submitValue = ParseAndReturnValueFromString (checkNameValueSet, questionItem.ToString ());


			for (answerNr = 0; answerNr < numberOfAnswers; answerNr++) {
				//Debug.Log ("i am in answers "+answerNr);

				checkNameValueSet = new NameValueSet ("answer" + answerNr, "_answer"+answerNr, "<\\/span>", "<\\/label> <\\/div>");
				newQuestion.quizAnswers.Add (ParseAndReturnValueFromString (checkNameValueSet, questionItem.ToString ()));
				//myNameValueSet.Add(new NameValueSet("answer"+answerNr, "answer"+answerNr+"\\\" class=\\\"m-l-1\\\"><span class=\\\"answernumber\\\">", "<\\/span>", "<\\/label> <\\/div>"));

			}

			//Here we make sure that only answers which fit in our scheme are added

			//we only allow answers with the "answer"-Tag, which means they are single choice, not multiple choice
			if (questionItem.ToString ().Contains ("_answer") &&

				//at this point, we only support 4 answerOptions
				numberOfAnswers == 4) {

				dataController.AddQuestionsToQuiz (courseId, quizId, newQuestion);
			} else {
				//Debug.Log ("didn't add question");
			}
		}

		Debug.Log ("finished building list for quiz "+quizId);

		//dataController.GetQuizByCourseByID (courseId, quizId).PrintQuiz ();




	}



	///######################################
	//here we have the result logic:
	//1. transmit what i have chosen
	//2. ask, if it was right
	//3. delete the answer so we can redo the same question again


	async Task TransmitChosenQuestion(int buttonId, int finishAttempt) {
		Debug.Log ("TransmitChosenQuestion");
//		dataController.AddChosenAnswerToQuestion (buttonId);
//		//I don't raise the active question just now
		//Debug.Log("you chose button "+buttonId);
		MoodleQuestion activeQuestion = dataController.ActiveQuestion (false);

		//activeQuestion.PrintQuestion ();
		Debug.Log("json");
		string newString = await moodleObject.GetComponent<MoodleAPI>().TransmitUsersAnswer(
			activeUser.userToken,
			activeQuestion.attemptId,
			"q"+activeQuestion.attemptUniqueId +":"+activeQuestion.questionNr+"_answer",
			buttonId.ToString(),
			"q"+activeQuestion.attemptUniqueId+":"+activeQuestion.questionNr+"_:sequencecheck",
			activeQuestion.sequenceCheckValue,
			"q"+activeQuestion.attemptUniqueId+":"+activeQuestion.questionNr+"_-submit",
			"Pr%C3%BCfen", finishAttempt);
		JSONObject encodedString = new JSONObject(newString);


		// here we could catch exception "attemptalreadyclosed"

			Debug.Log(encodedString.ToString());
	}


	async Task TransmitResultOfQuestionToServer(int entryId, string linkId) {
		Debug.Log ("TransmitResultOfQuestionToServer");
		MoodleDatabase duelDB = dataController.GetDbByName ("DuelDB");
		string randomizedQuestions = dataController.myActiveGame.randomGameSequence;

		JSONObject encodedString;


		if (entryId != 0) {

			//Write line to Duel_Entry
			Debug.Log ("json");
			Debug.Log ("write randomized questions " + randomizedQuestions);
			string newString = await moodleObject.GetComponent<MoodleAPI> ().TransmitAndUpdateAnswerSet1 (
				                   activeUser.userToken, entryId,
				                   duelDB.dbField ["Answers_Set"].ToString (), randomizedQuestions);
			encodedString = new JSONObject (newString);
			if (encodedString.HasField ("exception") && encodedString ["exception"].ToString ().Contains ("dml_missing_record_exception")) {
				Debug.Log ("I couldn't update, so I make a new line, error with entryid " + entryId);

				await WriteNewLineToDuelEntryDB (randomizedQuestions, linkId, 0, dataController.activeCourseId);
				Debug.Log ("just wrote line in dueldb with linid " + linkId);
		
			}
			Debug.Log (encodedString);
		} else {
		
			Debug.Log ("wanted to write, but entryId is 0");
		}
	}


	async Task<bool> FetchResultForQuestionId(int buttonId, int attemptClosed) {
		Debug.Log ("FetchResultForQuestionId");
		//we are still in active question and we don't raise it
		MoodleQuestion activeQuestion = dataController.ActiveQuestion (false);

		JSONObject encodedString;

		string newString;

		Debug.Log("json");
		if (attemptClosed == 0) {
			newString = await moodleObject.GetComponent<MoodleAPI> ().FetchQuizQuestionsByPage (activeUser.userToken, activeQuestion.attemptId, activeQuestion.pageNr);
		} else {
			newString = await moodleObject.GetComponent<MoodleAPI> ().FetchAttemptReview (activeUser.userToken, activeQuestion.attemptId, activeQuestion.pageNr);
		}

		encodedString = new JSONObject (newString);


		encodedString = encodedString["questions"];

		// we run through the list of questions on this page and find the one with the right id
		foreach (JSONObject questionItem in encodedString.list) {
			//Debug.Log ("one line for every q on page");
			if (questionItem ["slot"].ToString () == activeQuestion.questionNr.ToString ()) {
				if (questionItem.ToString ().Contains ("gradedright")) {
					Debug.Log ("I answered " + activeQuestion.questionNr.ToString () + " correctly");
					return true;
				} else {
					
					Debug.Log ("I didn't answer "+dataController.myActiveGame.randomQuestionNr+" "+dataController.myActiveGame.questionNr+ " correctly");
					return false;
				}
			}
		}
		return false;
	}

	async void SetQuestionUpForRedo(int buttonId, int finishAttempt) {
		Debug.Log ("SetQuestionUpForRedo");
		//		dataController.AddChosenAnswerToQuestion (buttonId);
		//		//I don't raise the active question just now
		Debug.Log("i am in Redo");
		MoodleQuestion activeQuestion = dataController.ActiveQuestion (false);

		//activeQuestion.PrintQuestion ();
		Debug.Log("json");
		string newString = await moodleObject.GetComponent<MoodleAPI>().TransmitUsersAnswer(
			activeUser.userToken,
			activeQuestion.attemptId,
			"q"+activeQuestion.attemptUniqueId+":"+activeQuestion.questionNr+"_answer",
			buttonId.ToString(),
			"q"+activeQuestion.attemptUniqueId+":"+activeQuestion.questionNr+"_:sequencecheck",
			(int.Parse(activeQuestion.sequenceCheckValue)+1).ToString(),
			"redoslot"+activeQuestion.questionNr,
			"Versuchen Sie eine weitere Frage wie diese hier.", finishAttempt);
		JSONObject encodedString = new JSONObject(newString);

		//Debug.Log("I reset the question");

		if (encodedString ["exception"]) {
			Debug.Log (encodedString["exception"]);
			Debug.Log ("something went wrong, sequencecheckvalue was "+activeQuestion.sequenceCheckValue);
		}

	}


	///######################################
	//here we have some parsing logic


	string ParseAndReturnValueFromString(NameValueSet newNameValueSet, string newStringToParse) {
		int startCursorPosition;
		int endCursorPosition;

		//we check first if we find the startTag at all. If not, we return error
		if (newStringToParse.IndexOf (newNameValueSet.startTag) != -1) {

			if (newNameValueSet.searchTag != newNameValueSet.startTag) {
//				Debug.Log ("we have to use searchtag on " + newNameValueSet.name);
//				Debug.Log (newStringToParse.IndexOf (newNameValueSet.searchTag));
				startCursorPosition = newStringToParse.IndexOf (newNameValueSet.searchTag) + newNameValueSet.searchTag.Length;
				//we cut everything away before the found string
				newStringToParse = newStringToParse.Substring (startCursorPosition, newStringToParse.Length - startCursorPosition);
//				Debug.Log ("after cutting");
//				Debug.Log (newStringToParse);
			}

			startCursorPosition = newStringToParse.IndexOf (newNameValueSet.startTag) + newNameValueSet.startTag.Length;
			//we cut everything away before the found string
//			Debug.Log ("startcursorpostion " + startCursorPosition);
			newStringToParse = newStringToParse.Substring (startCursorPosition, newStringToParse.Length - startCursorPosition);

//			Debug.Log ("after cutting");
//			Debug.Log (newStringToParse);

			endCursorPosition = newStringToParse.IndexOf (newNameValueSet.endTag);
//			Debug.Log ("endcursorpostion " + endCursorPosition);

			//Debug.Log (newStringToParse);
			newStringToParse = newStringToParse.Substring (0, endCursorPosition);

//			Debug.Log ("i return value " + newStringToParse);

			return clean(newStringToParse);
		} 
		else {
			Debug.Log ("didn't find searchtag in string: "+newNameValueSet.searchTag);
			return "didn't find searchtag in string";
		}

	
	}
		

	int ParserReturnNumberOfQuestions(string encodedString) {

		//we delete all Pages from String
		encodedString = encodedString.Replace (",0", "");

		//and count the questions
		return encodedString.Split(',').Length;
	}

	int ParserReturnNumberOfPages(string encodedString) {

		encodedString = encodedString.Replace (",0", "@");

		//0 identifies Pages
		//as every Set finishes with 0, we have to substract 1
		return encodedString.Split ('@').Length-1;
	}


	//	#####################

	// I create Buttons for every course i have where we have at least on quiz

	//	#####################


	void CreateButtonsForListOfCourses() {
		Debug.Log ("CreateButtonsForListOfCourses");
		int i = 0;
		foreach(MoodleCourse courseItem in dataController.myListofCourses) 
		{
			if (courseItem.courseNumberOfQuizzes > 0) {

				newCourseButton = Instantiate<GameObject> (CourseListItem, CourseContentPanel.transform, false);
				newCourseButton.GetComponent<CourseItemController> ().SetLabel (courseItem.courseId, courseItem.courseName, courseItem.enrolledUserCount, courseItem.courseNumberOfQuizzes);
				newCourseButton.name = newCourseButton.GetInstanceID().ToString ();
				newCourseButton.GetComponent<Button> ().onClick.AddListener (delegate {OnClickCourseButton(courseItem.courseId, newCourseButton.name);});

				i++;
			}
		}
		if (i == 0) {
			Debug.Log ("no courses with quizzes in list, register as a different user");

			activePanelId = 0;
			//SwitchPanel (activePanelId);
			Debug.Log ("just switched to panel " + activePanelId);

		}
	}


	//	#####################

	// I create Buttons for every Quiz where we have at least on Question

	//	#####################

	async Task CreateButtonsForListOfQuizes(int courseId) {
		bool containsYellowTab = false;
		//create Tab if not there

		int newButtonPosition = 0;

		foreach (Transform child in QuizContentPanel.transform) {
			
			if (child.transform.name.Contains(YellowTab.name)) {
			
				containsYellowTab = true;

			}

			if (child.transform.name.Contains(GreenTab.name)) {
				break;
			}
			newButtonPosition++;
			
		}
		if (!containsYellowTab) {
			CreateAvailableQuizzesTab (newButtonPosition);
			containsYellowTab = true;
			newButtonPosition++;
		}


		Debug.Log ("CreateButtonsForListOfQuizes");
		foreach (MoodleQuiz quizItem in dataController.GetCourseByID(courseId).myQuizzes) {
			//Debug.Log ("i try to create a button for quiz id "+quizItem.quizID);

			if (quizItem.preferedFeedbackBehaviour.Contains("immediatefeedback")) {
				newQuizButton = Instantiate<GameObject> (QuizListItem, QuizContentPanel.transform, false);

//			Debug.Log ("my quiz button");
//			Debug.Log (courseId);
//			Debug.Log (quizItem.quizID);


			//create label

				newQuizButton.name = newQuizButton.GetInstanceID ().ToString ();
				newQuizButton.transform.SetSiblingIndex(newButtonPosition);
				newQuizButton.GetComponent<QuizItemController> ().SetLabel (quizItem.quizID, quizItem.quizName, "");
				newQuizButton.GetComponent<Button> ().onClick.AddListener (delegate {OnClickQuizButton(courseId, quizItem.quizID, "");});
			//
			//quizItem.PrintQuiz();
			}
		}
	}

	void CreateButtonForChallenge(int courseId, int quizId, string name, int duelEntryId, string linkId, int challengerId, int status, bool asSubject) {

		Debug.Log ("CreateButtonForChallenge");


		//when we create a button, we first check if we need a tab as well (this is the case when it's the first Button of a category

		int newButtonPosition = 0;

		bool containsRedTab = false;
		bool containsGreenTab = false;
		foreach (Transform child in QuizContentPanel.transform) {

			if ((child.transform.name.Contains(RedTab.name))) {
				containsRedTab = true;
				newButtonPosition++;
			}

			if (child.transform.name.Contains(GreenTab.name)) {
				containsGreenTab = true;
			}


		}

		if (!containsRedTab && status == 0) {
			CreateOpenGameTab ();
			newButtonPosition++;
		}

		if (!containsGreenTab && status == 1) {
			CreateEndedGamesTab ();
		}



		MoodleQuiz quizItem = dataController.GetQuizByCourseByID (courseId, quizId);
		string labelString;
		string labelStringTag = "";


		if (status == 1) {
			newButtonPosition = QuizContentPanel.transform.childCount;
		}

		if (!asSubject) {
			labelString = labelStringTag+"Gegen " + name;
		} else {
			labelString = labelStringTag+"Gegen " + name;
		}
		
		newQuizButton = Instantiate<GameObject> (OpenGamesButton, QuizContentPanel.transform, false);
		newQuizButton.transform.SetSiblingIndex (newButtonPosition);
		newQuizButton.gameObject.name = linkId;
		newQuizButton.GetComponent<QuizItemController> ().SetLabel (quizItem.quizID, labelString, quizItem.quizName);
		//newQuizButton.GetComponent<QuizItemController> ().SetLabel (quizItem.quizID, labelString, newQuizButton.name);
		newQuizButton.GetComponent<Button> ().onClick.AddListener (delegate {OnClickChalllengeButton(courseId, quizItem.quizID, duelEntryId, linkId);});
	
	}


	void CreateButtonsForListOfUsers(int courseId, int quizId) {
		Debug.Log ("CreateButtonsForListOfUsers");
		foreach (MoodleUser userItem in myListOfUsers) {
			//Debug.Log ("i try to create a button for quiz id "+quizItem.quizID);

			//if (userItem.preferedFeedbackBehaviour.Contains("immediatefeedback")) {
			newUserButton = Instantiate<GameObject> (UserListItem, UserContentPanel.transform, false);

				//			Debug.Log ("my quiz button");
				//			Debug.Log (courseId);
				//			Debug.Log (quizItem.quizID);


			//create label
			newUserButton.GetComponent<UserItemController> ().SetLabel (userItem.userId, userItem.fullName, userItem.userName);
			newUserButton.name = newUserButton.GetInstanceID().ToString ();
			newUserButton.GetComponent<Button> ().onClick.AddListener (delegate {OnClickUserButton(quizId, userItem.userId, courseId, newUserButton.name);});
			//
			//quizItem.PrintQuiz();

		}
	}





	///######################################
	/// onClick Button Logic
	/// 
	/// 1. course button
	/// 2. quiz button
	/// 3. question button
	/// 4. user button



	void OnClickCourseButton(int courseId, string courseName) {

		//ProceedFromListOfCoursesToListOfQuizzes (courseId);

	}



	void OnClickQuizButton(int courseId, int quizId, string buttonName) {

		Debug.Log ("button clicked with course: "+courseId+" and quiz:"+quizId);
		Debug.Log (buttonName);


		ProceedFromListofQuizzesToTheFirstQuestion (courseId, quizId, buttonName);


	}




	async Task OnClickChalllengeButton(int courseId, int quizId, int duelEntryId, string linkId) {

		//I check duelDB to understand the status of the game
		//if it's my turn, I proceeed
		Debug.Log ("onclickchallengebutton"+linkId);




		ShowWaitingPanel (true);


		OpenGameClass newGame = await FetchDataFromDuellDB (quizId,duelEntryId,linkId, courseId);

		Debug.Log("my "+newGame.randomGameSequence+" her "+newGame.herRandomGameSequence);
		Debug.Log ("dueentryid "+newGame.duelEntryId);

		if (string.IsNullOrEmpty (newGame.randomGameSequence)) {

			Debug.Log ("we write new line because random sequence was empty");

			//we read her sequence and reset the results
			newGame.randomGameSequence = dataController.ResetRandomGameSequence (newGame.herRandomGameSequence);
			//we write the line with our gamesequence and save the duelentryid to our active game
			newGame.duelEntryId = await WriteNewLineToDuelEntryDB (newGame.randomGameSequence, newGame.link_Id, newGame.status, courseId);
		}

		//Debug.Log("my "+newGame.randomGameSequence+" her "+newGame.herRandomGameSequence);

		dataController.myActiveGame = null;

		dataController.myActiveGame = newGame;
		dataController.myActiveGame.buttonName = linkId;

		//Debug.Log("my "+dataController.myActiveGame.randomGameSequence+" her "+dataController.myActiveGame.herRandomGameSequence);

		//start the questions
		await ProceedFromChallengeToFirstQuestion (courseId, quizId);
		Debug.Log ("after proceed");

	}

	public void OnClickProceedToQuestions() {

		ShowWaitingPanel (true);

		ProceedToQuestionsSequence ();
	}



	//button on stoppover
	public async Task ProceedToQuestionsSequence() {
		Debug.Log (" ProceedToQuestionsSequence");

		unfinishedPackage = true;
		if ((dataController.myActiveGame.randomQuestionNr < dataController.randomQuizSize) &&  (dataController.myActiveGame.singlePlayerMode || IsItMyTurn (dataController.myActiveGame.randomGameSequence, dataController.myActiveGame.herRandomGameSequence))) {

			//if we don't have a line in duel_DB on our own

			if (string.IsNullOrEmpty (dataController.myActiveGame.randomGameSequence)) {
				//we read her sequence and reset the results
				dataController.myActiveGame.randomGameSequence = dataController.ResetRandomGameSequence (dataController.myActiveGame.herRandomGameSequence);
				//we write the line with our gamesequence and save the duelentryid to our active game
				dataController.myActiveGame.duelEntryId = await WriteNewLineToDuelEntryDB (dataController.myActiveGame.randomGameSequence, dataController.myActiveGame.link_Id, dataController.myActiveGame.status, dataController.activeCourseId);
			}

			//we haven't raised the Nr before we went into Pause, so we have to do it here.
			//but only if this is not the first step
//			if (dataController.myActiveGame.randomQuestionNr != 0) {
//				dataController.myActiveGame.randomQuestionNr++;
//			}
			//await RetrieveNextQuestion (true);

			DisplayQuestion (dataController.ActiveQuestion(false));

			unfinishedPackage = true;
			//we skip one panel, we don't need the startButton here. Or do we?

			//we go to "Fragen"
			ShowWaitingPanel(false);

			SwitchPanel (5);

		} 
		//not my turn in the game, so back to list of quizzes
		else if (AmIFinished(dataController.myActiveGame.randomGameSequence)) {
			Debug.Log ( dataController.myActiveGame.buttonName );


			foreach (Transform child in UserContentPanel.transform) {
				GameObject.Destroy(child.gameObject);
			}

			foreach (Transform child in QuizContentPanel.transform) {
				Debug.Log (child.gameObject.name);
				if (child.gameObject.name == dataController.myActiveGame.buttonName) {
					
					Debug.Log ("found the name");
					child.SetAsLastSibling ();
				}
			}


			ShowWaitingPanel(false);
			SwitchPanel (3);

			// i reset active game
			dataController.myActiveGame = null;
			dataController.myListOfRandomQuestions.Clear ();


		}	
		else {

			Debug.Log ("not my turn in the game, so back to list of quizzes");

			// i reset active game
			dataController.myActiveGame = null;
			dataController.myListOfRandomQuestions.Clear ();

			foreach (Transform child in UserContentPanel.transform) {
				GameObject.Destroy(child.gameObject);
			}



			ShowWaitingPanel(false);
			SwitchPanel (3);




		}
	}




	async Task<OpenGameClass> FetchDataFromDuellDB(int quizId, int duelEntryId, string linkId, int courseId) {
		Debug.Log (linkId);
		//Debug.Log ("FetchDataFromDuellDB");	

		MoodleDatabase duelDB = dataController.GetDbByNameAndByCourseId ("DuelDB", courseId);

			JSONObject encodedString;
			//load the necessary data
		Debug.Log("json with linId "+linkId);

		//dataController.PrintListOfDB ();

		string newString = await moodleObject.GetComponent<MoodleAPI> ().FetchRecordsInDB (
			                   activeUser.userToken, duelDB.databaseId,
			                   duelDB.dbField ["Link_ID"], linkId);
		
		encodedString = new JSONObject(newString);

			Debug.Log (encodedString);

			string content = "";

			OpenGameClass newGame = new OpenGameClass ();

			//we fill the GameClass with everything we know
			newGame.quizId = quizId;
			//newGame.duelEntryId = duelEntryId;
			newGame.link_Id = linkId;

	
			foreach (JSONObject entriyItem in encodedString["entries"].list) {
			//Debug.Log ("read entry "+entriyItem["id"].ToString());
			OpenGameClass tempGame = new OpenGameClass ();
				//we need the answers_set1, where the sequence of questions and a status value is saved
				//we want to know if the entry is relevant to us
				//for this, we have to know who wrote this entry
				//and if the person who wrote it (me or her) has wrote something newer
				//if it was her


			//first we have to check if its the right linkId

			foreach (JSONObject recordItem in entriyItem["contents"].list) {
				//Debug.Log ("item in contents");
				string field = recordItem ["fieldid"].ToString (); 
				content = recordItem ["content"].ToString ().Replace ("\"", "");
				//.Substring (1, recordItem ["fieldid"].ToString ().Length - 2);
				//Debug.Log (field);

				if (int.Parse (field) == duelDB.dbField ["Link_ID"]) {
					tempGame.link_Id = content;
				}


				//for the answer-set, we have to differentiate again
				//is it her random sequence or mine
				if (int.Parse (field) == duelDB.dbField ["Answers_Set"]) {
					if (int.Parse(entriyItem["userid"].ToString()) != activeUser.userId) {
						//Debug.Log ("found her sequence and it was "+content);
						tempGame.herRandomGameSequence = content;
					}
					else {
						//Debug.Log ("found my sequence and it was "+content);
						tempGame.randomGameSequence = content;
					}
				}
				if (int.Parse (field) == duelDB.dbField ["Status"]) {
					tempGame.status = int.Parse(content);
				}
			}

		
			//we ran through the content first. Only if it's the right one, we 

				if (int.Parse(entriyItem["userid"].ToString()) != activeUser.userId) {
					//we check her timestamp
					//if the time we saved is smaller (or null) than the timestamp we have
					//we save the new timestamp
				if (tempGame.herTimeModified < int.Parse(entriyItem["timemodified"].ToString())) {
					tempGame.herTimeModified = int.Parse (entriyItem ["timemodified"].ToString ());
						//Debug.Log ("we go on to fetch the data of this entry");
					}
					//if this entry is older than the one we have, we delete it
					else {
						//Debug.Log ("i delete entry");
					newString = await moodleObject.GetComponent<MoodleAPI> ().DeleteEntry (
						activeUser.userToken, int.Parse (entriyItem ["id"].ToString ()));
					encodedString = new JSONObject (newString);
						continue;
					}
				}
				//we can be sure that it must be our record then
				else {
					//we check our timestamp
					//if the time we saved is smaller (or null) than the timestamp we have
					//we save the new timestamp
				if (tempGame.myTimeModified < int.Parse(entriyItem["timemodified"].ToString())) {
					tempGame.myTimeModified = int.Parse (entriyItem ["timemodified"].ToString ());

						//we are challenged, so we want the right entry id, which is not the one we got from dbHost
						//Debug.Log("duelentryid before: "+newGame.duelEntryId);
					tempGame.duelEntryId = int.Parse (entriyItem ["id"].ToString ());
					//Debug.Log("duelentryid after: "+tempGame.duelEntryId);
						//Debug.Log ("we go on to fetch the data of this entry");
					}
					//if this entry is older than the one we have, we go on without doing anything
					else {
						continue;
					}
				}
			//update Game with information from entry
			//newGame.PrintGame();
			//tempGame.PrintGame ();
			newGame.UpdateInformationFromDuelDB (tempGame);
		}

//		// I have to forsee something if entries comes back empty
		if (encodedString["entries"].list.Count < 1) {
			Debug.Log ("didn't find no entries");
			newGame = null;

		}

		//Debug.Log("my "+newGame.randomGameSequence+" her "+newGame.herRandomGameSequence);

		//we have to make sure, 
		if (newGame.randomGameSequence.Length > 0 || newGame.herRandomGameSequence.Length > 0) {
			
			string[] splitString1 = newGame.randomGameSequence.Split ('@');
			string[] splitString2 = newGame.herRandomGameSequence.Split ('@');

			if (((splitString1.Count () == 1) || (splitString1.Count () == dataController.randomQuizSize))
				&& ((splitString2.Count () == 1) || (splitString2.Count () == dataController.randomQuizSize))) {

			} else {
				Debug.Log ("i have to return game null");
				newGame = null;
			}


		} else {
			Debug.Log ("no random string bigger than 0");
			//newGame = null;
		}



		Debug.Log ("end of Function with duel entry "+newGame.duelEntryId);

		//newGame.PrintGame ();

		return newGame;

	}


	/// <summary>
	/// Determines whether it is my turn by comparing the randomquestion-Strings
	/// </summary>
	/// <returns><c>true</c> if this instance is it my turn the specified myRandomQuestions herRandomQuestions; otherwise, <c>false</c>.</returns>
	/// <param name="myRandomQuestions">My random questions.</param>
	/// <param name="herRandomQuestions">Her random questions.</param>

	public bool IsItMyTurn(string myRandomQuestions, string herRandomQuestions) {
		//Debug.Log (" IsItMyTurn");
		//if I don't have a string yet, it's my turn
		if (string.IsNullOrEmpty( myRandomQuestions)) {
			return true;
			//if I have a string but she doesn't yet
			//I use a dummy string where every question is 0
			//this is necessary, because I might not have answered yes as well, and I should be able to anser the questions
		} else if (string.IsNullOrEmpty( herRandomQuestions)) {
			herRandomQuestions = "7,0@3,0@4,0@1,0@0,0@6,0@5,0@2,0@8,0";
		}
		string[] string1 = myRandomQuestions.Split('@');
		string[] string2 = herRandomQuestions.Split('@');
		int i = 0;
		int j = 0;

		foreach (string stringItem in string1) {
			if (int.Parse( string1 [i].Substring (string1 [i].Length - 1, 1)) != 0) {
				i++;
			}
			if (int.Parse( string2 [j].Substring (string2 [j].Length - 1, 1)) != 0) {
				j++;
			}
		}
		///we might have incomplete packages, so we count in two.
		/// this means: if I have answered only one question and the other one 0, I still can go on.
		/// if i have answered 2 and the other one 0, I have one more question as well.
		if (i <= j+2) {
			return true;
		} else {
			return false;
		}
	}

	public bool AmIFinished(string myRandomQuestions) {
		Debug.Log ("AmIFinished with string "+myRandomQuestions);

		if (string.IsNullOrEmpty (myRandomQuestions)) {
			return false;
		}


		string[] randomString =  myRandomQuestions.Split('@');
		bool isFinished = true;

		foreach (string stringItem in randomString) {
			if (int.Parse (stringItem.Substring (stringItem.Length - 1, 1)) == 0) {
				isFinished = false;
			}
		}
		return isFinished;
	}


	public void ButtonPressed(int myButton) {

		Debug.Log ("i hit button "+myButton);
		Debug.Log ("dealing with question "+dataController.myActiveGame.randomQuestionNr+" "+dataController.myActiveGame.questionNr);

		if (!WaitPanel.activeSelf) {

			//		if (myButton == myQuestions.myQuestions [numberOfQuestion-1].rightAnswer) {
			//			myQuestions.myQuestions [numberOfQuestion-1].succeeded = true;
			//		}
			ShowWaitingPanel (true);

			FetchRightResultAndDisplayNextQuestion (myButton);
		} else {
			Debug.Log ("I am out of work currently");
		}


	}


	public void OnClickUserButton(int quizId, int userId, int courseId, string buttonName) {

		CreateChallenge (userId, courseId);
	
		//challengedUserId = userId;
	}


	///######################################
	/// here we have some UI Logic


	public void GoToNexPanel() {
	
		activePanelId++;
		SwitchPanel (activePanelId);
		Debug.Log ("just switched to next Panel "+activePanelId);
	
	}


	public async Task ShowWaitingPanel(bool isActive) {
		
		//Debug.Log ("now in show panel");

		if (isActive) {
			
			redSpinner.GetComponent<RedSpinnerController> ().AddListener ();

			if (!WaitPanel.activeSelf) {
				WaitPanel.SetActive (isActive);;
			}

		} else {
			//Debug.Log ("i try to delete listener");

			if (redSpinner.GetComponent<RedSpinnerController> ().listener == 1) {
				//Debug.Log ("i turn off panel");
				redSpinner.GetComponent<RedSpinnerController> ().DeleteListener();
				redSpinner.GetComponent<RedSpinnerController> ().LenghtOfActivity ();
				WaitPanel.SetActive (isActive);
			} else if (redSpinner.GetComponent<RedSpinnerController> ().listener > 1)  {
				//Debug.Log ("i just delete one listener");
				redSpinner.GetComponent<RedSpinnerController> ().DeleteListener();
			}
			else {
				Debug.Log ("this should never happen, listenr below 0");
			}

		}

		while (WaitPanel.activeSelf && (redSpinner.GetComponent<RedSpinnerController> ().LenghtOfActivity () < 10f)) {

			await new WaitForSeconds (0.2f);
		}
		if (WaitPanel.activeSelf && (redSpinner.GetComponent<RedSpinnerController> ().LenghtOfActivity () >= 10f)) {


			WaitPanel.SetActive(false);

			dataController.myActiveGame = null;

			SwitchPanel(3);


		}

	}

		

	public void DisplayQuestion(MoodleQuestion question) {

		Debug.Log ("DisplayQuestion");
		//dataController.PrintActiveQuestion ();

		questionText.text = question.quizQuestion;

		bool lastQuestionSuccess;


		if (question.quizAnswers.Count == 4) {

			buttonA.GetComponentInChildren<Text> ().text = question.quizAnswers [0];
			buttonB.GetComponentInChildren<Text> ().text = question.quizAnswers [1];
			buttonC.GetComponentInChildren<Text> ().text = question.quizAnswers [2];
			buttonD.GetComponentInChildren<Text> ().text = question.quizAnswers [3];

		}

	}


	public void SwitchPanel(int myPanelID) {
		//Debug.Log ("SwitchPanel");
		//when we switch panel, we have to adjust activepanelid
		activePanelId = myPanelID;

		//Register
		if (myPanelID == 1) {
			registerPage.SetActive (true);
			listOfCoursesPage.SetActive (false);
			listofQuizzesPage.SetActive (false);
			startPage.SetActive (false);
			questions.SetActive (false);
			endPage.SetActive (false);
			stopOverPage.SetActive (false);
		//List of Courses
		} else if (myPanelID == 2) {
			registerPage.SetActive (false);
			listOfCoursesPage.SetActive (true);
			listofQuizzesPage.SetActive (false);
			startPage.SetActive (false);
			questions.SetActive (false);
			endPage.SetActive (false);
			stopOverPage.SetActive (false);
		//List of Quizzes
		} else if (myPanelID == 3) {
			registerPage.SetActive (false);
			listOfCoursesPage.SetActive (false);
			listofQuizzesPage.SetActive (true);
			startPage.SetActive (false);
			questions.SetActive (false);
			endPage.SetActive (false);
			stopOverPage.SetActive (false);

			listofQuizzesPage.GetComponent<PanelController> ().SetText(activeUser.userName);

		//Start Page
		} else if (myPanelID == 4) {
			registerPage.SetActive (false);
			listOfCoursesPage.SetActive (false);
			listofQuizzesPage.SetActive (false);
			startPage.SetActive (true);
			questions.SetActive (false);
			endPage.SetActive (false);
			stopOverPage.SetActive (false);

			startPage.GetComponent<PanelController> ().SetText(activeUser.userName);
		//Questions
		} else if (myPanelID == 5) {
			registerPage.SetActive (false);
			listOfCoursesPage.SetActive (false);
			listofQuizzesPage.SetActive (false);
			startPage.SetActive (false);
			questions.SetActive (true);
			endPage.SetActive (false);
			stopOverPage.SetActive (false);

			questions.GetComponent<PanelController> ().SetText(activeUser.userName);

		//EndPage
		} else if (myPanelID == 6) {
			registerPage.SetActive (false);
			listOfCoursesPage.SetActive (false);
			listofQuizzesPage.SetActive (false);
			startPage.SetActive (false);
			questions.SetActive (false);
			endPage.SetActive (true);
			stopOverPage.SetActive (false);
		}
		//StopOver
		else if (myPanelID == 7) {
			registerPage.SetActive (false);
			listOfCoursesPage.SetActive (false);
			listofQuizzesPage.SetActive (false);
			startPage.SetActive (false);
			questions.SetActive (false);
			endPage.SetActive (false);
			stopOverPage.SetActive (true);

			stopOverPage.GetComponent<PanelController> ().SetText(activeUser.userName);
		}

	}


//	public void GoToStep(int myStep) {
//		if (myStep == 1) {
//			step1.SetActive (true);
//			step2.SetActive (false);
//			step3.SetActive (false);
//		}
//		else if (myStep == 2) {
//			step1.SetActive (false);
//			step2.SetActive (true);
//			step3.SetActive (false);
//		}
//		else if (myStep == 3) {
//			step1.SetActive (false);
//			step2.SetActive (false);
//			step3.SetActive (true);
//		}
//
//	}



	/// <summary>
	/// Retrieves the next question.
	/// </summary>
	/// <param name="dontQuit">If set to <c>true</c> dont quit.</param>
	/// 

	public async Task RetrieveNextQuestion(bool dontQuit) {

		//we have to check if we already run through all the questions

		Debug.Log ("start to retrieve next question "+dataController.myActiveGame.randomQuestionNr+" "+dataController.myActiveGame.questionNr);
		Debug.Log ("unfinished package "+unfinishedPackage);


		if (!dontQuit && dataController.myActiveGame.randomQuestionNr >= dataController.randomQuizSize) {
			//if so, we go to end page
			Debug.Log("i was told to quit");

			SetBackRedAndGreen ();


			ShowWaitingPanel (false);

//			foreach (Transform child in QuizContentPanel) {
//			
//				GameObject.Destroy (child.gameObject);
//
//			}
//
//
//			//we call this because we don't show the list of courses anymore
//			await ProceedFromListOfCoursesToListOfQuizzes ();


			SwitchPanel (7);


			stopOverButtonText.text = "Finished. Go back to Quizzes";

			stopOverPage.GetComponent<StopOverController> ().SetLights (dataController.myActiveGame.randomGameSequence, dataController.myActiveGame.herRandomGameSequence);
			//stopOverPage.G

		}
		else if (dataController.myActiveGame.randomQuestionNr % packageSize == 0 && dataController.myActiveGame.randomQuestionNr != 0 && unfinishedPackage) {

			Debug.Log ("finished Package");
			unfinishedPackage = false;

			//here I save my results to the server:
			//Debug.Log("print game");
			//dataController.myActiveGame.PrintGame ();

			//update entry on server in duelDB



			if (IsItMyTurn (dataController.myActiveGame.randomGameSequence, dataController.myActiveGame.herRandomGameSequence)) {
			
				stopOverButtonText.text = "Go to the next package";
			
			} else {

				stopOverButtonText.text = "Wait for the other one... back to quizzes";

			}



			SetBackRedAndGreen ();
			ShowWaitingPanel (false);
			SwitchPanel (7);


			stopOverPage.GetComponent<StopOverController> ().SetLights (dataController.myActiveGame.randomGameSequence, dataController.myActiveGame.herRandomGameSequence);
			//stopOverPage.GetComponent<StopOverController> ().SetLightsForPlayer2 ();


		}

		else {

			Debug.Log ("normal question getting");
			//else we call method to display next question


			//This is not necessary anymore
//			while (dataController.ActiveQuestion (false).answered) {
//				Debug.Log ("this questions is set to answered, so i raise counter:");
//				Debug.Log ("question "+dataController.myActiveGame.randomQuestionNr+" "+dataController.myActiveGame.questionNr+ " ");
//
//				//dataController.ActiveQuestion (false).PrintQuestion ();
//				dataController.RaiseActiveQuestion ();
//				if (dataController.ThisWasTheLastQuestion() == 1) {
//					break;
//				}
//			}

			ShowWaitingPanel (false);

			//Debug.Log ("after while, i use ");
			DisplayQuestion (dataController.ActiveQuestion(false));
			//Debug.Log ("i got last question");
		}

		//
		//numberOfQuestion++;


	}

	void DisplayRedOrGreenBasedOnResult(bool result) {

		int questionNr = dataController.myActiveGame.randomQuestionNr % 3;

		if (questionNr == 0) {
			if (result) {
				step1_green.SetActive (true);
			} else {
				step1_red.SetActive (true);
			}
		}
		else if (questionNr == 1) {
			if (result) {
				step2_green.SetActive (true);
			} else {
				step2_red.SetActive (true);
			}
		}
		else if(questionNr == 2) {
			if (result) {
				step3_green.SetActive (true);
			} else {
				step3_red.SetActive (true);
			}
		}

	}

	void SetBackRedAndGreen() {

		step1_green.SetActive (false);
		step1_red.SetActive (false);
		step2_green.SetActive (false);
		step2_red.SetActive (false);
		step3_green.SetActive (false);
		step3_red.SetActive (false);



	}




	/// <summary>
	/// Fetchs the right result and display next question.
	/// </summary>
	/// <returns>The right result and display next question.</returns>
	/// <param name="myButton">My button.</param>


	async Task FetchRightResultAndDisplayNextQuestion(int myButton) {

		int closeAttempt = dataController.ThisWasTheLastQuestion ();

		Debug.Log ("this is the last question: "+closeAttempt);


		await TransmitChosenQuestion (myButton, closeAttempt);




		bool result = await FetchResultForQuestionId (myButton, closeAttempt);


		//TransmitChosenQuestion (myButton, 0);

		Debug.Log ("i transmitted the question and check result");
	
		dataController.SaveResultToActiveQuestion (result);
		DisplayRedOrGreenBasedOnResult (result);



		dataController.myActiveGame.randomGameSequence = ListOfIntToJsonString (dataController.myListOfRandomQuestions);

		Debug.Log ("i set question up for redo");

		//we could transmit the result after every answer to the server
		
		//still have to find a way to transmit the entry id of dueldb

		//if we are not in singlePlayer
		if (!dataController.myActiveGame.singlePlayerMode) {
			await TransmitResultOfQuestionToServer (dataController.myActiveGame.duelEntryId, dataController.myActiveGame.link_Id);
		}

		//we don't have to await this
		if (closeAttempt == 0) {
			SetQuestionUpForRedo (myButton, 0);
		}


		if (!dataController.myActiveGame.singlePlayerMode) {
			Debug.Log ("try to update entry in duelDB with duelentryid "+dataController.myActiveGame.duelEntryId);
			await TransmitResultOfQuestionToServer (dataController.myActiveGame.duelEntryId, dataController.myActiveGame.link_Id);
			Debug.Log ("tried to update entry in duelDB");
		}


		await new WaitForSeconds (0.3f);

		//Debug.Log ("i now go on to retrieve next question");

		await RetrieveNextQuestion (dataController.RaiseActiveQuestion ());

	}


	public void EndGame() {
		Debug.Log ("i quit");
		Application.Quit();

	}


	string RetrieveNameById(int userId) {
	
		foreach (MoodleUser listItem in myListOfUsers) {
		
			if (listItem.userId == userId) {
				return listItem.fullName;
			}
		}
	
		return "anonymous";
	}


	string clean(string s)
	{

		//Debug.Log ("in cleaning");

		s= s.Replace ("\\r", " ");
		s= s.Replace ("\\n", " ");

		//Debug.Log (s);

		int startIndex;
		int endIndex;
		int length;
		string deleteString;
		startIndex = s.IndexOf ('<');
		endIndex = s.IndexOf('>');


		CleanJason decodedString = new CleanJason ();

		s = decodedString.FixEncoding (s);

		while ((startIndex != -1) && (endIndex != -1)) {
			
			//Debug.Log (s);
			length = endIndex - startIndex + 1;
			deleteString  = s.Substring (startIndex, length);
			s = s.Replace (deleteString, "");
			startIndex = s.IndexOf ('<');
			endIndex = s.IndexOf('>');
		}

		StringBuilder sb = new StringBuilder (s);

		sb.Replace("\"", "");
		sb.Replace("\\", "");
		sb.Replace(";", "");

		return sb.ToString();
	}


}
