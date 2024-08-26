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

    public void SetAllowMovement(bool givenValue) 
    {
        _allowMovement = givenValue;
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
            _currentDialogueTrigger = other.GetComponent<DialogueTrigger>();
            other.GetComponent<DialogueTrigger>().ShowInteractIndicator(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(string.Format("Player OnTriggerExit, other.tag is: {0}", other.tag));

        if (other.CompareTag("DialogueTrigger"))
        {
            _currentDialogueTrigger = null;
            other.GetComponent<DialogueTrigger>().ShowInteractIndicator(false);
        }
    }
}
