using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MMenuSettingsScript : MonoBehaviour
{

    public event Action PressBabyEvent;
    public event Action PressHumanEvent;
    public event Action PressAlienEvent;
    public event Action PressAcceptEvent;
    public event Action PressCancelEvent;
    public event Action<bool> CrowdingChangeEvent;
    public event Action<bool> GameModeChangeEvent;

    [SerializeField]public Toggle crowdingToogle;
    [SerializeField]public Toggle gameModeToogle;
    [SerializeField] public RectTransform chooseButtons;
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
    public void OnCrowdingChange()
    {
        CrowdingChangeEvent?.Invoke(crowdingToogle.isOn);
    }
    public void OnGameModeChange()
    {
        GameModeChangeEvent?.Invoke(gameModeToogle.isOn);
    }
    // Start is called before the first frame update
    void Start()
    {
        crowdingToogle = gameObject.transform.Find("Panel").Find("Toggle").GetComponent<Toggle>();
        gameModeToogle = gameObject.transform.Find("Panel").Find("GameModeToogle").GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
