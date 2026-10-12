
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private float verticalVelocity;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float sprintSpeed = 8f;
    private Vector3 movement;

    public float speed = 5f;
    public Transform cameraTransform;
    public float rotationSpeed = 10f;



    private void Awake()
    {
        controls = new PlayerControls();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void Jump()
    {
        if (controls.Player.Jump.WasPressedThisFrame()
            && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
    }
    private void CameraMovement()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        movement = forward * input.y + right * input.x;
    }
    private void CameraRotationDirection()
    {
        Vector3 rotationDirection = new Vector3(movement.x, 0f, movement.z);

        if (rotationDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
    private void Fall()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;
        controller.Move(movement * Time.deltaTime);
    }

    private void Update()
    {
        CameraMovement();

        float currentSpeed = speed;

        if (controls.Player.Sprint.IsPressed())
        {
            currentSpeed = sprintSpeed;
        }

        movement *= currentSpeed;

        Fall();

        Jump();

        CameraRotationDirection();




    }
}

