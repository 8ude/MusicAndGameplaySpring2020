using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a line renderer visualization of an AudioSource
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class AudioLine : MonoBehaviour {

    public AudioSource aSource;

    public LineRenderer lineRenderer;

    //this must be a power of 2!!!
    public int numLinePoints = 128;
    
    public float lineSpacing = 1f;
    
    //translating audio data to unity's Y direction
    public float audioScaling = 10f;

    //applying scaling because the audio data is inherently jittery
    //smaller values ~= smoother animation
    public float linearSmoothing = 0.1f;

    //this is our array of audio data, which is updated every frame
    public float[] audioData;
    
    Transform lineStartLocation;
    
    // Start is called before the first frame update
    void Start() {

        //assigning the start location
        lineStartLocation = transform;
        
        //initializing the audio data
        audioData = new float[numLinePoints];
        
        //initializing the line renderer according to the spacing that we want
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numLinePoints;
        for (int i = 0; i < numLinePoints; i++) {
            
            //point position is the position of this segment of the line renderer
            Vector3 pointPosition = new Vector3(
                lineStartLocation.position.x + (i*lineSpacing),
                0,
                0
                );
            lineRenderer.SetPosition(i, pointPosition);
        }
    }

    // Update is called once per frame
    void Update() {
        
        //fill our audioData with numbers from the audio source
        aSource.GetOutputData(audioData, 0);


        
        //assign the y position of the corresponding line point
        for (int i = 0; i < numLinePoints; i++) {

            //lerp between the old position and the new position via the linear smoothing
            float newYPosition = Mathf.Lerp(
                lineRenderer.GetPosition(i).y,
                lineStartLocation.position.y + (audioData[i] * audioScaling),
                linearSmoothing
            ); 
            
            //erase whole words = option + delete
            //set the new point position vector
            Vector3 pointPosition = new Vector3(
                lineStartLocation.position.x + (i*lineSpacing),
                newYPosition,
                lineStartLocation.position.z
            );
            
            
            //assign the new position
            lineRenderer.SetPosition(i, pointPosition);

            
        }
        
    }
}
