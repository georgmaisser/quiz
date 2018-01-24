using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodleDbRecord {


	public int dBRecordId = 0;

	public List<MoodleDbFields> dBFields = new List<MoodleDbFields>();


	//public Dictionary<string, MoodleDbFields> dBFields = new Dictionary<string, MoodleDbFields>();


	public MoodleDbRecord() {


	}


	public MoodleDbRecord(int newId, int newDataId, string newName, string newType) {

		dBFields.Add(new MoodleDbFields(newId,newDataId, newName,newType));
		}



	public MoodleDbRecord(int newId, string newName) {
	
	
	}

	



	public void PrintRecord() {
		
		foreach (MoodleDbFields fieldItem in dBFields) {
			Debug.Log ("db record id:"+dBRecordId);
			fieldItem.PrintField ();
		}

	}




}
