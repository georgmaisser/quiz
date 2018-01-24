//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//
//public class QuizletAPI : MonoBehaviour
//{
//
//		const string baseURL = "https://api.quizlet.com/2.0/";
//		public static List<QuizletSet> quizletSets = new List<QuizletSet> ();
//		public QuizletData quizletData;
//		public string userName;
//
//		//TODO: Initialize the API with App Settings
//		public void Start ()
//		{
//				userName = "quizlette357240";
//				UpdateUserSets();
//				Debug.Log (quizletData.apiToken);
//		}
//
//		public void loadSets ()
//		{
//				/*
//				string response = FetchJSON ("users/quizlette357240");
//				JSONObject quizletObject = GetObject (response);
//				accessData (quizletObject);
//				*/
//
//				GameObject go = GameObject.Find ("Gamefield bottom");
//				GameController other = (GameController)go.GetComponent (typeof(GameController));
//				other.LoadListOfSets (); 
//
//		}
//
//		// Returns JSON encoded string on success or empty string, if there was a error downloading the JSON string
//		// TODO: Implement a result handler that calls the calling method or any other method specified in the params after yield return www
//		// or implement an event handler
//		public string FetchJSON (string quizletEndpoint)
//		{
//				string userURL = baseURL + quizletEndpoint;
//
//				QuizletCredentials credentials = new QuizletCredentials (quizletData.apiToken);
//				WWW www = new WWW (userURL, null, credentials.AuthHeader);
//
//				while (!www.isDone) {
//						new WaitForSeconds (0.1f);
//				} 
//				if (www.error == null) {
//						Debug.Log ("WWW Ok!: " + www.text);
//						return www.text;
//				} else {
//						Debug.Log ("WWW Error: " + www.error);
//						return "";
//				} 
//		}
//
//		public string FetchUserDetails (string userName)
//		{
//				return FetchJSON ("users/" + userName);
//		}
//
//		public string FetchUserSets (string userName)
//		{
//				return FetchJSON ("users/" + userName + "/sets");
//		}
//
//		public string FetchSingleSet (int setID)
//		{
//				return FetchJSON ("sets/" + setID);
//		}
//
//		public string FetchSingleSetTerms (int setID)
//		{
//				return FetchJSON ("sets/" + setID + "/terms");
//		}
//
//		public string FetchMultiSetsByID (int[] setIDs)
//		{
//				string[] result = setIDs.Select (x => x.ToString ()).ToArray ();
//				string idquery = string.Join (",", result);
//				return FetchJSON ("?set_ids=" + idquery);
//		}
//
//		public JSONObject GetObject (string jsonstring)
//		{
//				JSONObject quizletObject = new JSONObject (jsonstring);
//				return quizletObject;
//		}
//
//		public void UpdateUserSets ()
//		{
//				string response = FetchUserSets (userName);
//				JSONObject quizletObject = GetObject (response);
//				//accessData (quizletObject);
//				foreach (JSONObject singleSet in quizletObject.list) 
//					{
//						QuizletSet quizletSet = new QuizletSet();
//						quizletSet.id =  singleSet.GetField("id").ToString();
//						quizletSet.title = singleSet.GetField("title").ToString();
//						quizletSet.numberOfTerms = singleSet.GetField("term_count").ToString();
//					}
//		}
//
//
//
//		void accessData (JSONObject obj)
//		{
//				switch (obj.type) {
//				case JSONObject.Type.OBJECT:
//						for (int i = 0; i < obj.list.Count; i++) {
//								string key = (string)obj.keys [i];
//								JSONObject j = (JSONObject)obj.list [i];
//								Debug.Log (key + " is an object");
//								accessData (j);
//						}
//						break;
//				case JSONObject.Type.ARRAY:
//						foreach (JSONObject j in obj.list) {
//								Debug.Log (j.keys.ToString () + "array");
//								accessData (j);
//						}
//						break;
//				case JSONObject.Type.STRING:
//						Debug.Log (obj.str);
//						break;
//				case JSONObject.Type.NUMBER:
//						Debug.Log (obj.n);
//						break;
//				case JSONObject.Type.BOOL:
//						Debug.Log (obj.b);
//						break;
//				case JSONObject.Type.NULL:
//						Debug.Log ("NULL");
//						break;
//		
//				}
//		}
//}
