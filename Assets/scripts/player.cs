using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

    // Use this for initialization
    public bool alive = true;

   // AudioSource audio;
    //public AudioClip gunSound;

    public Camera cam;
    private Transform trans;
   // public Transform cameraTransform;
    public GameObject generate, prefabs;
    //public Camera HeroCamera;

    public Vector3 force, gap, rot;

    void Start()
    {
        transform.localEulerAngles = new Vector3(90, 90, 90);
        trans = cam.GetComponent<Transform>();
        force = new Vector3(0, 0, 1000);
        gap = new Vector3(0,1.5f, 0);
        rot = new Vector3(0, 90, 0);
        // cam.GetComponent<Camera>();
        // cameraTransform = HeroCamera.GetComponent<Transform>();

    }

    void Update()
    {
		//Debug.Log(cam.transform.eulerAngles);
        if (alive) // if alive
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float help = cam.transform.rotation.y + 270;
				generate = Instantiate(prefabs,cam.transform.position + gap + 5*cam.transform.forward, Quaternion.identity);
				//Debug.Log (cam.transform.rotation);
				generate.transform.eulerAngles = new Vector3(-90f,-180f+cam.transform.eulerAngles.y,90f);


				//Debug.Log(generate.transform.eulerAngles);
                Rigidbody rg = generate.GetComponent<Rigidbody>();
				//generate.transform.rotation = cam.transform.rotation;
                Transform trans = generate.GetComponent<Transform>();

                //trans.localEulerAngles = new Vector3(270, cam.transform.rotation.y+180, 0);
                rg.AddForce(cam.transform.forward*500);
}
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "eyes")
        {
            other.transform.parent.GetComponent<monster>().checkSight();
        }
        if(other.gameObject.name == "bossEyes")
        {
            other.transform.parent.GetComponent<boss>().checkSight();
        }
        if(other.CompareTag("lostPage"))
        {
            Destroy(other.gameObject);
            gameplayCanvas.instance.findPage();
        }
    }
}
