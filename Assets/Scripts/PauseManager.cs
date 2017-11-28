using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PauseManager : MonoBehaviour {

   [SerializeField] private GameObject pause_panel;
    private bool is_in_pause = false;
    public event Action onPauseEvent;

    private static PauseManager Instance;

    public static PauseManager pauseManager
    {
        get
        {
            return Instance;
        }

    }

    public void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {     
        if (Input.GetButtonDown("Pause") && !is_in_pause)
        {
            is_in_pause = true;
            pause_panel.SetActive(true);
            
            Time.timeScale = 0;
        }
        else
        {
            if (Input.GetButtonDown("Pause") && is_in_pause)
            {
                is_in_pause = false;
                pause_panel.SetActive(false);

                Time.timeScale = 1;
            }
        }


    }

    public void OnResumeGameButtonDown()
    {
        is_in_pause = false;
        pause_panel.SetActive(false);
        Time.timeScale = 1;
    }

    public bool GetIsInPause()
    {
        return is_in_pause;
    }
}
