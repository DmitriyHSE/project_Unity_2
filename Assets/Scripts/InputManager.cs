using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    private Gun gun;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        gun = GetComponentInChildren<Gun>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crounch.performed += ctx => motor.StartCrouch();
        onFoot.Sprint.performed += ctx => motor.StartSprint();
        onFoot.Shoot.performed += ctx => gun?.Shoot();
    }

    void Update()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Looking.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisnable()
    {
        onFoot.Disable();
    } 
}
