using System.Runtime.CompilerServices;
using UnityEngine;
using UnityPipeline.Microsoft.CodeAnalysis.FlowAnalysis;

public class PlayerMotor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CharacterController controller;
    private Vector3 playerVelocity;
    public float playerSpeed = 5f;
    private bool isGrounded;

    public float gravity = -9.8f;

    public float jumpheight = 3f;
    private bool crouching = false;
    private bool sprinting = false;
    private bool lerpCrouch = false;
    private float crouchTimer = 0f;
    private float timeToCrouch = 0.25f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / timeToCrouch;
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

    //receive the inputs from the InputManager.cs and apply them to our character controller.
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime * playerSpeed);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpheight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;

    }


    public void Sprint()
    {
        sprinting = !sprinting;

        
        if (sprinting)
        {
            playerSpeed = 8;

        }

        else
        {
            playerSpeed = 5;
        }
    }
}
