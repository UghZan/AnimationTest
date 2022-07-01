using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("Settings")]
    public float RotationSpeed;

    [Header("References")]
    [SerializeField] PlayerMovement PlayerMovement;
    [SerializeField] Transform PlayerModel;
    [SerializeField] Transform PlayerPelvisBone;

    Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        if(PlayerMovement.PlayerControlled)
            RotatePlayerTorsoTowardsMouse(); //move bones during LateUpdate, so that we don't clash with animations
    }

    private void Update()
    {
        if (PlayerMovement.PlayerControlled)
        {
            if (PlayerMovement.IsMoving) RotatePlayerLegsTowardsDirection();
        }
        else
            RotatePlayerTowardsTarget();
    }

    public void RotatePlayerTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation((PlayerMovement.MovementTarget - transform.position).normalized, Vector3.up); //get a rotation towards move direction
        PlayerModel.localRotation = targetRotation;
    }

    public void RotatePlayerTorsoTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10; //so that mouse position correctly transforms in ScreenToWorldPoint

        Vector3 worldMousePosition = _cam.ScreenToWorldPoint(mousePosition);
        worldMousePosition.y = transform.position.y;//so that player rotates on torso level without looking up or down

        Quaternion targetRotation = Quaternion.LookRotation((worldMousePosition - transform.position).normalized);
        Quaternion rotationCorrection = Quaternion.AngleAxis(-90, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right); //corrects bone rotation
        PlayerPelvisBone.rotation = targetRotation * rotationCorrection;
    }

    public void RotatePlayerLegsTowardsDirection()
    {
        Quaternion targetRotation = Quaternion.LookRotation(PlayerMovement.MovementVector.normalized, Vector3.up);
        PlayerModel.localRotation = Quaternion.Lerp(PlayerModel.rotation, targetRotation, Time.deltaTime * RotationSpeed); //smoothly rotate legs towards move direction
    }
}
