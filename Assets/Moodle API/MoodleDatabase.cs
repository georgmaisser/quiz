using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodleDatabase {

	public int databaseId;
	public string databaseName;
	public int courseId;

	public List<MoodleDbRecord> dBRecords = new List<MoodleDbRecord>();


	public Dictionary<string , int> dbField = new Dictionary<string, int> ();


	public MoodleDatabase(int newDatabaseId, int newCourseId, string newName) {
	
		databaseId = newDatabaseId;
		courseId = newCourseId;
		databaseName = newName;

	}

	public void AddFieldToRecordZero(int newId, int newDataId, string newName, string newType) {

		if (dBRecords.Count > 0) {
			Debug.Log ("i got a record 0 already");
		} else {
			dBRecords.Add (new MoodleDbRecord ());
		}
		dBRecords [0].dBFields.Add (new MoodleDbFields (newId, newDataId, newName, newType));

	}
		


	public void PrintDatabase(bool printFields) {

		Debug.Log ("print db id: "+databaseId+" course "+courseId+" name: "+databaseName+" records: "+dBRecords.Count);

//		foreach (MoodleDbRecord recordItem in dBRecords) {
//		
//			recordItem.PrintRecord ();
//		
//		}
		if (printFields) {
			foreach (KeyValuePair<string, int> fieldItem in dbField) {
		
				Debug.Log (fieldItem);
		
			}
		}
	}

}
