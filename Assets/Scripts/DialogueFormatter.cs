using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFormatter : MonoBehaviour
{
    public List<FormattedContent> FormatLine(string line) 
    {
        List<FormattedContent> result = new List<FormattedContent>();

        string tempContent = "";

        for (int i = 0; i < line.Length; i++)
        {
            char tempChar = line[i];

            //<> for tags. {} for events.
            //NOTE: an event can not be inside a tag, or vice versa.
            if (tempChar == '<')
            {
                //Debug.Log(string.Format("Finished formatting text, text is: {0}", tempContent));

                result.Add(new FormattedText(tempContent));
                tempContent = "<";
            }
            else if (tempChar == '>')
            {
                //finish tag
                tempContent += tempChar;
                //Debug.Log(string.Format("Finished formatting tag, tag is: {0}", tempContent));

                result.Add(new FormattedTag(tempContent));
                tempContent = "";
            }

            else if (tempChar == '{')
            {
                //Debug.Log(string.Format("Finished formatting text, text is: {0}", tempContent));

                result.Add(new FormattedText(tempContent));
                //The '{' is not part of the EventInfo and as such is not saved.
                tempContent = "";
            }

            else if (tempChar == '}')
            {
                //finish event
                //The '}' is not part of the EventInfo and as such is not saved.
                //Debug.Log(string.Format("Finished formatting eventcall, eventcall is: {0}", tempContent));

                EventInfo tempEventInfo = FormatEventCall(tempContent);
                result.Add(new FormattedEvent(tempEventInfo));
                tempContent = "";
            }

            else
            {
                tempContent += tempChar;
            }

        }
        //Add the remaining string at end of line.
        //Debug.Log(string.Format("Done with line, remainder-string is: {0}", tempContent));
        result.Add(new FormattedText(tempContent));

        //Debug.Log(string.Format("Done with line, amount of FormattedStrings: {0}", result.Count));
        return result;
    }

    public List<FormattedEvent> FormatEvents(string line)
    {
        List<FormattedEvent> result = new List<FormattedEvent>();

        string tempContent = "";

        for (int i = 0; i < line.Length; i++)
        {
            char tempChar = line[i];

            if (tempChar == '{')
            {
                //An event starts with an open curly brace.
                //The '{' is not part of the EventInfo and as such is not saved.
                tempContent = "";
            }

            else if (tempChar == '}')
            {
                //An event ends with a closed curly brase.
                //The '}' is not part of the EventInfo and as such is not saved.
                //Debug.Log(string.Format("Finished formatting eventcall, eventcall is: {0}", tempContent));

                EventInfo tempEventInfo = FormatEventCall(tempContent);
                result.Add(new FormattedEvent(tempEventInfo));
                tempContent = "";
            }

            else
            {
                tempContent += tempChar;
            }
        }

        //Debug.Log(string.Format("Done with evnets, amount of FormattedEvents: {0}", result.Count));
        return result;
    }



    private EventInfo FormatEventCall(string eventInfoString)
    {
        //Events are seperated into parts by ','. See DialogueEvents for more info.
        string[] splitString = eventInfoString.Split(',');
        //Debug.LogWarning(string.Format("in FormatEventCall: splitString is: {0}", splitString));

        switch (splitString[0])
        {
            case "SetTypingSpeed":
                SetTypingSpeedEventInfo stsResult = new SetTypingSpeedEventInfo();
                stsResult._typingSpeedSetting = (TypingSpeedSetting)Enum.Parse(typeof(TypingSpeedSetting), splitString[1]);
                return stsResult;

            case "PauseTyping":
                PauseTypingEventInfo ptResult = new PauseTypingEventInfo();
                float _pauseInMiliseconds = float.Parse(splitString[1]);
                ptResult._pauseDuration = _pauseInMiliseconds / 1000;
                return ptResult;

            case "SetLineNr":
                SetLineNumberEventInfo slnResult = new SetLineNumberEventInfo();
                slnResult._lineNumber = Convert.ToInt32(splitString[1]);
                return slnResult;

            case "AutoNextLine":
                AutoNextLineEventInfo anlResult = new AutoNextLineEventInfo();
                anlResult._isAutoNextLine = bool.Parse(splitString[1]);
                return anlResult;

            case "ChangeMusic":
                ChangeMusicEventInfo cmResult = new ChangeMusicEventInfo();
                return cmResult;

            case "PlaySound":
                PlaySoundEventInfo psResult = new PlaySoundEventInfo();
                psResult._soundEffectNumber = Convert.ToInt32(splitString[1]);
                return psResult;

            case "SetTextAnimationStyle":
                SetTextAnimationStyleEventInfo stasResult = new SetTextAnimationStyleEventInfo();
                stasResult._textAnimationStyle = (TextAnimationStyle)Enum.Parse(typeof(TextAnimationStyle), splitString[1]);
                return stasResult;

            case "SetTextAnimationIntensity":
                SetTextAnimationIntensityEventInfo staiResult = new SetTextAnimationIntensityEventInfo();
                staiResult._textAnimationIntensity = (TextAnimationIntensity)Enum.Parse(typeof(TextAnimationIntensity), splitString[1]);
                return staiResult;


            default:
                Debug.LogWarning(string.Format("Default in FormatEventCall. EventType {0} does not exist.", splitString[0]));
                DebugEventInfo debugResult = new DebugEventInfo();
                debugResult.EventDescription = ("Default in FormatEventCall. EventType does not exist.");
                debugResult.PriorityLevel = 1;
                return debugResult;
        }

    }

}
