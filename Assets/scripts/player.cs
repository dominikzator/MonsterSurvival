using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour {

    // Use this for initialization
    public bool alive = true;

   // AudioSource audio;
    //public AudioClip gunSound;

    public Camera cam;
    private Transform trans;
   // public Transform cameraTransform;
    public GameObject generate, prefabs;
	public GameObject manaBar;

	private float costValue = 0.3f;
    //public Camera HeroCamera;

    public Vector3 force, gap, rot;

    void Start()
    {
        transform.localEulerAngles = new Vector3(90, 90, 90);
        trans = cam.GetComponent<Transform>();
        force = new Vector3(0, 0, 1000);
        gap = new Vector3(0,1.5f, 0);
        rot = new Vector3(0, 90, 0);
    }

    void Update()
    {
        if (alive) // if alive
        {
			if (Input.GetMouseButtonDown(0) && cam.transform.forward.y>0.1f && manaBar.GetComponent<Image> ().fillAmount>= costValue)
            {
                float help = cam.transform.rotation.y + 270;
				generate = Instantiate(prefabs,cam.transform.position +cam.transform.forward, Quaternion.identity);
				generate.transform.eulerAngles = new Vector3(-90f,-180f+cam.transform.eulerAngles.y,90f);
                Rigidbody rg = generate.GetComponent<Rigidbody>();
                Transform trans = generate.GetComponent<Transform>();
				manaBar.GetComponent<Image> ().fillAmount -= 0.3f;
			}

			manaBar.GetComponent<Image> ().fillAmount += Time.deltaTime * 0.1f;
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
