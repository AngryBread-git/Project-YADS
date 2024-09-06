using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueFormatter))]
public class DialogueSetUpper : MonoBehaviour
{
    private DialoguePart[] _dialogueParts;
    private DialogueFormatter _dialogueFormatter;
    private int _currentLineNr;

    void Start()
    {
        _dialogueFormatter = GetComponent<DialogueFormatter>();
    }


    public int GetDialogueLength() 
    {
        return _dialogueParts.Length;
    }

    public void SaveDialogueParts(DialogueSO dialogueSO) 
    {
        _dialogueParts = dialogueSO.GetDialogueParts();
        //Debug.Log(string.Format("SaveDP, _dialogueParts.Length is: {0}", _dialogueParts.Length));
    }

    public void SetCurrentLineNr(int givenCurrentLineNr) 
    {
        _currentLineNr = givenCurrentLineNr;
    }

    public List<FormattedContent> GetCurrentLineAsFormattedContent() 
    {

        //Debug.Log(string.Format("Get, _dialogueParts.Length is: {0}", _dialogueParts.Length));
        //Debug.Log(string.Format("Get, currentLineNr is: {0}", currentLineNr));
        string currentLine = _dialogueParts[_currentLineNr].GetConversationLine();

        List<FormattedContent> result = _dialogueFormatter.FormatLine(currentLine);

        return result;
    }

    public List<FormattedEvent> GetBeforeLineEvents() 
    {
        string tempString = _dialogueParts[_currentLineNr].GetBeforeLineEvents();

        List<FormattedEvent> result = _dialogueFormatter.FormatEvents(tempString);
        return result;      
    }

    public List<FormattedEvent> GetAfterLineEvents()
    {
        //Debug.Log(string.Format("GetAfterLineEvents, _dialogueParts.Length is: {0}", _dialogueParts.Length));
        //Debug.Log(string.Format("GetAfterLineEvents, currentLineNr is: {0}", currentLineNr));
        string tempString = _dialogueParts[_currentLineNr].GetAfterLineEvents();
        
        List<FormattedEvent> result = _dialogueFormatter.FormatEvents(tempString);
        return result;
    }

    public string GetCharacterName() 
    {
        return _dialogueParts[_currentLineNr].GetName();
    }

    public AudioClip GetVoiceClip()
    {
        return _dialogueParts[_currentLineNr].GetVoiceClip();
    }


    public TypingSpeedSetting GetTypingSpeed()
    {
        return _dialogueParts[_currentLineNr].GetTypingSpeed();
    }



    public TextAnimationStyle GetAnimationStyle() 
    {
        return _dialogueParts[_currentLineNr].GetAnimationStyle();
    }

    public TextAnimationIntensity GetAnimationIntensity() 
    {
        return _dialogueParts[_currentLineNr].GetAnimationIntensity();
    }

    public bool GetAnimateSpecifiedWords() 
    {
        return _dialogueParts[_currentLineNr].GetAnimateSpecifiedWords();
    }

    public List<int> GetSpecifiedWordIndexes()
    {
        return _dialogueParts[_currentLineNr].GetSpecifiedWordIndexes();
    }


}
