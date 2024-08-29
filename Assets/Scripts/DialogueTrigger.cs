using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSO[] _dialogueSO;
    [SerializeField] private int _currentDialogueNr = 0;
    private DialogueSetUpper _dialogueSetUpper;
    private DialogueSystem _dialogueSystem;
    private SpriteRenderer _interactIndicator;

    // Start is called before the first frame update
    private void Start()
    {
        _dialogueSetUpper = FindObjectOfType<DialogueSetUpper>();
        _dialogueSystem = FindObjectOfType<DialogueSystem>();

        _interactIndicator = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EventCoordinator<FinishedDialogueEventInfo>.RegisterListener(OnDialogueFinished);

    }

    private void OnDisable()
    {

        EventCoordinator<FinishedDialogueEventInfo>.UnregisterListener(OnDialogueFinished);
    }


    public void InitializeDialogue() 
    {
        ShowInteractIndicator(false);

        _dialogueSetUpper.SaveDialogueParts(_dialogueSO[_currentDialogueNr]);

        StartedDialogueEventInfo ei = new StartedDialogueEventInfo();
        ei._dialogueTrigger = this;
        EventCoordinator<StartedDialogueEventInfo>.FireEvent(ei);
    }


    private void OnDialogueFinished(FinishedDialogueEventInfo ei) 
    {
        if (this.Equals(ei._dialogueTrigger)) 
        {
            IncreaseDialogueNr();
        }
    }

    private void IncreaseDialogueNr() 
    {
        if (_currentDialogueNr + 1 < _dialogueSO.Length)
        {
            _currentDialogueNr += 1;
        }
        else 
        {
            //Debug.Log(string.Format("DialogueTrigger, finished last dialogue in array."));
        }
    }

    public void ShowInteractIndicator(bool isActive) 
    {
        _interactIndicator.enabled = isActive;
    }

}
