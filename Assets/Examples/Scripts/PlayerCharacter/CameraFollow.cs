using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);
    private Vector3 _orgOffset;

    //TODO: seperate value for rotation used to screenshake.

    [SerializeField] private Quaternion quaternion = new Quaternion(0, 0, 0,0);
    private void Start()
    {
        transform.rotation = quaternion;
        _orgOffset = offset;
    }
    void LateUpdate()
    {
        Vector3 target = new Vector3(player.position.x, 0, player.position.z) + offset;
        transform.position = target;
    }

    //This method is used for a basic camera shake. If cinemachine is used, a more complex shake effect can be used.
    public void AddOffset(Vector3 givenOffset) 
    {
        offset = _orgOffset + givenOffset;
    }

    public void ResetOffset() 
    {
        offset = _orgOffset;
    }
}
