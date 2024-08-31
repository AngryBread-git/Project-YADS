using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TypingSpeedSetting
{
    //Note: The enums are defined in this order to make "normal" the default value in DialoguePart
    normal,
    fast,
    slow,
    instant,
}


public class DialogueSystem : MonoBehaviour
{
    //Other Classes
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [SerializeField] private TextMeshProUGUI _nameText;

    private DialogueSetUpper _dialogueSetUpper;
    private DialogueSound _dialogueSound;
    private Player _player;
    private DialogueTrigger _currentDialogueTrigger;
    private DialogueUISystem _dialogueUISystem;

    //Each TypingDelay corresponds to a TypingSpeed. 
    //A low value on a delay gives a high typing speed. A high value on a delay gives a low typing speed.
    [SerializeField] private TypingSpeedSetting _typingSpeedSetting;
    private float _instantTypingDelay = 0.001f;
    [SerializeField] [Range(0.01f, 0.3f)] private float _fastTypingDelay;
    [SerializeField] [Range(0.01f, 0.3f)] private float _normalTypingDelay;
    [SerializeField] [Range(0.01f, 0.3f)] private float _slowTypingDelay;



    //Status values
    private bool _isDialogueActive;
    private bool _isTyping;
    private bool _isPaused;
    private float _pauseDuration;
    private bool _autoPrintNextLine;
    private float _currentTypingDelay;
    private List<FormattedContent> _formattedLine;

    [SerializeField] private int _lineAmount;
    [SerializeField] private int _currentLineNr = 0;


    void Start()
    {
        _dialogueSetUpper = FindObjectOfType<DialogueSetUpper>();
        _player = FindObjectOfType<Player>();
        _dialogueSound = FindObjectOfType<DialogueSound>();
        _dialogueUISystem = FindObjectOfType<DialogueUISystem>();

        ApplyTypingSpeedSetting();


        _dialogueText.text = "";
    }

    private void OnEnable()
    {
        EventCoordinator<StartedDialogueEventInfo>.RegisterListener(StartDialogue);
        EventCoordinator<SetTypingSpeedEventInfo>.RegisterListener(SetTypingSpeedSetting);

        EventCoordinator<PauseTypingEventInfo>.RegisterListener(PauseTyping);
        EventCoordinator<SetLineNumberEventInfo>.RegisterListener(SetCurrentLineNumber);
        EventCoordinator<AutoNextLineEventInfo>.RegisterListener(SetAutoNextLine);

    }

    private void OnDisable()
    {
        EventCoordinator<StartedDialogueEventInfo>.RegisterListener(StartDialogue);
        EventCoordinator<SetTypingSpeedEventInfo>.UnregisterListener(SetTypingSpeedSetting);

        EventCoordinator<PauseTypingEventInfo>.UnregisterListener(PauseTyping);
        EventCoordinator<SetLineNumberEventInfo>.UnregisterListener(SetCurrentLineNumber);
        EventCoordinator<AutoNextLineEventInfo>.UnregisterListener(SetAutoNextLine);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && _isDialogueActive)
        {
            LoadNextDialoguePart();
        }
        
    }

    private void StartDialogue(StartedDialogueEventInfo ei) 
    {
        //If there's a dialogue active, then dont start a new one.
        if (_isDialogueActive) 
        {
            return;
        }

        //set values
        _isDialogueActive = true;

        //currentLineNr starts at -1, then in LoadNextLine it is increased by 1, meaning that it starts at line 0.
        _currentLineNr = -1;
        _lineAmount = _dialogueSetUpper.GetDialogueLength();
        _currentDialogueTrigger = ei._dialogueTrigger;

        _dialogueText.text = "";
        _dialogueText.maxVisibleCharacters = 0;

        LoadNextDialoguePart();
    }

    private void EndDialogue() 
    {
        //Debug.Log(string.Format("DialogueSystem, EndDialogue"));
        _isDialogueActive = false;
        _autoPrintNextLine = false;

        FinishedDialogueEventInfo ei = new FinishedDialogueEventInfo();
        ei._dialogueTrigger = _currentDialogueTrigger;
        EventCoordinator<FinishedDialogueEventInfo>.FireEvent(ei);

        //Set the text to nothing.
        _dialogueText.text = "";
        _dialogueText.maxVisibleCharacters = 0;
    }


    private void LoadNextDialoguePart() 
    {
        //If the player presses the input while the text is printing then the line will print very very fast.
        if (_isTyping) 
        {
            _typingSpeedSetting = TypingSpeedSetting.instant;
            ApplyTypingSpeedSetting();

            return;
        }
        _currentLineNr += 1;
        //Debug.Log(string.Format("LoadNextDialoguePart, _currentLineNr is: {0}", _currentLineNr));

        if (_currentLineNr < _lineAmount)
        {
            _dialogueText.text = "";
            _dialogueText.maxVisibleCharacters = 0;
            //Debug.Log(string.Format("LoadNextDialoguePart, maxvisiblecharacters is: {0}", _textMeshPro.maxVisibleCharacters));

            //Play sound for the new line.
            PlaySoundEventInfo ei = new PlaySoundEventInfo();
            ei._soundEffectNumber = 0;
            EventCoordinator<PlaySoundEventInfo>.FireEvent(ei);

            _dialogueSetUpper.SetCurrentLineNr(_currentLineNr);
            LoadInfoBeforeLine();                        
            _formattedLine = _dialogueSetUpper.GetCurrentLineAsFormattedContent();

            StartCoroutine(PrintLine(_formattedLine));

        }
        else if (_currentLineNr >= _lineAmount) 
        {
            EndDialogue();
        }
    }

    #region LoadInfoBeforeLine

    private void LoadInfoBeforeLine() 
    {
        //Debug.Log(string.Format("DialogueSystem, LoadInfoBeforeLine"));

        LoadCharacterName();

        LoadVoiceClip();

        LoadTypingSpeed();

        LoadAnimationStyle();

        LoadAnimationIntensity();

        LoadAnimateSpecifiedWords();

        LoadBeforeLineEvents();
    }

    private void LoadCharacterName() 
    {
        _nameText.text = _dialogueSetUpper.GetCharacterName();
    }

    private void LoadVoiceClip() 
    {
        _dialogueSound.ChangeVoiceClip(_dialogueSetUpper.GetVoiceClip());
    }


    private void LoadTypingSpeed() 
    {
        _typingSpeedSetting = _dialogueSetUpper.GetTypingSpeed();
        ApplyTypingSpeedSetting();
    }

    private void LoadAnimationStyle() 
    {
        //Debug.Log(string.Format("DialogueSystem, LoadAnimationStyle"));
        TextAnimationStyle animationStyle = _dialogueSetUpper.GetAnimationStyle();

        SetTextAnimationStyleEventInfo ei = new SetTextAnimationStyleEventInfo();
        ei._textAnimationStyle = animationStyle;
        EventCoordinator<SetTextAnimationStyleEventInfo>.FireEvent(ei);
    }


    private void LoadAnimationIntensity()
    {
        //Debug.Log(string.Format("DialogueSystem, LoadAnimationIntensity"));
        TextAnimationIntensity animationIntensity = _dialogueSetUpper.GetAnimationIntensity();

        SetTextAnimationIntensityEventInfo ei = new SetTextAnimationIntensityEventInfo();
        ei._textAnimationIntensity = animationIntensity;
        EventCoordinator<SetTextAnimationIntensityEventInfo>.FireEvent(ei);
    }

    private void LoadAnimateSpecifiedWords() 
    {
        bool animateSpecifiedWords = _dialogueSetUpper.GetAnimateSpecifiedWords();

        List<int> specifiedWordIndexes = _dialogueSetUpper.GetSpecifiedWordIndexes();

        SetSpecifiedWordAnimationEventInfo ei = new SetSpecifiedWordAnimationEventInfo();
        ei._animateOnlyOneWord = animateSpecifiedWords;
        ei._specifiedWordIndexes = specifiedWordIndexes;

        EventCoordinator<SetSpecifiedWordAnimationEventInfo>.FireEvent(ei);

    }

    //PreLineEvents
    private void LoadBeforeLineEvents() 
    {
        List<FormattedEvent> formattedEvents = _dialogueSetUpper.GetBeforeLineEvents();

        if (formattedEvents.Count < 1)
        {
            //Debug.Log(string.Format("DialogueSystem, no Events before line."));
        }
        else 
        {
            for (int i = 0; i < formattedEvents.Count; i++) 
            {
                FireFormattedEvent(formattedEvents[i]);
            }
        }

    }


    #endregion LoadInfoBeforeLine


    #region LoadInfoAfterLine

    private void LoadInfoAfterLine() 
    {
        //Debug.Log(string.Format("DialogueSystem, LoadInfoAfterLine"));
        LoadAfterLineEvents();
    }


    private void LoadAfterLineEvents()
    {
        List<FormattedEvent> formattedEvents = _dialogueSetUpper.GetAfterLineEvents();

        if (formattedEvents.Count < 1)
        {
            //Debug.Log(string.Format("DialogueSystem, no Events after line."));
        }
        else
        {
            for (int i = 0; i < formattedEvents.Count; i++)
            {
                FireFormattedEvent(formattedEvents[i]);
            }
        }

    }

    #endregion LoadInfoAfterLine

    private void SetIsTyping(bool givenValue) 
    {

        _isTyping = givenValue;

        _dialogueUISystem.SetNextLineIndicator(!givenValue);
    }

    private void ApplyTypingSpeedSetting()
    {
        switch (_typingSpeedSetting) 
        {
            case TypingSpeedSetting.instant:
                _currentTypingDelay = _instantTypingDelay;
                break;
            case TypingSpeedSetting.fast:
                _currentTypingDelay = _fastTypingDelay;
                break;
            case TypingSpeedSetting.normal:
                _currentTypingDelay = _normalTypingDelay;
                break;
            case TypingSpeedSetting.slow:
                _currentTypingDelay = _slowTypingDelay;
                break;
            default:
                Debug.LogWarning(string.Format("ApplyTypingDelaySetting, setting was not of a recognized type."));
                _currentTypingDelay = _normalTypingDelay;
                break;
        }

        
    }

    #region EventListeners
    private void SetCurrentLineNumber(SetLineNumberEventInfo ei) 
    {
        if (ei._lineNumber >= _lineAmount - 1)
        {
            Debug.LogWarning(string.Format("SetCurrentLineNumber, ei._lineNumber: {0} was greater than _currentTextFileLineAmount: {1}. _currentLineNR will not be changed", ei._lineNumber, _lineAmount));

        }
        else 
        {
            //NOTE: _currentLineNr is increased by 1 in load next line. In order to load Line X then _currentLineNr is decreased by 1.
            _currentLineNr = ei._lineNumber - 1;
            Debug.Log(string.Format("SetCurrentLineNumber, _currentLineNr is now: {0}", _currentLineNr));
            Debug.Log(string.Format("SetCurrentLineNumber, next line will be: {0}", _currentLineNr + 1));
        }
        
    }

    private void SetAutoNextLine(AutoNextLineEventInfo ei) 
    {
        _autoPrintNextLine = ei._isAutoNextLine;
    }

    private void SetTypingSpeedSetting(SetTypingSpeedEventInfo ei) 
    {
        if (_typingSpeedSetting == TypingSpeedSetting.instant)
        {
            //Debug.Log(string.Format("SetTypingSpeedSetting, currently at Instant. Speed will not change from event."));
            return;
        }

        _typingSpeedSetting = ei._typingSpeedSetting;
        
        ApplyTypingSpeedSetting();
    }
    

    private void PauseTyping(PauseTypingEventInfo ei) 
    {

        //Uncomment the statement below to make it so that instant TypingSpeed will ignore pauses in dialogue.
        /*if (_typingDelaySetting == TypingDelaySetting.instant)
        {
            Debug.Log(string.Format("PauseTyping, currently at Instant. Pause will not be applied from event."));
            return;
        }
        */
        

        if (ei._pauseDuration > 0)
        {
            //Debug.Log(string.Format("PauseTyping, ei_pauseDuration is: {0}", ei._pauseDuration));
            _pauseDuration = ei._pauseDuration;
        }
        else 
        {
            //Debug.LogWarning(string.Format("PauseTyping, ei_pauseDuration was below 0: {0}. Duration will be set to 1.0", ei._pauseDuration));
            _pauseDuration = 1.0f;
        }

        _isPaused = true;
    }

    #endregion EventListeners


    private IEnumerator PrintLine(List<FormattedContent> currentLine) 
    {
        SetIsTyping(true);

        for (int i = 0; i < currentLine.Count; i++) 
        {
           
            //If the FormattedContent is a tag, then type it out all at once to apply it.
            if (currentLine[i].ContentType == ContentType.tag)
            {
                FormattedTag tempTag = (FormattedTag)currentLine[i];
                _dialogueText.text += tempTag.Tag;
            }
            //If the FormattedContent is an Event, then fire it.
            else if (currentLine[i].ContentType == ContentType.eventinfo)
            {
                FormattedEvent tempEvent = (FormattedEvent)currentLine[i];
                FireFormattedEvent(tempEvent);

            }
            //Else if it's a string then type it out one character at a time.
            else if (currentLine[i].ContentType == ContentType.text)
            {

                FormattedText tempText = (FormattedText)currentLine[i];

                string lineSubsection = tempText.Text;
                //Add the string to the textobject.
                _dialogueText.text += lineSubsection;

                for (int j = 0; j < lineSubsection.Length; j++)
                {

                    _dialogueText.maxVisibleCharacters += 1;
                    //Debug.Log(string.Format("Printing line, maxvisiblecharacters is: {0}", _textMeshPro.maxVisibleCharacters));

                    //if the character is a space, use a short delay. Also do not play a sound.
                    if (lineSubsection[j].Equals(' '))
                    {
                        yield return new WaitForSeconds(_currentTypingDelay * 0.5f);
                    }
                    //If it's punctuation then the typing has a slightly longer delay.
                    if (lineSubsection[j].Equals('.') || lineSubsection[j].Equals(',') || lineSubsection[j].Equals('!') || lineSubsection[j].Equals('?'))
                    {
                        _dialogueSound.PlayDialogueSound();
                        yield return new WaitForSeconds(_currentTypingDelay * 1.2f);
                    }
                    else
                    {
                        _dialogueSound.PlayDialogueSound();
                        yield return new WaitForSeconds(_currentTypingDelay);
                    }
                    
                }
                
            }
            
            //if a PauseEvent is fired then wait for the duration, then continue.
            if (_isPaused) 
            {
                yield return new WaitForSeconds(_pauseDuration);
                _pauseDuration = 0.0f;
                _isPaused = false;
            }

        }

        SetIsTyping(false);
        LoadInfoAfterLine();

        if (_autoPrintNextLine) 
        {            
            LoadNextDialoguePart();
        }
    }

    //The event types are not dynamic, so the calls have to be explicit
    //The switch case is used to make explicit calls.
    private void FireFormattedEvent(FormattedEvent tempEvent) 
    {
        switch (tempEvent.Info) 
        {
            case SetTypingSpeedEventInfo ei:
                EventCoordinator<SetTypingSpeedEventInfo>.FireEvent(ei);
                break;

            case PauseTypingEventInfo ei:               
                EventCoordinator<PauseTypingEventInfo>.FireEvent(ei);
                break;
            case SetLineNumberEventInfo ei:
                EventCoordinator<SetLineNumberEventInfo>.FireEvent(ei);
                break;
            case AutoNextLineEventInfo ei:
                EventCoordinator<AutoNextLineEventInfo>.FireEvent(ei);
                break;
            case ChangeMusicEventInfo ei:
                EventCoordinator<ChangeMusicEventInfo>.FireEvent(ei);
                break;
            case PlaySoundEventInfo ei:
                EventCoordinator<PlaySoundEventInfo>.FireEvent(ei);
                break;

            case SetTextAnimationStyleEventInfo ei:
                EventCoordinator<SetTextAnimationStyleEventInfo>.FireEvent(ei);
                break;

            case SetTextAnimationIntensityEventInfo ei:
                EventCoordinator<SetTextAnimationIntensityEventInfo>.FireEvent(ei);
                break;

            case SetSpecifiedWordAnimationEventInfo ei:
                EventCoordinator<SetSpecifiedWordAnimationEventInfo>.FireEvent(ei);
                break;

            case DebugEventInfo ei:
                Debug.LogWarning(string.Format("DebugEvent: {0}", ei.EventDescription));
                break;
            case null:
                Debug.LogWarning(string.Format("Not a valid event."));
                break;
        }
    }

}
