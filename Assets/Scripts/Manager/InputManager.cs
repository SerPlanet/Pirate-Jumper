using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public PlayerControler playerControler;

    private InputAction jump;

    public event Action OnJump;

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

        jump.performed += JumpPerformed;
    }

    public void JumpPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    private void OnDisable()
    {
        jump.Disable();
    }
}
