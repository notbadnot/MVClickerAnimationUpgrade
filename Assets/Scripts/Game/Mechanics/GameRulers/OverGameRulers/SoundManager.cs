using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject soundObjectPrefab;
    // Start is called before the first frame update
    public SoundObject SpawnSoundObject()
    {
        Debug.Log("Sound manager is working");
        GameObject soundObject = Instantiate(soundObjectPrefab);
        Object.DontDestroyOnLoad(soundObject);
        Debug.Log("Sound manager is working 2");
        return soundObject.GetComponent<SoundObject>();
    }


}
