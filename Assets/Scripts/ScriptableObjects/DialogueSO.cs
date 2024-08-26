using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScripableObjects/DialogueSO")]
public class DialogueSO : ScriptableObject
{

    [SerializeField] private DialoguePart[] _dialogueParts;

    public int LengthOfPartsArray()
    {
        return _dialogueParts.Length;
    }

    public DialoguePart[] GetDialogueParts()
    {
        return _dialogueParts;
    }
}
