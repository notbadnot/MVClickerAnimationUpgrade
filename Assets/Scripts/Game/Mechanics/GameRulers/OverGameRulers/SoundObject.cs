using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        Debug.Log("Sound object started");
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public void Play(AudioClip audioClip, Vector3 position, bool is2D = false, bool looping = false)
    {
        transform.position = position;
        Play(audioClip, is2D, looping);
    }
    public void Play(AudioClip audioClip, bool is2D = false, bool looping = false)
    {
        audioSource.clip = audioClip;
        if (is2D)
        {
            audioSource.spatialBlend = 0;
        }
        else { audioSource.spatialBlend = 0.5f; }
        audioSource.loop = looping;
        audioSource.Play();
    }
    public void Stop()
    {
        audioSource.Stop();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

}
