using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleaver : MonoBehaviour {

	public int charges = 1;

	private Vector3 rotateVector = new Vector3 (0f, 15f, 0f);
	private bool flying = true;

	// Use this for initialization
	void Start () {
		//gameObject.GetComponent<Animator> ().SetTrigger ("rotateTrigger");
		//rotateVector = new Vector3 (0f, 15f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

		if (flying) {
			//Debug.Log ("test");
			gameObject.transform.Rotate (rotateVector);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log (col.gameObject.name);
		//if(col.gameObject.name != "Floor")
		//{
		//Debug.Log ("Collision Enter Cleaver " + col.gameObject.name);
		flying = false;
		//}
	}
}
