using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to adjust how much our mesh changes with audio input
//now that we have a MIDI controller, we want to have all our controls on the same object
public class AudioShaderReact : MonoBehaviour {

	//assign the spectrum analyzer that you wish to control material properties with
    public SpectrumAnalysis analyzer;
	
	//lower values = smoother animation
	public float smoothing = 0.75f;

	[UnityEngine.Range(0f, 1f)] public float shaderTimeAmount;
	[UnityEngine.Range(50f, 20000f)] public float shaderTimeLowBound, shaderTimeHighBound;

	[UnityEngine.Range(0f, 10f)] public float shaderDisplaceAmount;
	
	[UnityEngine.Range(0f, 100f)] public float shaderColorAmount;
	[UnityEngine.Range(50f, 20000f)] public float shaderColorLowBound, shaderColorHighBound;
	
	public GameObject reactiveGameObject;
	[SerializeField] Material myMaterial;
	
	
    float normalizedAudioInput = 0f;
	float prevAudioInput = 0f;
	float smoothedAudioInput = 0f;

	private float prevShaderTime;
	private float prevShaderColor;
	

	// Use this for initialization
	void Start () {

		myMaterial = reactiveGameObject.GetComponent<Renderer> ().material;

		prevShaderTime = 0f;
		prevShaderColor = 0f;
		
	}
	
	// Update is called once per frame
	void Update () {
		

		//we use the overall audio input to adjust the displacement
		normalizedAudioInput = analyzer.GetWholeEnergy() * shaderDisplaceAmount;

		//we're applying some additional smoothing here
		smoothedAudioInput = Mathf.Lerp (prevAudioInput, normalizedAudioInput, smoothing);
		prevAudioInput = smoothedAudioInput;

		//Find the time value from the range of frequencies that we want
		float shaderTime = analyzer.GetEnergyFrequencyRange(shaderTimeLowBound, shaderTimeHighBound);
		shaderTime *= shaderTimeAmount;
		
		//Apply Smoothing
		shaderTime = Mathf.Lerp(prevShaderTime, shaderTime, smoothing);
		prevShaderTime = shaderTime;
		
		float shaderColor = analyzer.GetEnergyFrequencyRange(shaderColorLowBound, shaderColorHighBound);
		shaderColor *= shaderColorAmount;

		
		shaderColor = Mathf.Lerp(prevShaderColor, shaderColor, smoothing);
		prevShaderColor = shaderColor;

		//check to see if we have a material, then start adjusting shader values
		if (myMaterial != null) {
			myMaterial.SetFloat ("_AudioInput", smoothedAudioInput);
			myMaterial.SetFloat ("_ShaderTime", shaderTime);
			myMaterial.SetFloat ("_ShaderColor", shaderColor);
		}
        
	}
}
