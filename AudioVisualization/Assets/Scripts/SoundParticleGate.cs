using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParticleGate : MonoBehaviour {

    public SpectrumAnalysis spectrumAnalysis;

    public float volumeThreshold = 0.1f;
    
    public ParticleSystem particleSystem;
    
    
    
    // Start is called before the first frame update
    void Start() {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        float SpectrumEnergy = spectrumAnalysis.GetWholeEnergy();

        if (SpectrumEnergy > volumeThreshold) {
            particleSystem.Play();
        }
    }
}
