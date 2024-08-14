using Controller;
using UnityEngine;

public class PlayerSwimming : MonoBehaviour
{
    public float swimSpeed = 5f;           // Скорость плавания
    public bool CanSwim = false;           // Флаг возможности плавания

    private CharacterController characterController;
    private PlayerController playerController;
    private Transform cameraTransform;

    void Start()
    {
        playerController= GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (CanSwim)
        {
            HandleSwimming();
            playerController.enabled = false;
        }
        else
        {
            playerController.enabled= true;
        }
    }

    public void SetWaterParameters(bool canSwim)
    {
        CanSwim = canSwim;
    }

    private void HandleSwimming()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        float vertical = 0;
        float horizontal = 0;

        if (Input.GetKey(KeyCode.W))
        {
            vertical += swimSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vertical -= swimSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            horizontal -= swimSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontal += swimSpeed;
        }

        Vector3 direction = (forward * vertical) + (right * horizontal);
        direction.y = forward.y * vertical; // Чтобы двигаться вверх/вниз вместе с камерой

        characterController.Move(direction * Time.deltaTime);
    }
}