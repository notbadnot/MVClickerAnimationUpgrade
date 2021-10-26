using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MMenuStartScript : MonoBehaviour
{
    public event Action StartEvent;
    public event Action SettingsEvent;
    public event Action QuitEvent;
    public event Action CreditsEvent;
    public event Action LeaderBoardEvent;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPressStart()
    {
        StartEvent?.Invoke();
    }
    public void OnPressSettings()
    {
        SettingsEvent?.Invoke();
    }
    public void OnPressQuit()
    {
        QuitEvent?.Invoke();
    }
    public void OnPressCredits()
    {
        CreditsEvent?.Invoke();
    }
    public void OnPressLeaderBoard()
    {
        LeaderBoardEvent?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
