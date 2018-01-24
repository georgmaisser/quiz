using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

 
public class ClassAsset
{
	[MenuItem("Assets/Create/QuizletData")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<QuizletData> ();
	}
}

#endif