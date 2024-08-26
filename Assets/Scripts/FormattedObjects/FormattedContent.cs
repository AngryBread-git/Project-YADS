using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContentType
{
    text, tag, eventinfo
}

public abstract class FormattedContent
{
    private ContentType _contentType;

    public ContentType ContentType
    {
        get { return _contentType; }
        set { _contentType = value; }
    }

    public FormattedContent(ContentType givenContentType) 
    {
        _contentType = givenContentType;
    }
}
