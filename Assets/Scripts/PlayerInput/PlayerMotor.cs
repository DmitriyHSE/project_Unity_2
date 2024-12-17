using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private FPSController fpscontroller;
    public bool lerpCrouch;
    public bool crouching;
    public bool sprinting;
    public float crouchTimer = 0.0f;

    void Update()
    {
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
            fpscontroller.speed = fpscontroller.speed * 2;
        }
    }

    public void StopSprint()
    {
        if (sprinting)
        {
            sprinting = false;
            fpscontroller.speed = fpscontroller.speed / 2;
        }
    }
}
