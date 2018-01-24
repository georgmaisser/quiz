using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MoodleUser {

	public string userName;
	public string fullName;
	public int userId;
	public string userToken;
	public int lastAccess;


	public MoodleUser() {


	}

	public MoodleUser(int newId, string newName, string newToken) {

		userName = clean(newName);
		fullName = userName;
		userId = newId;
		userToken = newToken;

	}


	public MoodleUser(int newId, string newFullName, int newLastAccess) {

		//userName = newUserName;
		fullName = clean( newFullName);
		userName = fullName;
		userId = newId;
		lastAccess = newLastAccess;

	}




	public bool HasToken() {
		if (userToken.Length > 1) {
			return true;
		} else {
			return false;
		}

	}

	public string clean(string s)
	{
		StringBuilder sb = new StringBuilder (s);

		sb.Replace("<p>", "");
		sb.Replace("<\\/p>", "");
		sb.Replace("\\u00fc", "ü");
		sb.Replace("\"", "");
		sb.Replace("\\", "");
		sb.Replace(";", "");

		return sb.ToString();
	}




}