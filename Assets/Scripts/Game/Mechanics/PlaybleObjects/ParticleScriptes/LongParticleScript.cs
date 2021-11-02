using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongParticleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
