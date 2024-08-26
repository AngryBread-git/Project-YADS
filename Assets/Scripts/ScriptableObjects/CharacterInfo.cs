using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "ScripableObjects/CharacterSO")]
public class CharacterInfo : ScriptableObject
{
    [SerializeField] private string _characterName;
    [SerializeField] private AudioClip _voiceClip;

    public string GetName()
    {
        return _characterName;
    }

    public AudioClip GetVoiceClip()
    {
        return _voiceClip;
    }

}
