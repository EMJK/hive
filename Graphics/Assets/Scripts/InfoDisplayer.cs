using UnityEngine;
using System.Collections;

public class InfoDisplayer : MonoBehaviour {

    private static bool showInfo = false;
    private float timer = 0;
    private float infoTime = 3;
    private static GUIText infoGUI;

    // Use this for initialization
    void Start () {
	    //Debug.Log("InfoDisplayer started");
	}
	
	// Update is called once per frame
	void Update () {

        if (showInfo)
        {
            if (timer < infoTime)
            {
                timer += Time.deltaTime;
            }
            else {
                infoGUI.enabled = false;
                showInfo = false;
                timer = 0;
            }
        }
    }

    public static void DisplayInfoMessage(string infoText)
    {
        infoGUI.text = infoText;
        infoGUI.enabled = true;
        showInfo = true;
    }
}


