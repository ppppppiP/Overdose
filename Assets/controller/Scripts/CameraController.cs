using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    private float rotationX = 0;
    public float lookVertical;
    public float lookHorizontal;
    [SerializeField, Range(0.5f, 10)] private float lookSpeed = 2.0f;
    [SerializeField, Range(10, 120)] private float lookXLimit = 80.0f;
    [SerializeField] private float runningFOV = 65.0f;
    [SerializeField] private float speedToFOV = 4.0f;
    [SerializeField] Controller.PlayerController pla;
    private float installFOV;
    private Camera cam;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        lookSpeed = PlayerPrefs.GetFloat("Sensitivity");
    }
    private void Update()
    {


        if (Cursor.lockState == CursorLockMode.Locked)
        {
            lookVertical = -Input.GetAxis("Mouse Y");
            lookHorizontal = Input.GetAxis("Mouse X");

            rotationX += lookVertical * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, lookHorizontal * lookSpeed, 0);

           if (pla.isRunning && pla.moving)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runningFOV, speedToFOV * Time.deltaTime);
            else
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, installFOV, speedToFOV * Time.deltaTime);
        }
    }
}