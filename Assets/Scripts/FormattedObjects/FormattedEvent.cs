using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormattedEvent : FormattedContent
{
    private EventInfo _eventInfo;
    public EventType _eventType;

    public EventInfo Info
    {
        get { return _eventInfo; }
        set { _eventInfo = value; }
    }

    public FormattedEvent(EventInfo givenEventInfo) : base(ContentType.eventinfo)
    {
        _eventInfo = givenEventInfo;
    }
}
