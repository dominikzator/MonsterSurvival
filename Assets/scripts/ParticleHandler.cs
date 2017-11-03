using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour {

	public GameObject hitParticle;

	private Camera cam;
	private Vector3 moveVector;
	private bool moving;
	public int charges = 1;

	private ParticleSystem[] objects;
	// Use this for initialization
	void Start () {
		objects = GetComponentsInChildren<ParticleSystem> ();
		moving = true;
		cam = FindObjectOfType<Camera> ();
		moveVector = cam.transform.forward  + new Vector3(180f,0f,0f);
		transform.rotation.eulerAngles.Set (-90f,cam.transform.forward.y,transform.rotation.eulerAngles.z);
		Vector3 helpVector = new Vector3 (90f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			transform.Translate (moveVector * Time.deltaTime * 0.1f);
		} 
	}

	void OnParticleCollision(GameObject other) {
		
		if (other.gameObject.tag != "Floor"){
			GetComponent<Animator> ().SetTrigger ("collisionTrigger");
			moving = false;
			Instantiate (hitParticle,transform.position,Quaternion.identity);
			DestroyObject ();

		}

		if (other.gameObject.tag == "Monster") {
			if (charges == 1) {
				other.GetComponent<monster> ().LoseLife ();
				charges--;
			}
		}
	}

	public void DestroyObject()
	{
		Destroy (gameObject);
	}



}
