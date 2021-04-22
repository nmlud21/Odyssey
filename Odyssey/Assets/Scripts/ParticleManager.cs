using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem particleSystem;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
        {
            particleSystem.Simulate(Time.unscaledDeltaTime, false, false);
        }
        else
        {
            particleSystem.Simulate(Time.deltaTime, false, false);
        }
    }
}
