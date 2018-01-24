using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MoodleAPI : MonoBehaviour
{

	const string baseURL = "https://moodle.linguin.at/m1";
	const string websevicesURL = "/webservice/rest/server.php?";
	const string fetchTokenURL = "/login/token.php?";

	public GameController myGameController;

	//public GameObject WaitingPanel;
	//public GameObject SpinningWheel;


	public void Start ()
		{



		}


		// Returns JSON encoded string on success or empty string, if there was a error downloading the JSON string
		// TODO: Implement a result handler that calls the calling method or any other method specified in the params after yield return www
		// or implement an event handler
	public string FetchJSON (string moodleEndpoint)
		{

		//StartCoroutine(MakeWWWCall(moodleEndpoint, (myReturnValue) => {return myReturnValue;}));

		string userURL = baseURL+moodleEndpoint;
		Debug.Log (moodleEndpoint);

		WWW www = new WWW (userURL);

				while (!www.isDone) {
						
						Debug.Log ("i am waiting");
						new WaitForEndOfFrame ();
						
				}

		//myGameController.ShowWaitingPanel (false);
				

				
				if (www.error == null) {
						//Debug.Log ("WWW Ok!: " + www.text);
						return www.text;
				} else {
						Debug.Log ("WWW Error: " + www.error);
						return "";
				} 
		}


	public async Task<string> AsyncFetchJSON (string moodleEndpoint) {
	
		string userURL = baseURL+moodleEndpoint;

		//Debug.Log (userURL);

		myGameController.ShowWaitingPanel (true);

		WWW www = await new WWW (userURL);

		while (!www.isDone) {

			//Debug.Log ("i am waiting in async");
			await new WaitForEndOfFrame ();

		}

		//Debug.Log (myGameController.redSpinner.GetComponent<RedSpinnerController>().listener);
		myGameController.ShowWaitingPanel (false);
		//Debug.Log ("i turn off panel with "+moodleEndpoint+" and listener is then "+myGameController.redSpinner.GetComponent<RedSpinnerController>().listener);

		if (www.error == null) {
			//Debug.Log ("WWW Ok!: " + www.text);
			return www.text;
		} else {
			Debug.Log ("WWW Error: " + www.error);
			return "";
		} 

	}


	public async Task<string> FetchCoreInfo (string userToken)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
							+ "wstoken="+userToken
							+"&wsfunction=core_webservice_get_site_info"
							+"&moodlewsrestformat=json");
		return userURL;

	}

	public async Task<string> FetchUserToken (string name, string pw)
	{
		string userURL = await AsyncFetchJSON (fetchTokenURL
							+"username="+name
							+"&password="+pw
							+"&service=moodle_mobile_app");
		return userURL;
	}

	public async Task<string> FetchUserCourses (string userToken, int userId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
							+"wstoken="+userToken
							+"&userid="+userId
							+"&wsfunction=core_enrol_get_users_courses"
							+"&moodlewsrestformat=json");

		return userURL;
		

	}

	public async Task<string> FetchQuizzesinCourse (string userToken, int courseId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&courseids[0]="+courseId
			+"&wsfunction=mod_quiz_get_quizzes_by_courses"
			+"&moodlewsrestformat=json");
		return userURL;


	}

	public async Task<string> FetchUsersInCourse (string userToken, int courseId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&courseid="+courseId
			+"&wsfunction=core_enrol_get_enrolled_users"
			+"&moodlewsrestformat=json");

		return userURL;


	}

	//core_user_get_users_by_field&field=id&values[0]=2
	public async Task<string> FetchUserById (string userToken, int userId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&wsfunction=core_user_get_users_by_field"
			+"&moodlewsrestformat=json"
			+"&field=id"
			+"&values[0]="+userId);
		return userURL;
		
	}



	public async Task<string> FetchDatabasesInCourse (string userToken, int courseId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&courseids[0]="+courseId
			+"&wsfunction=mod_data_get_databases_by_courses"
			+"&moodlewsrestformat=json");

		return userURL;

	}


	public async Task<string> FetchFieldsInDatabases(string userToken, int databaseId)
		{
		string userURL = await AsyncFetchJSON (websevicesURL
				+"wstoken="+userToken
				+"&databaseid="+databaseId
				+"&wsfunction=mod_data_get_fields"
				+"&moodlewsrestformat=json");
		return userURL;
		}



	public async Task<string> StartNewAttemptIdByQuiz (string userToken, int quizId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&quizid="+quizId
			+"&wsfunction=mod_quiz_start_attempt"
			+"&moodlewsrestformat=json");
		return userURL;


	}

	public async Task<string> FetchAttemptIdByQuiz (string userToken, int quizId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&quizid="+quizId
			+"&wsfunction=mod_quiz_get_user_attempts"
			+"&moodlewsrestformat=json"
			+"&includepreviews=1&status=all");
		return userURL;


	}

	//https://moodle.linguin.at/m1/webservice/rest/server.php?wstoken=9b22134105c31eb861f1dc98585eb6a0
	//&wsfunction=mod_quiz_process_attempt&moodlewsrestformat=json&attemptid=22&data[0][name]=q22:1_answer
	//&data[0][value]=0&data[1][name]=q22:1_:sequencecheck&data[1][value]=1&data[2][name]=q22:1_-submit
	//&data[2][value]=Pr%C3%BCfen&finishattempt=0&timeup=0

	public async Task<string> TransmitUsersAnswer (string userToken, int attemptid, 
										string data0Name, string data0Value,
										string data1Name, string data1Value,
										string data2Name, string data2Value,
										int finishAttempt)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&wsfunction=mod_quiz_process_attempt"
			+"&moodlewsrestformat=json"
			+"&attemptid="+attemptid
			+"&data[0][name]="+data0Name		//answer
			+"&data[0][value]="+data0Value		//
			+"&data[1][name]="+data1Name		//sequencecheck
			+"&data[1][value]="+data1Value		
			+"&data[2][name]="+data2Name		//submit name
			+"&data[2][value]="+data2Value		//Pr%C3%BCfen
			+"&finishattempt="+finishAttempt
			+"&timeup=0"
			+"&moodlewssettingfilter=true"
			+"&moodlewssettingfileurl=true");

		//Debug.Log (url);
		return userURL;
//			+"moodlewssettingfilter:true"
//			+"moodlewssettingfileurl:true"
		
	}



	/// &moodlewssettingfilter=true&returncontents=1
	///  

	public async Task<string> FetchRecordByEntryId(
		string userToken, int entryId) {
	
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&entryid="+entryId
			+"&wsfunction=mod_data_get_entry"
			+"&moodlewsrestformat=json"
			+"&moodlewssettingfilter=true"
			+"&returncontents=1");
		return userURL;
	}


	public async Task<string> TransmitChallengeToControlDB_Host (
		string userToken, int databaseId,
		string data0Name, string data0Value,
		string data1Name, string data1Value,
		string data2Name, string data2Value,
		string data3Name, string data3Value,
		string data4Name, string data4Value,
		string data5Name, string data5Value)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&databaseid="+databaseId
			+"&wsfunction=mod_data_add_entry"
			+"&moodlewsrestformat=json"
			//+"&attemptid="+attemptid
			+"&data[0][fieldid]="+data0Name		//answer
			+"&data[0][value]="+data0Value		//
			+"&data[1][fieldid]="+data1Name		//sequencecheck
			+"&data[1][value]="+data1Value		
			+"&data[2][fieldid]="+data2Name		//submit name
			+"&data[2][value]="+data2Value
			+"&data[3][fieldid]="+data3Name		//submit name
			+"&data[3][value]="+data3Value
			+"&data[4][fieldid]="+data4Name		//submit name
			+"&data[4][value]="+data4Value
			+"&data[5][fieldid]="+data5Name		//submit name
			+"&data[5][value]=\""+data5Value+"\"");
		

		//Debug.Log (url);
		return userURL;
		//			+"moodlewssettingfilter:true"
		//			+"moodlewssettingfileurl:true"

	}

	//TransmitChallengeToDuel_Entry

	public async Task<string> TransmitChallengeToDuel_Entry (
		string userToken, int databaseId,
		string data0Name, string data0Value,
		string data1Name, string data1Value,
		string data2Name, string data2Value)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&databaseid="+databaseId
			+"&wsfunction=mod_data_add_entry"
			+"&moodlewsrestformat=json"
			//+"&attemptid="+attemptid
			+"&data[0][fieldid]="+data0Name		//answer
			+"&data[0][value]=\""+data0Value+"\""		//
			+"&data[1][fieldid]="+data1Name		//sequencecheck
			+"&data[1][value]=\""+data1Value+"\""		
			+"&data[2][fieldid]="+data2Name		//submit name
			+"&data[2][value]=\""+data2Value+"\"");


		//Debug.Log (url);
		return userURL;
		//			+"moodlewssettingfilter:true"
		//			+"moodlewssettingfileurl:true"

	}

	//CheckIfChallengedByUserId


	public async Task<string> CheckIfChallengedByUserId (
		string userToken, int databaseId,
		string data0Name, string data0Value)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&databaseid="+databaseId
			+"&wsfunction=mod_data_search_entries"
			+"&moodlewsrestformat=json"
			+"&returncontents=1"
			//+"&attemptid="+attemptid
			+"&advsearch[0][name]=f_"+data0Name		//answer
			+"&advsearch[0][value]=\""+data0Value+"\"");
		
		//Debug.Log (url);
		return userURL;

	}


	public async Task<string> FetchRecordsInDB (
		string userToken, int databaseId,
		int data0Name, string data0Value)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&databaseid="+databaseId
			+"&wsfunction=mod_data_search_entries"
			+"&moodlewsrestformat=json"
			+"&returncontents=1"
			//+"&attemptid="+attemptid
			+"&advsearch[0][name]=f_"+data0Name		//answer
			+"&advsearch[0][value]="+data0Value);

			return userURL;

	}


	public async Task<string> TransmitAndUpdateAnswerSet1 (
		string userToken, int entryId,
		string data0Name, string data0Value)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&entryid="+entryId
			+"&wsfunction=mod_data_update_entry"
			+"&moodlewsrestformat=json"
			+"&data[0][fieldid]="+data0Name		//answer
			+"&data[0][value]=\""+data0Value+"\"");
		
		return userURL;
	}

	//

	public async Task<string> DeleteEntry (
		string userToken, int entryId)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+"wstoken="+userToken
			+"&entryid="+entryId
			+"&wsfunction=mod_data_delete_entry"
			+"&moodlewsrestformat=json");

		return userURL;
	}

	public async Task<string> FetchQuizQuestionsByPage (string userToken, int attemptId, int pageNr)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
		             + "wstoken=" + userToken
		             + "&attemptid=" + attemptId
		             + "&page=" + pageNr
		             + "&wsfunction=mod_quiz_get_attempt_data"
			+ "&moodlewsrestformat=json");
		//Debug.Log (url);
		return userURL;

	}

	public async Task<string> FetchAttemptReview (string userToken, int attemptId, int pageNr)
	{
		string userURL = await AsyncFetchJSON (websevicesURL
			+ "wstoken=" + userToken
			+ "&attemptid=" + attemptId
			+ "&page=" + pageNr
			+ "&wsfunction=mod_quiz_get_attempt_review"
			+ "&moodlewsrestformat=json");
		//Debug.Log (url);
		return userURL;


	}
		
}
public class NameValueSet {

	public string name;
	public string value;
	public string searchTag;
	public string startTag;
	public string endTag;

	public NameValueSet() {
	
	
	
	}

	public NameValueSet(string newName, string newStartTag, string newEndTag) {

		name = newName;
		value = "";
		startTag = newStartTag;
		//if we don't specify a searchTag, we just use the startTag
		searchTag = startTag;
		endTag = newEndTag;
	
	}

	public NameValueSet(string newName, string newSearchTag, string newStartTag, string newEndTag) {

		name = newName;
		value = "";
		startTag = newStartTag;
		searchTag = newSearchTag;
		endTag = newEndTag;

	}





}