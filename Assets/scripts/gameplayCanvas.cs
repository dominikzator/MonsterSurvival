using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameplayCanvas : MonoBehaviour {

    public static gameplayCanvas instance;
    public GameObject directionalLight;
    public monster[] monsters;
    public Text txtPages;
    public string pagesString;
    public int pagesTotal = 4;
    private int pagesFound = 0;

    public GameObject boss;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        pagesString = "Pages " + pagesFound.ToString() + "/" + pagesTotal.ToString();
        txtPages.text = pagesString;
    }
	
    public void updateCanvas()
    {
        pagesString = "Pages " + pagesFound.ToString() + "/" + pagesTotal.ToString();
        txtPages.text = pagesString;
    }

    public void findPage()
    {
        pagesFound++;
        updateCanvas();

        if(pagesFound >= pagesTotal)
        {
            directionalLight.SetActive(true);

            for(int n = 0; n<monsters.GetLength(0);n++)
            {
                monsters[n].death();
            }
			boss.GetComponent<boss> ().death ();
            //boss.SetActive(true);
        }
    }

}
