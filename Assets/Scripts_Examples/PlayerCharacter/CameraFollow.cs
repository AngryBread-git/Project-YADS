using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [SerializeField]private Vector3 offset = new Vector3(0, 0, 0);
    [SerializeField] private Quaternion quaternion = new Quaternion(0, 0, 0,0);
    private void Start()
    {
        transform.rotation = quaternion;
    }
    void LateUpdate()
    {
        Vector3 target = new Vector3(player.position.x, 0, player.position.z) + offset;
        transform.position = target;
    }
}
