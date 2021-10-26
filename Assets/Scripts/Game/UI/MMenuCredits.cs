using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MMenuCredits : MonoBehaviour
{
    public event Action OkPressEvent; 
    // Start is called before the first frame update
    public void OnrPessOk()
    {
        OkPressEvent?.Invoke();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
