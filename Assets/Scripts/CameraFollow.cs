
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float distance = 5f;
    public float sensitivity = 0.15f;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    private PlayerControls controls;
    private float yaw;
    private float pitch = 15f;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
    }

    private void LateUpdate()
    {
        if (player == null)
            return;

        if (controls.Player.CameraRotate.IsPressed())
        {
            Vector2 mouseDelta =
                controls.Player.Look.ReadValue<Vector2>();

            yaw += mouseDelta.x * sensitivity;
            pitch -= mouseDelta.y * sensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Vector3 target = player.position + Vector3.up * 1.5f;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

        transform.position = target + offset;
        transform.LookAt(target);
    }

    private void OnDestroy()
    {
        controls.Dispose();
    }
}
