using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void GameWin()
    {
        SceneManager.LoadScene("GameWin");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("FirstMenu");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Mushroom");
    }
}
