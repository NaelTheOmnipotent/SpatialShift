using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerScript : MonoBehaviour
{
    //References
    PlayerInput playerInput;
    Rigidbody2D rb;

    //Variables
    [HideInInspector] public bool jumped;
    [HideInInspector] public bool jumpBuffer;
    [HideInInspector] public bool jumpHeld;

    #region First Frames
    private void Awake()
    {
        //Adds the PlayerInput
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        //Enables it
        playerInput.Enable();

        //Subscribes to the Jump Events
        playerInput.BasicMovement.Jump.started += JumpStarted;
        playerInput.BasicMovement.Jump.performed += JumpHeld;
        playerInput.BasicMovement.Jump.canceled += JumpCancled;
    }
    private void OnDisable()
    {
        //Disables it
        playerInput.Disable();
    }

    private void Start()
    {
        //Reference to RigidBody
        rb = GetComponent<Rigidbody2D>();
    }


    #endregion

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            jumped = false;
        }
    }

    //Returns the Movement Input
    public Vector2 MovementInput()
    {
        return playerInput.BasicMovement.Movement.ReadValue<Vector2>();
    }

    //Returns the JumpInput
    public bool JumpInput()
    {
        if (jumped) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    //Checks if the Jump button is pressed
    private void JumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        jumpBuffer = true;
        jumped = true;
    }
    
    private void JumpHeld(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        jumpHeld = true;
    }

    //Checks if the Jump button is released
    private void JumpCancled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        jumped = false;
        jumpBuffer = false;
        jumpHeld = false;
    }

    //Returns true if the ShiftingButton is held down
    public bool isHoldingTeleport()
    {
        var isHolding = playerInput.BasicMovement.Shifting.ReadValue<float>();
        if (isHolding > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Returns the ShiftingDirection
    public Vector2 ShiftingVector()
    {
        return playerInput.BasicMovement.ShiftingVector.ReadValue<Vector2>();
    }

    //Returns true if the PauseMenu Button is Triggered
    public bool OnPauseMenu()
    {
        return playerInput.BasicMovement.PauseMenu.triggered;
    }
}
