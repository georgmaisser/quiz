//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//
//
//public class GameController: MonoBehaviour {
//
//
//	public GameObject myElement;
//
//	public GameObject wfElement;
//
//	public GameObject EventSystem;
//
//	public List<GameObject> myWFElements;
//
//	public float heightOfAllElements = 0f;
//	public ScrollRect scrollView;
//
//
//
//
//	// Use this for initialization
//	void Start () {
//
//		if (!LookForElements ()) {
//			SpawnNewElement ();
//
//		} else {
//			Debug.Log ("Ready to go");
//		}
//			
//
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
//	public void SpawnNewElement() {
//
//		//StartCoroutine (SpawnNewElementLater());
//
//		RectTransform rt;
//		float newPosition = 0f;
//
//		//yield return new WaitForEndOfFrame();
//
//
//		foreach (GameObject wfElementInLine in myWFElements) {
//
//			newPosition += wfElementInLine.GetComponent<WriteFlowElement> ().myHeight;
//			//Debug.Log ("new position in loop "+newPosition);
//
//		}
//
//
//		Debug.Log ("my position:"+newPosition);
//
//		heightOfAllElements = newPosition;
//
//		wfElement = Instantiate<GameObject> (myElement, this.gameObject.transform, false);
//		//myElement, this.gameObject.transform.position ,Quaternion.identity);
//		//wfElement.transform.SetParent (this.gameObject.transform);
//
//		rt = (RectTransform)wfElement.transform;
//
//		//we add half the size of element
//
//		newPosition = wfElement.GetComponent<WriteFlowElement> ().standardHeight / 2f + heightOfAllElements;
//
//		myWFElements.Add (wfElement);
//
//
//		rt.anchoredPosition = new Vector2 (0f, -newPosition);
//
//
//
//	}
//
//
//	bool LookForElements() {
//		bool elementsPresent = false;
//
//		if (this.transform.childCount > 0) {
//			elementsPresent = true;
//		}
//
//		return elementsPresent;
//	}
//
////	IEnumerator SpawnNewElementLater()  {
////		RectTransform rt;
////		float newPosition = 0f;
////
////		yield return new WaitForEndOfFrame();
////
////
////		foreach (GameObject wfElementInLine in myWFElements) {
////
////			newPosition += wfElementInLine.GetComponent<WriteFlowElement>().myHeight;
////			Debug.Log ("new position in loop "+newPosition);
////
////		}
////
////		heightOfAllElements = newPosition;
////
////		Debug.Log ("my position:"+newPosition);
////
////
////
////		wfElement = Instantiate<GameObject> (myElement, this.gameObject.transform, false);
////			//myElement, this.gameObject.transform.position ,Quaternion.identity);
////		//wfElement.transform.SetParent (this.gameObject.transform);
////
////		rt = (RectTransform)wfElement.transform;
////
////		//we add half the size of element
////
////		newPosition += wfElement.GetComponent<WriteFlowElement> ().standardHeight / 2f;
////
////
////		myWFElements.Add (wfElement);
////
////
////		rt.anchoredPosition = new Vector2 (0f, -newPosition);
////
////
////
////	}
//
//
//}
