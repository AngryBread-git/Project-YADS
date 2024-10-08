using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUISystem : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueTextGameObject;
    [SerializeField] private GameObject _textBox;
    [SerializeField] private GameObject _nextLineIndicator;

    // Start is called before the first frame update
    void Start()
    {
        _textBox.SetActive(false);
        _dialogueTextGameObject.SetActive(false);

        SetNextLineIndicator(false);
    }

    private void OnEnable()
    {
        EventCoordinator<StartedDialogueEventInfo>.RegisterListener(ShowDialogueUI);

        EventCoordinator<FinishedDialogueEventInfo>.RegisterListener(HideDialogueUI);

    }

    private void OnDisable()
    {
        EventCoordinator<StartedDialogueEventInfo>.RegisterListener(ShowDialogueUI);

        EventCoordinator<FinishedDialogueEventInfo>.UnregisterListener(HideDialogueUI);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowDialogueUI(StartedDialogueEventInfo ei) 
    {
        _dialogueTextGameObject.SetActive(true);
        _textBox.SetActive(true);
        //NextLineIndicator is show at the end of a line, not when the dialogue starts.
        SetNextLineIndicator(false);
    }

    private void HideDialogueUI(FinishedDialogueEventInfo ei)
    {
        _dialogueTextGameObject.SetActive(false);
        _textBox.SetActive(false);
        SetNextLineIndicator(false);
    }

    public void SetNextLineIndicator(bool givenValue) 
    {
        if (_nextLineIndicator is null)
        {
            Debug.LogWarning(string.Format("DialogueUISystem, _nextLineIndicator is null."));
        }
        else 
        {
            _nextLineIndicator.SetActive(givenValue);
        }
        
    }
}
