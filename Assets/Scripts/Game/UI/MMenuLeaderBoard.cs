using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MMenuLeaderBoard : MonoBehaviour
{
    public event Action OkClickedEvent;
    public void OnOkClicked()
    {
        OkClickedEvent?.Invoke();
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
