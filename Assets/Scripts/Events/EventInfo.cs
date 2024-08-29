using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EventInfo
{
    /*
     * The base EventInfo only has a generic description string.
     * All other EventInfos inheret from this class.
     * Not all EventInfos are present in a DialoguePart, but those that are should have info about their formatting
    */
    
    public string EventDescription;
}

public class DebugEventInfo : EventInfo
{
    //Info about the importance of the debug. 1 being the highest.
    public int PriorityLevel;
}

