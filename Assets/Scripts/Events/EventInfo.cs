using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EventInfo
{
    /*The base EventInfo
     * only has a generic description string.
     * All other EventInfos inheret from this class.
     * Not all EventInfos are present in a textfile, but those that are should have info about their formatting
    */
    
    public string EventDescription;
}

public class DebugEventInfo : EventInfo
{
    //info about the importance of the debug. 1 being the highest.
    public int PriorityLevel;
}

//This is an example of an event. An enemy dies, so particles are spawned at it's location and a sound plays.
public class UnitDeathEventInfo : EventInfo
{
    public GameObject UnitGO;

    public GameObject UnitDeathParticle;

    public AudioClip UnitDeathAudioClip;
}

