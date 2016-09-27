using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuExitScript : MonoBehaviour
{

    public Canvas exitMenu;
    public Button exitText;

    // Use this for initialization
    void Start()
    {
        exitMenu = exitMenu.GetComponent<Canvas>();
        exitText = exitText.GetComponent<Button>();
        exitMenu.enabled = false;
    }

    public void exitPress()
    {
        exitMenu.enabled = true;
        exitText.enabled = false;
    }

    public void noPress()
    {
        exitMenu.enabled = false;
        exitText.enabled = true;
    }

    public void exitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
