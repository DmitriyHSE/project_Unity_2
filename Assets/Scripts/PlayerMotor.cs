using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 8.0f;
    public bool isGrounded;
    public bool lerpCrouch;
    public bool crouching;
    public bool sprinting;
    public float crouchTimer = 0.0f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.0f;

    void Update()
    {
        isGrounded = controller.isGrounded;


        if (Input.GetKey(KeyCode.LeftShift))
            StartSprint();
        else
            StopSprint();

        if (Input.GetKey(KeyCode.LeftControl))
            StartCrouch();
        else
            StopCrouch();


        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void StartCrouch()
    {
        if (!crouching)
        {
            crouching = true;
            crouchTimer = 0;
            lerpCrouch = true;
        }
    }

    public void StopCrouch()
    {
        if (crouching)
        {
            crouching = false;
            crouchTimer = 0;
            lerpCrouch = true;
        }
    }

    public void StartSprint()
    {
        if (!sprinting)
        {
            sprinting = true;
            speed = 15;
        }
    }

    public void StopSprint()
    {
        if (sprinting)
        {
            sprinting = false;
            speed = 8;
        }
    }
}
