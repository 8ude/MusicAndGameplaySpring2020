using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;

/// <summary>
/// Data holder for our Beat Map events - basically the things that make up our level
/// In this case, we're doing a falling gems interface similar to guitar hero or DDR
/// </summary>
[System.Serializable]
public class BeatMapEvent
{
    //We use the measure-beat-tick class for the timings of our beatmap events
    [Tooltip("The timing of your beatmap event")]
    public MBT eventMBT;

    //these need to be public because other events will change them, but we don't want to mess with them in the inspector
    //[HideInInspector] public bool cueCalled = false;
    //[HideInInspector] public bool cueActive = false;

    //Using Keyboard for now, eventually we'll use the input manager;
    [Tooltip("Make Sure This Matches one of your PlayerInputKeys!")]
    public KeyCode inputKey;

    //this will be assigned at runtime;
    [HideInInspector] public double cueTime;
    private GameObject _cueObject;
    
}

/// <summary>
/// This class manages the different events in the beatmap.  Right now it's designed to go sequentially:
/// once the cueTime is reached, it instantiates the cue 
/// </summary>
public class Beatmap : MonoBehaviour
{
    [Header("Our Beatmap Level")]
    public BeatMapEvent[] beatEvents;

    //the "cue" here can be a number of things
    //for now it's just the spawn time offset (in number of beats)
    //right now, this assumes that each beatEvent will have the same cue offset.
    //if you don't want this to be the case, have a seperate beatmap/input evaluator pair for these other events
    //(example - rhythm heaven has varying cue lengths)
    [Header("Cue Offset in Beats")]
    public int cueBeatOffset;

    //Make sure the OkWindow > GoodWindow > PerfectWindow!!!  Also make sure that you don't have successive beatmap at shorter timespans than your OkWindow
    [Header("Window Sizes in MS")]
    public double OkWindowMillis;
    public double GoodWindowMillis;
    public double PerfectWindowMillis;

    int beatEventIndex = 0;
    int cueIndex = 0;

    //these were for debugging
    double nextCueTime;
    double nextBeatEventTime;
    bool cueEndReached = false;



    //These should all be the same length, and should correspond to the different inputs, starting locations, and prefabs
    public KeyCode[] playerInputKeys;
    public GameObject[] cueStartLocations;
    public GameObject[] cuePrefabs;

    bool levelEndReached = false;




    void Start()
    {

        for (int i = 0; i < beatEvents.Length; i++)
        {
            //set the cue times for each beat event            
            beatEvents[i].cueTime = beatEvents[i].eventMBT.GetMilliseconds() - (cueBeatOffset * Clock.Instance.BeatLengthD() * 1000d);

        }

        beatEventIndex = 0;


        levelEndReached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (beatEventIndex >= beatEvents.Length)
        {
            levelEndReached = true;
        }

        if (!levelEndReached)
        {
            //Debug.Log("Event Millis in Update: " + beatEvents[beatEventIndex].eventMBT.GetMilliseconds());
            if (Clock.Instance.TimeMS >= beatEvents[beatEventIndex].cueTime)
            {

                CreateCue();
                //Add something visual here - juice particles, maybe start your animations?


                beatEventIndex++;

            }
        }

        

    }

    public void Reset()
    {
        cueIndex = 0;
        beatEventIndex = 0;
    }

    public void CreateCue()
    {

        Debug.Log("create cue");
        //determining which cue lane we're in (which cue type we're using)
        int cueLaneIndex = int.MaxValue;
        for (int i = 0; i < playerInputKeys.Length; i++)
        {
            if (playerInputKeys[i] == beatEvents[beatEventIndex].inputKey)
            {
                cueLaneIndex = i;
            }
        }

        if (cueLaneIndex == int.MaxValue)
        {
            Debug.LogWarning("beatmap input key doesn't match current inputs!");
        }
        GameObject newCue = Instantiate(cuePrefabs[cueLaneIndex], cueStartLocations[cueLaneIndex].transform.position, Quaternion.identity);

        FallingGem fallingGem = newCue.GetComponent<FallingGem>();

        fallingGem.bmEvent = beatEvents[beatEventIndex];

        //Set Window Timings
        fallingGem.OkWindowStart = fallingGem.bmEvent.eventMBT.GetMilliseconds() - (0.5d * OkWindowMillis);
        fallingGem.OkWindowEnd = fallingGem.bmEvent.eventMBT.GetMilliseconds() + (0.5d * OkWindowMillis);
        fallingGem.GoodWindowStart = fallingGem.bmEvent.eventMBT.GetMilliseconds() - (0.5d * GoodWindowMillis);
        fallingGem.GoodWindowEnd = fallingGem.bmEvent.eventMBT.GetMilliseconds() + (0.5d * GoodWindowMillis);
        fallingGem.PerfectWindowStart = fallingGem.bmEvent.eventMBT.GetMilliseconds() - (0.5d * PerfectWindowMillis);
        fallingGem.PerfectWindowEnd = fallingGem.bmEvent.eventMBT.GetMilliseconds() + (0.5d * PerfectWindowMillis);

        fallingGem.crossingTime = fallingGem.bmEvent.eventMBT.GetMilliseconds();
        
    }

   


}
