using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    public float _movementSpeed = 10.0f;
    private bool _allowMovement = true;
    private DialogueTrigger _currentDialogueTrigger;

    private void OnEnable()
    {
        EventCoordinator<StartedDialogueEventInfo>.RegisterListener(DisallowMovement);

        EventCoordinator<FinishedDialogueEventInfo>.RegisterListener(AllowMovement);

    }

    private void OnDisable()
    {
        EventCoordinator<StartedDialogueEventInfo>.RegisterListener(DisallowMovement);

        EventCoordinator<FinishedDialogueEventInfo>.UnregisterListener(AllowMovement);
    }

    void Update()
    {
        if (_allowMovement == false) 
        {
            return;
        }

        Vector3 input = Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward;

        transform.position += input.normalized * _movementSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && _currentDialogueTrigger != null) 
        {
            _currentDialogueTrigger.InitializeDialogue();

        }
    }

    private void DisallowMovement(StartedDialogueEventInfo ei) 
    {
        _allowMovement = false;
        SetCurrentDialogueTrigger(null);
    }

    private void AllowMovement(FinishedDialogueEventInfo ei)
    {
        _allowMovement = true;
    }

    public void SetCurrentDialogueTrigger(DialogueTrigger givenDialogueTrigger) 
    {
        _currentDialogueTrigger = givenDialogueTrigger;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(string.Format("Player OnTriggerEnter, other.tag is: {0}", other.tag));

        if (other.CompareTag("DialogueTrigger"))
        {
            SetCurrentDialogueTrigger(other.GetComponent<DialogueTrigger>());
            other.GetComponent<DialogueTrigger>().ShowInteractIndicator(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(string.Format("Player OnTriggerExit, other.tag is: {0}", other.tag));

        if (other.CompareTag("DialogueTrigger"))
        {
            SetCurrentDialogueTrigger(null);
            other.GetComponent<DialogueTrigger>().ShowInteractIndicator(false);
        }
    }
}
