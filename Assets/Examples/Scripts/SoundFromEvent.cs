using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFromEvent : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventCoordinator<PlaySoundEventInfo>.RegisterListener(PlayEventSound);

    }
    private void OnDisable()
    {
        EventCoordinator<PlaySoundEventInfo>.UnregisterListener(PlayEventSound);

    }

    //The AudioClips are stored in a simple array. There is definetly potential for a more advanced sound system.
    private void PlayEventSound(PlaySoundEventInfo ei) 
    {
        //Debug.Log(string.Format("PlayEventSound, _soundEffectNumber is: {0}", ei._soundEffectNumber));

        if (!_audioSource.isPlaying && _audioClips.Length > ei._soundEffectNumber) 
        {
            _audioSource.PlayOneShot(_audioClips[ei._soundEffectNumber]);
        }
    }



}
