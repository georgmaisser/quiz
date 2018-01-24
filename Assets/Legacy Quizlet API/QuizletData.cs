using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuizletData : ScriptableObject
{
		public string jsonstring;
		public string apiToken = "0e917f92f19f6611b11aea488428d2e6aa08d762";
		public List<QuizletSet> quizletSets;

}
