using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;
    private InputAction fly;

    public static bool isSpaceBarPressed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        fly = playerInput.actions["Fly"];
    }

    private void Update()
    {
        isSpaceBarPressed = fly.IsPressed();
    }
}
