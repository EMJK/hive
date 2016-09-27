using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Canvas authorsInfo;
    public Button startText;
    public Button authorsText;
    public Button exitText;

	// Use this for initialization
	void Start () {

        quitMenu = quitMenu.GetComponent<Canvas>();
        authorsInfo = authorsInfo.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        authorsText = authorsText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        quitMenu.enabled = false;
        authorsInfo.enabled = false;

	}

    public void exitPress()
    {
        quitMenu.enabled = true;
        authorsInfo.enabled = false;
        startText.enabled = false;
        authorsText.enabled = false;
        exitText.enabled = false;
    }

    public void authorsPress()
    {
        authorsInfo.enabled = true;
        quitMenu.enabled = false;
        startText.enabled = false;
        authorsText.enabled = false;
        exitText.enabled = false;
    }

    public void noPress()
    {
        quitMenu.enabled = false;
        authorsInfo.enabled = false;
        startText.enabled = true;
        authorsText.enabled = true;
        exitText.enabled = true;
    }

    public void startGame()
    {
        SceneManager.LoadScene("_SCENE_");
    }

    public void exitGame()
    {
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
