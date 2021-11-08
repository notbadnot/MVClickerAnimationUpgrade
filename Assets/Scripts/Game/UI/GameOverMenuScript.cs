using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameOverMenuScript : MonoBehaviour
{
    [SerializeField] InputField inputName;
    [SerializeField] public Text resultLabel;
    private string inputedPlayerName = "UnknownSoldier";
    public event Action<string> PressedOkEvent;
    // Start is called before the first frame update
    public void ChangeInputeDPlayerName()
    {
        string newName = inputName.text;
        if (newName == "" || newName == null)
        {
            inputedPlayerName = "UnknownSoldier";
        }
        else { inputedPlayerName = newName; }
    }
    public void OnPressedOkEvent()
    {
        PressedOkEvent?.Invoke(inputedPlayerName);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
