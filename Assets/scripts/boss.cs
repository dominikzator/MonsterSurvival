using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class boss : MonoBehaviour
{

    public GameObject player;
    public AudioClip[] footsounds;

    public AudioSource growl;

    private NavMeshAgent nav;
    private AudioSource sound;
    private Animator anim;
    private string state = "idle";
    private bool alive = true;
    private float wait = 0f;
    public Transform eyes;
    private bool highAlert = false;
    private float bossAlertness = 20f;
    public GameObject deathCam;
    public Transform camPos;

	public ParticleSystem wildfire;

    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        sound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        nav.speed = 1.2f;
        anim.speed = 1.2f;
    }

    public void footstep(int _num)
    {
        sound.clip = footsounds[_num];
        sound.Play();
    }

    //check if we can see a player
    public void checkSight()
    {
        if (alive)
        {
            RaycastHit rayHit;
            if (Physics.Linecast(eyes.position, player.transform.position, out rayHit))
            {
                //print("hit " + rayHit.collider.gameObject.name);
                if (rayHit.collider.gameObject.name == "player")
                {
                    if (state != "kill")
                    {
                        state = "chase";
                        nav.speed = 3.5f;
                        anim.speed = 3.5f;
                        //growl.pitch = 1.2f;
                        //growl.Play();
                    }
                }
                //
                // else if (rayHit.collider.gameObject.name != "player")
                // {
                //    if(state != "kill")
                //     {
                //         state = "hunt";
                //     }
                // }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
		wildfire.transform.position = transform.position;
		float distance = Vector3.Distance(transform.position, player.transform.position);
		//Debug.Log (distance);
		//Debug.Log (state);
		//Debug.Log (bossAlertness);
        Debug.DrawLine(eyes.position, player.transform.position, Color.green);
        if (alive)
        {
            anim.SetFloat("velocity", nav.velocity.magnitude);

            //Idle//
            if (state == "idle")
            {
                Vector3 randomPos = Random.insideUnitSphere * bossAlertness;
                NavMeshHit navHit;
                NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);

                //go near the player//
				if (highAlert) {
					NavMesh.SamplePosition (player.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);

					bossAlertness += 5f;
					//Debug.Log ("increment");
					if (bossAlertness > 20f) {
						highAlert = false;
						nav.speed = 1.2f;
						anim.speed = 1.2f;
					}
				} 

					nav.SetDestination(navHit.position);
					state = "walk";               
            }
            //Walk//
            if (state == "walk")
            {
                if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    state = "search";
                    wait = 5f;
                }
            }

            //Search
            if (state == "search")
            {
                if (wait > 0f)
                {
                    wait -= Time.deltaTime;
                    transform.Rotate(0f, 120f * Time.deltaTime, 0f);
                }
                else
                {
                    state = "idle";
                }
            }


            //Chase
            if (state == "chase")
            {

                nav.destination = player.transform.position;

                //lose sight of player
               // float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance > 15f)
                {
                    state = "hunt";

					//Debug.Log("set alert to 5");
                }
                else if (nav.remainingDistance <= nav.stoppingDistance + 2f && !nav.pathPending)
                {
                    if (player.GetComponent<player>().alive)
                    {
                        state = "kill";
                        player.GetComponent<player>().alive = false;
                        player.GetComponent<FirstPersonController>().enabled = false;
                        deathCam.SetActive(true);
                        deathCam.transform.position = Camera.main.transform.position;
                        deathCam.transform.rotation = Camera.main.transform.rotation;
                        Camera.main.gameObject.SetActive(false);
                        //growl.pitch = 0.7f;
                        //growl.Play();
                        Invoke("reset", 1f);
                    }
                }
                //if (alive)
                //{
                //    RaycastHit rayHit;
                //    if (Physics.Linecast(eyes.position, player.transform.position, out rayHit))
                //    {
                        //print("hit " + rayHit.collider.gameObject.name);
                        //if (rayHit.collider.gameObject.name != "player")
                        //{
                        //    if (state != "kill")
                        //    {
                        //        state = "hunt";
                        //    }
                        //}
                        //
                         //else if (rayHit.collider.gameObject.name == "player")
                        // {
                         //   if(state != "kill")
                         //    {
                         //        state = "chase";
                         //    }
                         //}
                  //  }
                //}
                // checkSight();
            }

            //Hunt
            if (state == "hunt")
            {
                if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    state = "search";
                    wait = 5f;
                    highAlert = true;
					bossAlertness = 5f;
                    checkSight();
                }
            }

            if (state == "kill")
            {
                deathCam.transform.position = Vector3.Slerp(deathCam.transform.position, camPos.position, 10f * Time.deltaTime);
                deathCam.transform.rotation = Quaternion.Slerp(deathCam.transform.rotation, camPos.rotation, 10f * Time.deltaTime);
                anim.speed = 1f;
                nav.SetDestination(deathCam.transform.position);
            }

        }
        //nav.SetDestination(player.transform.position);
    }

    void reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //die
    public void death()
    {
        anim.SetTrigger("dead");
        anim.speed = 1f;
        alive = false;
        nav.Stop();
    }
}
