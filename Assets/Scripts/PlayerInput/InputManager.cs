using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private Gun gun;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        gun = GetComponentInChildren<Gun>();

        onFoot.Crounch.performed += ctx => motor.StartCrouch();
        onFoot.Sprint.performed += ctx => motor.StartSprint();
        onFoot.Shoot.performed += ctx => gun?.Shoot();
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
