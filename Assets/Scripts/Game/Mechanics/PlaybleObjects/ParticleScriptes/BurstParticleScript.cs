using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstParticleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        Destroy(gameObject, particleSystem.main.startLifetime.constant);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
