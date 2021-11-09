using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject soundObjectPrefab;
    // Start is called before the first frame update
    public SoundObject SpawnSoundObject()
    {
        GameObject soundObject = Instantiate(soundObjectPrefab);
        Object.DontDestroyOnLoad(soundObject);
        return soundObject.GetComponent<SoundObject>();
    }


}
