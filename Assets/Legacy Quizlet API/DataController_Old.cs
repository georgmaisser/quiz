//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Text;
//
//
//// The DataController does the following things
//// 1. it fetches a json string from online (checks for playerprefs?)
//// 2. if not possible, there is an offline fallback
//// 3. it transfroms the json to a vocabulary list
//// 4. it stores active list to player prefs
//// 5. it handels the active vocabulary
//// 6. it writes learned information back to vocabulary list
//
//
//
//
//public class DataController : MonoBehaviour {
//
//
//	//my Data
//	string encodedString;
//
//	public VocabList myVocabList = new VocabList();
//	//public VocabGroup myVocabGroup = new VocabGroup ();
//	public Vocabulary myVocab;
//
//
//	public static List<QuizletSet> QuizletSets = new List<QuizletSet>();
//
//
//	//Communication
//	JSONObject quizletArray;
//	JSONObject term;
//	JSONObject arr;
//
//	CleanJason cleanMyJason = new CleanJason();
//
//	GameController myGameController;
//	public SetsController mySetsController;
//	public SetController mySetController;
//
//	// Use this for initialization
//	void Start () {
//
//		StartCoroutine (InitializeSequence());
//
//		//StartCoroutine(FetchMySet("33055234"));
//		//myGameController.StartGamePlay ();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//
//		//we need to wait for the return of IENumerator, we do this with the dontCheckerClass
//		if (!myGameController.myDontChecker.IWaitForWWW ()) {
//
//			//Debug.Log (encodedString);
//			//myGameController.myDontChecker.printList ();
//
//		}
//
//
//	}
//
//
//	void Initialize() {
//
//		// find Game Controller
//		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
//		if(gameControllerObject != null)
//		{
//			myGameController = gameControllerObject.GetComponent <GameController>();
//			//Debug.Log ("i found my game Controller script");
//		}
//		if(myGameController == null)
//		{
//			//Debug.Log ("Cannot find Gamecontroller Script");
//		}
//
//	}
//
//
//	public void StartButton(string myUserName) {
//
//		//FetchMyCards ("33055234");
//		
//		StartCoroutine(FetchMySets (myUserName));
//	}
//
//	public void GoToSet(string myID) {
//
//		StartCoroutine (FetchMySet (myID));
//	}
//
//
//
//
//	IEnumerator InitializeSequence() {
//
//		//we need the Gamecontroller
//		Initialize ();
//
//		yield return new WaitForSeconds (0.1f);
//			
//	}
//
//	public IEnumerator FetchMySets(string myUserName)  {
//			
//			string mySetTitle;
//			string mySetNumberOfTerms;
//			string myID;
//			string encodedString;
//			JSONObject termLine;
//	
//			//quizlette357240
//	
//			string url_api = "https://api.quizlet.com/2.0/users/"+myUserName+"?whitespace=0";
//	
//			Dictionary<string, string> dict_api =
//				new Dictionary<string, string>();
//	
//			string key = "Authorization";
//			string value = "Bearer 0e917f92f19f6611b11aea488428d2e6aa08d762";
//	
//			dict_api.Add(key,value);
//	
//			WWW download = new WWW( url_api, null, dict_api);
//			
//			yield return download;
//		
//			// check for errors
//			if (download.error == null)
//			{
//				//Debug.Log("WWW Ok!: " + download.text);
//				encodedString = download.text;
//	
//				// Debug.Log (encodedString);
//				//encodedString = Encoding.UTF8.GetString(encodedString);
//				//dontRunUpdate = false;
//			} else {
//				//Debug.Log("WWW Error: "+ download.error);
//				encodedString = "{\"username\": \"quizlette357240\",\"account_type\": \"plus\",\"profile_image\": \"https://quizlet.com/fbpic/hprofile-ak-xaf1/t31.0-1/c313.59.589.589/s320x320/287132_10150246730307531_6975532_o.jpg\",\"id\": 5774869,\"statistics\": {\"study_session_count\": 104,\"total_answer_count\": 217,\"public_sets_created\": 4,\"public_terms_entered\": 714,\"total_sets_created\": 4,\"total_terms_entered\": 714},\"sign_up_date\": 1357145409,\"sets\": [{\"id\": 33055234,\"url\": \"/33055234/master-1-flash-cards/\",\"title\": \"Master 1\",\"created_by\": \"quizlette357240\",\"term_count\": 110,\"created_date\": 1387448545,\"modified_date\": 1440443113,\"published_date\": 1387448545,\"has_images\": true,\"subjects\": [],\"visibility\": \"public\",\"editable\": \"only_me\",\"has_access\": true,\"can_edit\": true,\"description\": \"\",\"lang_terms\": \"de\",\"lang_definitions\": \"de\",\"password_use\": 0,\"password_edit\": 0,\"access_type\": 2,\"creator_id\": 5774869,\"display_timestamp\": \"In December 2013\"},{\"id\": 18069849,\"url\": \"/18069849/kroatisch-orte-flash-cards/\",\"title\": \"kroatisch Orte\",\"created_by\": \"quizlette357240\",\"term_count\": 21,\"created_date\": 1357384657,\"modified_date\": 1382731745,\"published_date\": 1357384657,\"has_images\": false,\"subjects\": [],\"visibility\": \"public\",\"editable\": \"only_me\",\"has_access\": true,\"can_edit\": true,\"description\": \"\",\"lang_terms\": \"de\",\"lang_definitions\": \"hr\",\"password_use\": 0,\"password_edit\": 0,\"access_type\": 2,\"creator_id\": 5774869,\"display_timestamp\": \"In January 2013\"},{\"id\": 18069839,\"url\": \"/18069839/kroatisch-verben-flash-cards/\",\"title\": \"kroatisch Verben\",\"created_by\": \"quizlette357240\",\"term_count\": 98,\"created_date\": 1357384450,\"modified_date\": 1386953513,\"published_date\": 1357384450,\"has_images\": false,\"subjects\": [],\"visibility\": \"public\",\"editable\": \"only_me\",\"has_access\": true,\"can_edit\": true,\"description\": \"\",\"lang_terms\": \"de\",\"lang_definitions\": \"hr\",\"password_use\": 0,\"password_edit\": 0,\"access_type\": 2,\"creator_id\": 5774869,\"display_timestamp\": false}],\"groups\": []}";
//			}
//	
//			encodedString = cleanMyJason.FixEncoding (encodedString);
//	
//
//		Debug.Log (encodedString);
//			//encodedString = cleanMyJason.FixJSONNumberBug (encodedString);
//	
//	
//			//this is how encoding should be fixed, but doesn't work
//			//byte[] bytes = Encoding.Default.GetBytes(encodedString);
//			//encodedString = Encoding.UTF8.GetString(bytes);
//	
//			quizletArray = new JSONObject(encodedString);
//	
//			JSONObject setsArray = quizletArray["sets"];
//	
//			int myCounter = setsArray.Count;
//	
//	
//			for (int k = 0; k <= myCounter; k++) {
//	
//				//and populate Buttons
//				if (setsArray[k]) {
//	
//					termLine = setsArray[k];
//
//
//					QuizletSet quizletSet = new QuizletSet();
//					
//
//
//				Debug.Log (termLine["id"].ToString());
//				Debug.Log (termLine["title"].ToString());
//
//
//				mySetsController.CreateButton (termLine["id"].ToString(), termLine["title"].ToString(), termLine ["term_count"].ToString ());
//
//					
//					myID = termLine["id"].ToString();
//					mySetTitle = termLine["title"].ToString();//termLine ["title"].ToString();
//					mySetNumberOfTerms = termLine ["term_count"].ToString ();
//	
//					mySetTitle = mySetTitle.Substring(1,mySetTitle.Length-2 );
//					myID = myID.Substring(1,myID.Length-2 );
//	
//					quizletSet.id = myID;
//					quizletSet.title = mySetTitle;
//					quizletSet.numberOfTerms = mySetNumberOfTerms;
//					QuizletSets.Add (quizletSet);
//					
//
//					
//
//
//
//
////					//we add the texture in a coroutine because downloading might take a while
////	
////					PopulateSetsButton(myID, mySetTitle,mySetNumberOfTerms);
//	
//				}
//				
//			}
//			
//	
//		}
//		
//
//
//	public Vocabulary myNewVocab(bool mySuccess) {
//		
//		myVocab = myVocabList.RetrieveNextVocab (myGameController.myGamePlay.randomizeBlocks, false, myGameController.myGamePlay.needSuccess, mySuccess, myGameController.myGamePlay.thisIsMyGamePlay);
//
//		return myVocab;
//	}
//
//
//
//	public IEnumerator FetchMySet(string mySet) {
//
//		string mySetTitle;
//		string mySetNumberOfTerms;
//		string myID;
//		string encodedString;
//		JSONObject termLine;
//
//		string url_api = "https://api.quizlet.com/2.0/sets/"+mySet+"?whitespace=0";
//
//		Dictionary<string, string> dict_api =
//			new Dictionary<string, string>();
//
//		string key = "Authorization";
//		string value = "Bearer 0e917f92f19f6611b11aea488428d2e6aa08d762";
//
//		dict_api.Add(key,value);
//
//		int myWaitChecker = myGameController.myDontChecker.newWWWChecker ("datacontroller");
//
//		//myGameController.myDontChecker.printList ();
//
//		WWW download = new WWW (url_api, null, dict_api);
//
//		yield return download;
//
//		// check for errors
//		if (download.error == null)
//		{
//			Debug.Log("WWW Ok!: " + download.text);
//			encodedString = download.text;
//
//		} else {
//			//Debug.Log("WWW Error: "+ download.error);
//
//			//we have an offline fallback set
//			encodedString = "{\"id\":33055234,\"url\":\"/33055234/master-1-flash-cards/\",\"title\":\"Master1\",\"created_by\":\"quizlette357240\",\"term_count\":109,\"created_date\":1387448545,\"modified_date\":1435617494,\"published_date\":1387448545,\"has_images\":false,\"subjects\":[],\"visibility\":\"public\",\"editable\":\"only_me\",\"has_access\":true,\"can_edit\":true,\"description\":\"\",\"lang_terms\":\"de\",\"lang_definitions\":\"de\",\"has_discussion\":false,\"password_use\":0,\"password_edit\":0,\"access_type\":2,\"creator_id\":5774869,\"creator\":{\"username\":\"quizlette357240\",\"account_type\":\"free\",\"profile_image\":\"\",\"id\":5774869},\"class_ids\":[],\"terms\":[{\"id\":1137886785,\"term\":\"0\",\"definition\":\"Sau\",\"image\":null,\"rank\":0},{\"id\":1137886786,\"term\":\"1\",\"definition\":\"Tee\",\"image\":null,\"rank\":1},{\"id\":1137886787,\"term\":\"2\",\"definition\":\"Noah\",\"image\":null,\"rank\":2},{\"id\":1137897001,\"term\":\"3\",\"definition\":\"Mai\",\"image\":null,\"rank\":3},{\"id\":1137897002,\"term\":\"4\",\"definition\":\"Reh\",\"image\":null,\"rank\":4},{\"id\":1137897003,\"term\":\"5\",\"definition\":\"Lee\",\"image\":null,\"rank\":5},{\"id\":1137897004,\"term\":\"6\",\"definition\":\"Schuh\",\"image\":null,\"rank\":6},{\"id\":1137897005,\"term\":\"7\",\"definition\":\"Kuh\",\"image\":null,\"rank\":7},{\"id\":1137897006,\"term\":\"8\",\"definition\":\"Fee\",\"image\":null,\"rank\":8},{\"id\":1137897007,\"term\":\"9\",\"definition\":\"Po\",\"image\":null,\"rank\":9}]}";
//
//		}
//
//		encodedString = cleanMyJason.FixEncoding (encodedString);
//
//		//here we also create the buttons in the right panel
//		TransformJsonToVocabularyList (encodedString);
//
//		myVocabList.CreateVocabGroups (7, 0);
//
//		Debug.Log ("i just built a vocab list");
//		myVocabList.PrintVocabList ();
//
//		//Debug.Log ("i set checker to false");
//		myGameController.myDontChecker.setMyCheckerToFalse (myWaitChecker);
//
//		//StartCoroutine (SaveMyEncodedVocabularyList(encodedString));
//
//
//	}
//
//	public void TransformJsonToVocabularyList(string encodedString) {
//		string myTerm;
//		string myDefinition;
//		string myTermLanguage;
//		string myDefinitionLanguage;
//		string myURL;
//		JSONObject termLine;
//
//		string setTitle;
//
//		//first we clear the list
//		myVocabList.CleanList ();
//
//		Debug.Log ("encoded string just before transformation"+encodedString);
//
//		quizletArray = new JSONObject(encodedString);
//
//
//		Debug.Log ("Array length then "+quizletArray["terms"].Count);
//
//		setTitle = quizletArray ["title"].ToString ().ToUpper ();
//		setTitle = setTitle.Replace ("'", "");
//		setTitle = setTitle.Replace ("\"", "");
//
//		if (setTitle.Length > 20) {
//
//			setTitle = setTitle.Substring (0, 19) + "...";
//
//		}
//
//		//Debug.Log (setTitle);
//
//		// UI Element
//		//levelTextName.text = setTitle;
//
//		arr = quizletArray["terms"];
//
//
//
//
//		Debug.Log ("die länge meins arrays "+arr.Count);
//
//		for (int k = 0; k <= arr.Count; k++) {
//			//chunkLevel.Add(0);
//
//			//and populate Buttons
//			if (arr[k]) {
//
//				termLine = arr[k];
//				myURL = GetUrlFromWWW(termLine["image"].ToString());
//				myTerm = termLine ["term"].ToString ();
//				myDefinition = termLine ["definition"].ToString ();
//				myTerm = myTerm.Substring(1,myTerm.Length-2 );
//				myTermLanguage = quizletArray ["lang_terms"].ToString ();
//				myTermLanguage = myTermLanguage.Substring(1, myTermLanguage.Length-2);
//				myDefinitionLanguage = quizletArray ["lang_definitions"].ToString ();
//				myDefinitionLanguage = myDefinitionLanguage.Substring(1, myDefinitionLanguage.Length-2);
//				myDefinition = myDefinition.Substring(1,myDefinition.Length-2 );
//
//				myVocabList.AddVocabulary(new Vocabulary (k, myTerm, myDefinition, myTermLanguage, myDefinitionLanguage, myURL, null));
//
//				//we add the texture in a coroutine because downloading might take a while
//				//Debug.Log("term 1: "+myTerm+", term2: "+myDefinition);
//				StartCoroutine(AddTextureToVocabulary (k));
//
//				mySetController.CreateButton (k.ToString(), myTerm, myDefinition);
//
//			}
//
//		}
//
//			
//	}
//
//	string GetUrlFromWWW(string stringFromWWW) {
//		string urlFromWWW = null;
//		int i = 0;
//		int k = 3;
//		string jpg = "no";
//		bool foundJpg = false;
//		while ((jpg != "http") && (i < stringFromWWW.Length) && (stringFromWWW.Length > 4)) {
//			i++;
//			jpg = stringFromWWW.Substring(stringFromWWW.Length - 4 - i, 4);
//			if ((jpg == ".jpg") || (foundJpg)) {
//				foundJpg = true;
//				k++;
//			}
//		}
//		if (!foundJpg) {
//			while ((jpg != "http") && (i < stringFromWWW.Length) && (stringFromWWW.Length > 4)) {
//				i++;
//				jpg = stringFromWWW.Substring(stringFromWWW.Length - 4 - i, 4);
//				if ((jpg == ".png") || (foundJpg)) {
//					foundJpg = true;
//					k++;
//				}
//			}
//
//
//		}
//
//
//
//		urlFromWWW = stringFromWWW.Substring (stringFromWWW.Length - 4 - i, k);
//		return urlFromWWW;
//	}
//
//	public IEnumerator AddTextureToVocabulary(int myID) {
//		Vocabulary vocab = myVocabList.myVocabularyList [myID];
//		Texture2D myNewTexture = new Texture2D (2, 2);
//
//		int myWaitChecker = myGameController.myDontChecker.newInitializationChecker ("add texture");
//
//		//we check if the vocabulary id is within the list
//		//and we don't have a term2 already
//
//		if ((myVocabList.myVocabularyList.Count - 1 >= myID) && (!vocab.myTerm2Texture)) {
//
//			//do we have a meaningful URL?
//			if (vocab.myURL.Length > 5) {
//
//				WWW www = new WWW (vocab.myURL);
//				yield return www;
//
//				myNewTexture.LoadImage (www.bytes);
//				//vocab.myTerm2Texture = myNewTexture;
//
//				myVocabList.AddTextureToVocab(vocab.myID, myNewTexture);
//
//				//Debug.Log ("Downloaded Texture to Vocab ID "+vocab.myID);
//
//				// Debug.Log ("i just added texture for id " + myID);
//			}
//			//texture already loaded -> do nothing
//		} else if (vocab.myTerm2Texture) {
//			//dont do anything, texture already here:
//			// Debug.Log ("got my texture already loaded");
//		}
//		//we don't have a texture for this ID
//		else {
//			myNewTexture = null;
//			//url = null;
//			myVocabList.AddTextureToVocab(vocab.myID, myNewTexture);
//		}
//		// we can continue
//		myGameController.myDontChecker.setMyCheckerToFalse (myWaitChecker);
//
//	}
//
//}
