using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueSound : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool _allowSound = true;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ChangeVoiceClip(AudioClip audioClip) 
    {
        _audioSource.clip = audioClip;
    }

    public void PlayDialogueSound()
    {
        //Debug.Log(string.Format("PlayDialogueSound, _allowSound is: {0}", _allowSound));
        if (_allowSound)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.Play();
            _allowSound = false;

            float delay = Random.Range(0.1f, 0.2f);
            StartCoroutine(AllowSoundAfterDelay(delay));
        }
    }

    private IEnumerator AllowSoundAfterDelay(float delay)
    {
        //Debug.Log(string.Format("before delay, _allowSound is: {0}", _allowSound));
        yield return new WaitForSeconds(delay);
        _allowSound = true;
        //Debug.Log(string.Format("after delay, _allowSound is: {0}", _allowSound));
    }

}
