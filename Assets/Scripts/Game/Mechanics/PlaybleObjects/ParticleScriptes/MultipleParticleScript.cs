using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleParticleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float timeToDestroy = 0;
        ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            timeToDestroy += particleSystem.main.startLifetime.constant;
        }
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
