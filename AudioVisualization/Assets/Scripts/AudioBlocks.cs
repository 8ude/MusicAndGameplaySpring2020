using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script works like a basic visualizer with two modes:
//in the first mode, it acts like an oscilloscope, with each block representing a point in time (or "phase")
//in the second mode, it shows the frequency data
public class AudioBlocks : MonoBehaviour
{
    public enum VisualizerMode { scope, fft }
    public VisualizerMode currentMode;
    VisualizerMode previousMode;

    //must be a power of 2!
    public int ArraySize = 512;

    public GameObject blockPrefab;

    private GameObject[] blocksArray;

    private float[] outputDataArray;
    private float[] fftDataArray;

    public float scalingFactorScope;
    public float scalingFactorFFT;
    public float blockSpacing;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        blocksArray = new GameObject[ArraySize];

        outputDataArray = new float[ArraySize];
        fftDataArray = new float[ArraySize * 2];

        for (int i = 0; i < ArraySize; i ++)
        {
            blocksArray[i] = Instantiate(
                blockPrefab,
                new Vector3(blockSpacing * i, 0, 0),
                Quaternion.identity);
        }
    }


    void Update()
    {
        //update audio buffers;
        audioSource.GetOutputData(outputDataArray, 0);
        audioSource.GetSpectrumData(fftDataArray, 0, FFTWindow.Hanning);

        //update our visuals based on what mode we're using
        switch (currentMode)
        {
            case VisualizerMode.scope:
                for (int i = 0; i < blocksArray.Length; i++)
                {
                    blocksArray[i].transform.localScale = new Vector3(
                        1f,
                        outputDataArray[i] * scalingFactorScope,
                        1f);
                }
                break;
            case VisualizerMode.fft:

                for (int i = 0; i < blocksArray.Length; i++)
                {
                    blocksArray[i].transform.localScale = new Vector3(
                        1f,
                        fftDataArray[i] * scalingFactorFFT,
                        1f);
                }

                break;
                   
        }
    }


}
