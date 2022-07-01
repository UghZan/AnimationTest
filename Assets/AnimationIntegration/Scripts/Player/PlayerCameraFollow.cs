using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 CameraOffset;
    [SerializeField] float FollowSpeed;

    //moving camera in LateUpdate (as recommended) makes it jittery, probably because of player position/camera position desync. So I moved it in FixedUpdate and it works like a charm
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + CameraOffset, Time.deltaTime * FollowSpeed);
    }
}
