using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePart 
{
    [SerializeField] private CharacterInfo _characterInfo;

    [SerializeField] private TypingSpeedSetting _typingSpeed;

    [SerializeField] private TextAnimationStyle _animationStyle;

    [SerializeField] private TextAnimationIntensity _animationIntensity;

    [SerializeField] private bool _animateSpecifiedWords;

    [SerializeField] private List<int> _specifiedWordIndexes;

    [TextArea(1, 3)] [SerializeField] private string _beforeLineEvents;

    [TextArea(3, 6)] [SerializeField] private string _line;

    [TextArea(1, 3)] [SerializeField] private string _afterLineEvents;


    public string GetName()
    {
        return _characterInfo.GetName();
    }

    public AudioClip GetVoiceClip()
    {
        return _characterInfo.GetVoiceClip();
    }


    public TypingSpeedSetting GetTypingSpeed()
    {
        return _typingSpeed;
    }

    public TextAnimationStyle GetAnimationStyle() 
    {
        return _animationStyle;
    }

    public TextAnimationIntensity GetAnimationIntensity()
    {
        return _animationIntensity;
    }

    public bool GetAnimateSpecifiedWords()
    {
        return _animateSpecifiedWords;
    }

    public List<int> GetSpecifiedWordIndexes()
    {
        return _specifiedWordIndexes;
    }

    public string GetBeforeLineEvents()
    {
        return _beforeLineEvents;
    }
    public string GetConversationLine()
    {
        return _line;
    }
    public string GetAfterLineEvents()
    {
        return _afterLineEvents;
    }

}
