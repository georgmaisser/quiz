using UnityEngine;
using System.Collections;

class AnimateTiledTexture : MonoBehaviour
{
	public int columns = 4;
	public int rows = 3;
	public float framesPerSecond = 10f;

	//the current frame to display
	private int index = 0;

	void Start()
	{
		//StartCoroutine(blinkOnce());

		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / columns, 1f / rows);
		GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
		blinkOnce ();
	}

	public void blinkOnce() {
		StartCoroutine(runTiles());

	}

	public IEnumerator runTiles()
	{
		Debug.Log ("i run tiles");
		bool oneLoop = true;

		while (oneLoop)
		{
			//move to the next index
			index++;
			if (index >= rows * columns) {
				index = 0;
				oneLoop = false;
			}
				

			//split into x and y indexes
			Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
				(index / columns) / (float)rows);          //y index

			GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

			yield return new WaitForSeconds(1f / framesPerSecond);
		}

	}
}