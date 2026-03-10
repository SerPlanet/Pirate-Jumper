using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public PlayerControler playerControler;

    private InputAction jump;

    public event Action OnJumpPressed;
    public event Action OnJumpReleased;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerControler = new PlayerControler();
    }

    private void OnEnable()
    {
        jump = playerControler.Player.Jump;
        jump.Enable();

        jump.started += JumpPressed;
        jump.canceled += JumpReleased;
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        OnJumpPressed?.Invoke();
    }

    public void JumpReleased(InputAction.CallbackContext context)
    {
        OnJumpReleased?.Invoke();
    }


    private void OnDisable()
    {
       
        jump.started -= JumpPressed;
        jump.canceled -= JumpReleased;
        jump.Disable();
    }
}
