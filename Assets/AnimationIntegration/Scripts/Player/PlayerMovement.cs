using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player controls depending on camera angle
//Uses Input System and Character Controller
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float PlayerMaxSpeed;

    [Header("Movement To Target Settings")]
    public float TargetBias; //how close should player be to target zone to count as "reached the target"

    [Header("Misc")]
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] Rigidbody Rigidbody;

    Camera _mainCamera;

    Vector3 MovementDirection; //movement direction from input
    public Vector3 MovementVector; //final direction

    public Vector3 MovementTarget;
    public bool IsMoving { get; private set; }
    public bool HasReachedTarget { get; private set; }

    [HideInInspector] public bool PlayerControlled = true; //if false, moves towards set target without player input
    [HideInInspector] public bool ShouldMove = true; //this variable is used to prevent animation and script clashing for position of the player, making him shake like crazy

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        GetMovementStatus();
        PlayerAnimator.SetBool("moving", IsMoving);
        UpdateMovementVector();
    }

    private void FixedUpdate()
    {
        if (!ShouldMove) return;

        Rigidbody.MovePosition(transform.position + PlayerMaxSpeed * Time.deltaTime * MovementVector);
    }

    //switch between control modes
    //if we go to player-controlled mode, reset non-controlled mode-related variables
    public void SwitchMovementMode(bool mode)
    {
        PlayerControlled = mode;
        if(mode)
        {
            HasReachedTarget = false;
        }
    }

    //move in direction set by camera
    void UpdateMovementVector()
    {
        if (PlayerControlled)
        {
            MovementDirection = GetMovementDirection();
            Vector3 camForward = Vector3.ProjectOnPlane(_mainCamera.transform.forward, Vector3.up).normalized, //project cam forward direction to floor
            camRight = Vector3.ProjectOnPlane(_mainCamera.transform.right, Vector3.up).normalized,
            nextVector = MovementDirection.y * camForward + MovementDirection.x * camRight;

            MovementVector = nextVector; // get a movement direction related to camera forward direction
        }
        else
        {
            MovementVector = (MovementTarget - transform.position).normalized;
            MovementVector.y = transform.position.y;
        }
    }

   //check if player is moving and if we have reached the target in case of non-controlled movement
    void GetMovementStatus()
    {
        if(PlayerControlled)
        {
            IsMoving = MovementDirection.magnitude > 0.1f;
        }
        else
        {
            float distanceLeft = Vector3.Distance(transform.position, MovementTarget);
            IsMoving = distanceLeft > TargetBias;
            HasReachedTarget = distanceLeft <= TargetBias;
        }
    }


    Vector2 GetMovementDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        return new Vector2(x, y).normalized;

    }
}
