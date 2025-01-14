using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CameraFollow _cameraFollow;

    private Transform _cameraTransform;
    private Vector3 _originalPosistion;

    private bool _currentlyShaking = false;
    private float _currentShakeDuration = 1.0f;
    private float _currentShakeAmount = 0.01f;

    private float _lightShakeDuration = 1.0f;
    private float _lightShakeAmount = 0.01f;

    private float _mediumShakeDuration = 1.0f;
    private float _mediumShakeAmount = 0.03f;

    private float _heavyShakeDuration = 1.2f;
    private float _heavyShakeAmount = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = gameObject.GetComponent<Transform>();
        _cameraFollow = gameObject.GetComponent<CameraFollow>();

        //StartCameraShake();
    }

    private void OnEnable()
    {
        EventCoordinator<CameraShakeEventInfo>.RegisterListener(CameraShakeFromEvent);

    }
    private void OnDisable()
    {
        EventCoordinator<CameraShakeEventInfo>.UnregisterListener(CameraShakeFromEvent);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_currentlyShaking)
        {
            if (_currentShakeDuration > 0)
            {

                Debug.Log(string.Format("in CameraShake"));


                Vector3 newOffset = Random.insideUnitSphere * _currentShakeAmount;
                Debug.Log(string.Format("newOffset is: {0}", newOffset));

                _cameraFollow.AddOffset(newOffset);

                _currentShakeDuration -= Time.deltaTime;
            }
            else 
            {
                _cameraFollow.ResetOffset();
                //_cameraTransform.localPosition = _originalPosistion;
                _currentlyShaking = false;
                Debug.Log(string.Format("Stopped CameraShake"));
            }

        }

        //For testing
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            StartCameraShake();
        }

    }

    private void CameraShakeFromEvent(CameraShakeEventInfo ei)
    {
        if (ei._cameraShakeStrength == CameraShakeStrength.light)
        {
            _currentShakeAmount = _lightShakeAmount;
            _currentShakeDuration = _lightShakeDuration;
        }
        else if (ei._cameraShakeStrength == CameraShakeStrength.medium) 
        {
            _currentShakeAmount = _mediumShakeAmount;
            _currentShakeDuration = _mediumShakeDuration;
        }
        else if (ei._cameraShakeStrength == CameraShakeStrength.heavy)
        {
            _currentShakeAmount = _heavyShakeAmount;
            _currentShakeDuration = _heavyShakeDuration;
        }

        _originalPosistion = _cameraTransform.localPosition;
        _currentlyShaking = true;
    }

    public void StartCameraShake()
    {
        Debug.Log(string.Format("StartCameraShake"));

        _originalPosistion = _cameraTransform.localPosition;
        Debug.Log(string.Format("_originalPosistion is: {0}", _originalPosistion));
        //TODO: use given values from event.
        _currentShakeAmount = 1.0f;
        _currentlyShaking = true;
    }
}
