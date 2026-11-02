using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.5f, -2f);

    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Player.Look.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Look.Disable();
    }
    private void LateUpdate()
    {
        transform.position = cameraTarget.transform.position + offset;

    }
}
