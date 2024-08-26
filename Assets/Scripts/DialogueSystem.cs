using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TypingDelaySetting
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
    [SerializeField] private GameObject _dialogueTextGameObject;
    [SerializeField] private GameObject _textBox;
    [SerializeField] private GameObject _interactIndicator;
    [SerializeField] private TextMeshProUGUI _nameText;
    private DialogueSetUpper _dialogueSetUpper;
    private DialogueSound _dialogueSound;
    private Player _player;
    private DialogueTrigger _currentDialogueTrigger;

    //Settings
    [SerializeField] private TypingDelaySetting _typingDelaySetting;
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
        ApplyTypingDelaySetting();


        _dialogueText.text = "";

        _textBox.SetActive(false);        
        _dialogueTextGameObject.SetActive(false);
        _interactIndicator.SetActive(false);

    }

    private void OnEnable()
    {
        EventCoordinator<SetTypingDelayEventInfo>.RegisterListener(SetTypingDelaySetting);

        EventCoordinator<PauseTypingEventInfo>.RegisterListener(PauseTyping);
        EventCoordinator<SetLineNumberEventInfo>.RegisterListener(SetCurrentLineNumber);
        EventCoordinator<AutoNextLineEventInfo>.RegisterListener(SetAutoNextLine);

    }

    private void OnDisable()
    {
        EventCoordinator<SetTypingDelayEventInfo>.UnregisterListener(SetTypingDelaySetting);

        EventCoordinator<PauseTypingEventInfo>.UnregisterListener(PauseTyping);
        EventCoordinator<SetLineNumberEventInfo>.UnregisterListener(SetCurrentLineNumber);
        EventCoordinator<AutoNextLineEventInfo>.UnregisterListener(SetAutoNextLine);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && _isDialogueActive)
        {
            LoadNextLine();
        }
        
    }

    public void StartDialogue(DialogueTrigger givenDialogueTrigger) 
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
        _currentDialogueTrigger = givenDialogueTrigger;


        //Lock player movement
        _player.SetAllowMovement(false);

        //activate assets.
        _textBox.SetActive(true);
        _dialogueTextGameObject.SetActive(true);
        _interactIndicator.SetActive(true);

        _dialogueText.text = "";
        _dialogueText.maxVisibleCharacters = 0;

        LoadNextLine();

    }

    private void EndDialogue() 
    {
        //Debug.Log(string.Format("DialogueSystem, EndDialogue"));
        _isDialogueActive = false;
        _autoPrintNextLine = false;

        //Unlock player movement
        _player.SetAllowMovement(true);
        //Set the current trigger to null to prevent the player from immediatly starting the next dialogue.
        _player.SetCurrentDialogueTrigger(null);

        //Increase the trigger's dialoguecount, so the next dialogue is available.
        _currentDialogueTrigger.IncreaseDialogueNr();

        //deactivate assets.
        _textBox.SetActive(false);
        _dialogueTextGameObject.SetActive(false);
        _interactIndicator.SetActive(false);
        _dialogueText.text = "";
        _dialogueText.maxVisibleCharacters = 0;

    }


    private void LoadNextLine() 
    {
        //If the player presses the input while the text is printing then the line will print instantly
        //Note: Any pauses in the line will be the normal duration.
        if (_isTyping) 
        {
            _typingDelaySetting = TypingDelaySetting.instant;
            ApplyTypingDelaySetting();

            return;
        }
        _currentLineNr += 1;
        //Debug.Log(string.Format("LoadNextLine, _currentLineNr is: {0}", _currentLineNr));

        if (_currentLineNr < _lineAmount)
        {
            _dialogueText.text = "";
            _dialogueText.maxVisibleCharacters = 0;
            //Debug.Log(string.Format("LoadNextLine, maxvisiblecharacters is: {0}", _textMeshPro.maxVisibleCharacters));

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

        LoadTypingDelay();

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


    private void LoadTypingDelay() 
    {
        _typingDelaySetting = _dialogueSetUpper.GetTypingDelay();
        ApplyTypingDelaySetting();
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
        if (givenValue)
        {
            _isTyping = true;
            if (_interactIndicator != null) 
            {
                _interactIndicator.SetActive(false);
            }           

        }
        else 
        {
            _isTyping = false;
            if (_interactIndicator != null)
            {
                _interactIndicator.SetActive(true);
            }
        }
    }

    private void ApplyTypingDelaySetting()
    {
        switch (_typingDelaySetting) 
        {
            case TypingDelaySetting.instant:
                _currentTypingDelay = _instantTypingDelay;
                break;
            case TypingDelaySetting.fast:
                _currentTypingDelay = _fastTypingDelay;
                break;
            case TypingDelaySetting.normal:
                _currentTypingDelay = _normalTypingDelay;
                break;
            case TypingDelaySetting.slow:
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

    private void SetTypingDelaySetting(SetTypingDelayEventInfo ei) 
    {
        _typingDelaySetting = ei._typingDelaySetting;
        
        ApplyTypingDelaySetting();
    }
    

    private void PauseTyping(PauseTypingEventInfo ei) 
    {
        
        if (ei._pauseDuration > 0)
        {
            //Debug.Log(string.Format("PauseTyping, ei_pauseDuration is: {0}", ei._pauseDuration));
            _pauseDuration = ei._pauseDuration;

        }
        else 
        {
            Debug.LogWarning(string.Format("PauseTyping, ei_pauseDuration was below 0: {0}", ei._pauseDuration));
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
            LoadNextLine();
        }
    }

    //The event types are not dynamic, so the calls have to be explicit
    //The switch case is used to make explicit calls.
    private void FireFormattedEvent(FormattedEvent tempEvent) 
    {
        switch (tempEvent.Info) 
        {
            case SetTypingDelayEventInfo ei:
                EventCoordinator<SetTypingDelayEventInfo>.FireEvent(ei);
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
