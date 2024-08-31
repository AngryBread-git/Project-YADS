using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartedDialogueEventInfo : EventInfo
{
    //The dialogue trigger which started the dialogue.
    public DialogueTrigger _dialogueTrigger;
}

public class FinishedDialogueEventInfo : EventInfo
{
    public DialogueTrigger _dialogueTrigger;
}

public class SetTypingSpeedEventInfo : EventInfo
{
    //written as "{SetTypingSpeed,X}" in a Line in a DialoguePart. With X being a valid TypingSpeedSetting.
    //Note: See the DialogueSystem for the TypingSpeedSettings

    //Note: This is available as a setting in DialoguePart.
    public TypingSpeedSetting _typingSpeedSetting;
}

public class PauseTypingEventInfo : EventInfo
{
    //written as "{PauseTyping,X}" in a Line in a DialoguePart. With X being a float.
    //Note: The pause duration is in milliseconds, so {PauseTyping,1500} pauses for 1.5 seconds.
    public float _pauseDuration;
}

public class SetLineNumberEventInfo : EventInfo
{
    //written as "{SetLineNr,X}" in a Line in a DialoguePart. With X being a int.
    //Note: the DialogueParts are read into an array, line 4 is in place 3 in the array.
    //So in order to go to line 4, the event should read "{SetLineNr,3}"
    public int _lineNumber;
}

public class AutoNextLineEventInfo : EventInfo
{
    //written as "{AutoNextLine,X}" in a Line in a DialoguePart. With X being a bool value.
    //Makes it so the dialogue goes to the next line after the current line is complete. 

    //At the end of a dialogue this is set to false in DialogueSystem.
    public bool _isAutoNextLine;
}

public class ChangeMusicEventInfo : EventInfo
{
    //written as "{ChangeMusic}" in a Line in a DialoguePart.
    //Note: could have an AudioClip, but this is a simple example.
}

public class PlaySoundEventInfo : EventInfo
{
    //written as "{PlaySound,X}" in a Line in a DialoguePart. With X being a int.
    //Note: could have an AudioClip, but this is a simple example.

    public int _soundEffectNumber;
}

public class PlayDialogueBlipEventInfo : EventInfo
{
    //This event is used in DialogueSystem to play "dialogue blips".
}

public class SetTextAnimationStyleEventInfo : EventInfo
{
    //written as "{SetTextAnimationStyle,X}" in a Line in a DialoguePart. With X being a TextAnimationStyle
    //Note: See the text animator for the TextAnimationStyles.

    //Note: This is available as a setting in DialoguePart.
    public TextAnimationStyle _textAnimationStyle;
}

public class SetTextAnimationIntensityEventInfo : EventInfo
{
    //written as "{SetTextAnimationIntensity,X}" in a Line in a DialoguePart. With X being a TextAnimationIntensity
    //Note: See the text animator for the TextAnimationIntensitys and their values.

    //Note: This is available as a setting in DialoguePart.
    public TextAnimationIntensity _textAnimationIntensity;
}


public class SetSpecifiedWordAnimationEventInfo : EventInfo
{
    //NOTE: This is a setting in DialoguePart.

    //Note: See TMP_Animator for the AnimaitonsStyles
    //Note: The words are zero-indexed. And seperated by spaces.

    public bool _animateOnlyOneWord;
    public List<int> _specifiedWordIndexes = new List<int>();
}

