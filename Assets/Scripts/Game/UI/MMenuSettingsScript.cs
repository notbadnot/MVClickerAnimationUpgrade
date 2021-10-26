using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MMenuSettingsScript : MonoBehaviour
{
    public event Action PressBabyEvent;
    public event Action PressHumanEvent;
    public event Action PressAlienEvent;
    public event Action PressAcceptEvent;
    public event Action PressCancelEvent;

    public void OnPressBaby() 
    {
        PressBabyEvent?.Invoke();
    }

    public void OnPressHuman()
    {
        PressHumanEvent?.Invoke();
    }
    public void OnPressAlien()
    {
        PressAlienEvent?.Invoke();
    }
    public void OnPressAccept()
    {
        PressAcceptEvent.Invoke();
    }
    public void OnPressCancel()
    {
        PressCancelEvent?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
