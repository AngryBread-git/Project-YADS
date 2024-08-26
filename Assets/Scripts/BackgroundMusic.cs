using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;
    private int _currentAudioClip = 0;
    private float _orgVolume;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _orgVolume = _audioSource.volume;

        _audioSource.clip = _audioClips[0];
        _audioSource.Play();
    }

    private void OnEnable()
    {
        EventCoordinator<ChangeMusicEventInfo>.RegisterListener(ChangeBackgroundMusic);
    }
    private void OnDisable()
    {
        EventCoordinator<ChangeMusicEventInfo>.UnregisterListener(ChangeBackgroundMusic);
    }


    private void ChangeBackgroundMusic(ChangeMusicEventInfo ei) 
    {

        _currentAudioClip += 1;

        if (_audioClips.Length <= _currentAudioClip)
        {
            _currentAudioClip = 0;
            StartCoroutine(CrossFace(_currentAudioClip));
        }
        else 
        {
            StartCoroutine(CrossFace(_currentAudioClip));
        }
        
    }

    private IEnumerator CrossFace(int audioClipNr) 
    {
        yield return StartCoroutine(FadeVolume(2.0f,0));
        _audioSource.clip = _audioClips[audioClipNr];
        _audioSource.Play();

        yield return StartCoroutine(FadeVolume(2.0f, _orgVolume));
    }

    private IEnumerator FadeVolume(float duration, float targetVolume)
    {
        float currentTime = 0;

        float startVolume = _audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
