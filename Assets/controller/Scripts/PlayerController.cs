using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerController")]
        [SerializeField] private Transform Camera;
        [SerializeField] private ItemChange Items;
        [SerializeField, Range(1, 10)] private float walkingSpeed = 3.0f;
        [SerializeField, Range(1, 10)] private float LadderSpeed = 3.0f;
        [SerializeField, Range(2, 20)] private float runningSpeed = 4.0f;
        [Range(0.1f, 5)] public float crouchSpeed = 1.0f;
        [SerializeField, Range(0, 20)] private float jumpSpeed = 6.0f;
        [SerializeField, Range(0.5f, 10)] private float lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)] private float lookXLimit = 80.0f;
        [Space(20)]
        [Header("Advance")]
        [SerializeField] private float runningFOV = 65.0f;
        [SerializeField] private float ladderSpeed = 3f;
        [SerializeField] private float speedToFOV = 4.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float timeToRunning = 2.0f;
        [SerializeField] private float crouchHeight = 1.0f;
        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool canRunning = true;
        [Space(20)]
        [Header("HandsHide")]
        [SerializeField] private bool canHideDistanceWall = true;
        [SerializeField, Range(0.1f, 5)] private float hideDistance = 1.5f;
        [SerializeField] private int layerMaskInt = 1;

        [Space(20)]
        [Header("Input")]
        [SerializeField] private KeyCode crouchKey;
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private KeyCode runKey;
        [SerializeField] private KeyCode moveForwardKey;
        [SerializeField] private KeyCode moveBackwardKey;
        [SerializeField] private KeyCode moveLeftKey;
        [SerializeField] private KeyCode moveRightKey;

        public CharacterController characterController;
        private Vector3 moveDirection = Vector3.zero;
        private bool isCrouching = false;
        private bool isJumping = true;
        private float installCrouchHeight;
        private float rotationX = 0;
        private bool isRunning = false;
        private Vector3 installCameraMovement;
        private float installFOV;
        private Camera cam;
        private bool moving;
        public float vertical;
        public float horizontal;
        public float lookVertical;
        public float lookHorizontal;
        public float runningValue;
        public float walkingValue;
        public bool wallDistance;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            if (Items == null && GetComponent<ItemChange>()) Items = GetComponent<ItemChange>();
            cam = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            installCrouchHeight = characterController.height;
            installCameraMovement = Camera.localPosition;
            installFOV = cam.fieldOfView;
            runningValue = runningSpeed;
            walkingValue = walkingSpeed;

            // Загрузка настроек управления из PlayerPrefs
            crouchKey = (KeyCode)PlayerPrefs.GetInt("CrouchKey", (int)KeyCode.LeftControl);
            jumpKey = (KeyCode)PlayerPrefs.GetInt("JumpKey", (int)KeyCode.Space);
            runKey = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);
            moveForwardKey = (KeyCode)PlayerPrefs.GetInt("MoveForwardKey", (int)KeyCode.W);
            moveBackwardKey = (KeyCode)PlayerPrefs.GetInt("MoveBackwardKey", (int)KeyCode.S);
            moveLeftKey = (KeyCode)PlayerPrefs.GetInt("MoveLeftKey", (int)KeyCode.A);
            moveRightKey = (KeyCode)PlayerPrefs.GetInt("MoveRightKey", (int)KeyCode.D);
            lookSpeed = PlayerPrefs.GetFloat("Sensitivity");
        }

        void Update()
        {
            RaycastHit crouchCheck;
            RaycastHit objectCheck;

            // Проверка на прыжок
            if (Input.GetKey(jumpKey) && canMove && characterController.isGrounded && !isCrouching)
            {
                isJumping = true;
                moveDirection.y = jumpSpeed;
            }

            // Применение гравитации
            if (!characterController.isGrounded && canMove)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Управление движением вперед и назад
            bool moveForward = Input.GetKey(moveForwardKey);
            bool moveBackward = Input.GetKey(moveBackwardKey);
            bool moveLeft = Input.GetKey(moveLeftKey);
            bool moveRight = Input.GetKey(moveRightKey);

            isRunning = !isCrouching ? canRunning && Input.GetKey(runKey) : false;
            vertical = (isRunning ? runningValue : walkingValue) * ((moveForward ? 1 : 0) - (moveBackward ? 1 : 0));
            horizontal = (isRunning ? runningValue : walkingValue) * ((moveRight ? 1 : 0) - (moveLeft ? 1 : 0));

            if (isRunning) runningValue = Mathf.Lerp(runningValue, runningSpeed, timeToRunning * Time.deltaTime);
            else runningValue = walkingValue;

            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * vertical) + (right * horizontal);

            // Обработка движения
            if (Input.GetKey(jumpKey) && canMove && characterController.isGrounded)
            {
                if (!isCrouching)
                {
                    moveDirection.y = jumpSpeed;
                }
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            characterController.Move(moveDirection * Time.deltaTime);
            moving = moveForward || moveLeft || moveRight;

            // Управление взглядом и камерой
            if (Cursor.lockState == CursorLockMode.Locked && canMove)
            {
                lookVertical = -Input.GetAxis("Mouse Y");
                lookHorizontal = Input.GetAxis("Mouse X");

                rotationX += lookVertical * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, lookHorizontal * lookSpeed, 0);

                if (isRunning && moving)
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runningFOV, speedToFOV * Time.deltaTime);
                else
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, installFOV, speedToFOV * Time.deltaTime);
            }

            // Управление приседанием
            if (Input.GetKey(crouchKey))
            {
                isCrouching = true;
                float height = Mathf.Lerp(characterController.height, crouchHeight, 5 * Time.deltaTime);
                characterController.height = height;
                walkingValue = Mathf.Lerp(walkingValue, crouchSpeed, 6 * Time.deltaTime);
            }
            else if (!Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.up), out crouchCheck, 0.8f, 1))
            {
                if (characterController.height != installCrouchHeight)
                {
                    isCrouching = false;
                    float height = Mathf.Lerp(characterController.height, installCrouchHeight, 6 * Time.deltaTime);
                    characterController.height = height;
                    walkingValue = Mathf.Lerp(walkingValue, walkingSpeed, 4 * Time.deltaTime);
                }
            }

            // Проверка на дистанцию до стены
            if (wallDistance != Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out objectCheck, hideDistance, layerMaskInt) && canHideDistanceWall)
            {
                wallDistance = Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out objectCheck, hideDistance, layerMaskInt);
                Items.ani.SetBool("Hide", wallDistance);
                Items.DefiniteHide = wallDistance;
            }

            // Управление лестницей
            if (!canMove)
            {
                if (Input.GetAxis("Vertical") > 0)
                    characterController.Move(Vector3.up * LadderSpeed);
                if (Input.GetAxis("Vertical") < 0)
                {
                    characterController.Move(Vector3.down * LadderSpeed);
                    if (Physics.Raycast(transform.position, Vector3.down, 10))
                    {
                        canMove = true;
                    }
                }

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Ladder>())
            {
                canMove = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Ladder>())
            {
                canMove = true;
            }
        }
    }
}
