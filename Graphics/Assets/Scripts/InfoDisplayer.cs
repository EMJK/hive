using UnityEngine;
using System.Collections;

public class InfoDisplayer : MonoBehaviour {

    private bool showInfo = false;
    private float timer = 0;
    public float infoTime = 3;
    public GUIText infoGUI;

    // Use this for initialization
    void Start () {
	
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

    public void displayInfoMessage(string infoText)
    {
        infoGUI.text = infoText;
        infoGUI.enabled = true;
        this.showInfo = true;
    }
}


