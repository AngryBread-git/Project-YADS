using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormattedTag : FormattedContent
{
    private string _tag;

    public string Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    public FormattedTag(string givenTag) : base(ContentType.tag)
    {
        _tag = givenTag;
    }
}
