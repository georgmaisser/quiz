using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodleDbFields {


	public int dBFieldId;
	public int dBFieldDataId;
	public string dBFieldName;
	public string dbFieldType;
	public string dBFieldValue;


	public MoodleDbFields(int newId, int newDataId, string newName, string newType, string newValue) {

		dBFieldId = newId;
		dBFieldDataId = newDataId;
		dBFieldName = newName;
		dbFieldType = newType;
		dBFieldValue = newValue;

	}

	public MoodleDbFields(int newId, int newDataId, string newName, string newType) {

		dBFieldId = newId;
		dBFieldDataId = newDataId;
		dBFieldName = newName;
		dbFieldType = newType;

	}

	public void PrintField() {
	
		Debug.Log ("field id:"+dBFieldId+" dataid: "+dBFieldDataId+" name: "+dBFieldName+" type: "+dbFieldType+" value: "+dBFieldValue);
	
	
	}
}
