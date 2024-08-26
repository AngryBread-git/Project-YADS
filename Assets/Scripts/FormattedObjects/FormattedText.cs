using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormattedText : FormattedContent
{
    private string _text;

    public string Text 
    {
        get { return _text; }
        set { _text = value; }
    }

    public FormattedText(string givenText) : base(ContentType.text)
    {
        _text = givenText;
    }
}
