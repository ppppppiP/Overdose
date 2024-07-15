using UnityEngine;

public class WalkState : PlayerFSMState
{
    CharacterController _controller;
    float _speed;

    public WalkState(FSMPlayer _fsm, CharacterController controller, float speed) : base(_fsm)
    {
        _controller = controller;
        _speed = speed;
    }

    public override void Enter()
    {
        Debug.Log("[WALK STATE ENTER]");
    }

    public override void Exit()
    {
        Debug.Log("[WALK STATE EXIT]");
    }

    public override void Update()
    {
        if (_controller != null)
            Move();

        if (Input.GetAxisRaw(StaticNames.Input.Horizontal) == 0 && 
            Input.GetAxisRaw(StaticNames.Input.Vertical) == 0)
            fsm.SetState<IdleState>();

        if (Input.GetKey(KeyCode.LeftShift))
            fsm.SetState<RunState>();
    }

    private void Move()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxis(StaticNames.Input.Horizontal), 
                                             Input.GetAxis(StaticNames.Input.Vertical));
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * inputDirection.y + cameraRight * inputDirection.x;
        moveDirection.Normalize();

        if (moveDirection != Vector3.zero)
        {
            _controller.Move(moveDirection * _speed * Time.deltaTime);

            if (inputDirection != Vector2.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

                float rotationSpeed = 5 * Time.deltaTime;

                if (_controller.transform.forward != moveDirection)
                {
                    _controller.transform.rotation = 
                     Quaternion.RotateTowards(_controller.transform.rotation, targetRotation, rotationSpeed);
                }
            }
        }

        if (inputDirection != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            float rotationSpeed = 100 * Time.deltaTime;

            _controller.transform.rotation = Quaternion.Lerp(_controller.transform.rotation, 
                                             targetRotation, rotationSpeed);
        }
    }
}

public class RunState : WalkState
{
    public RunState(FSMPlayer _fsm, CharacterController controller, float speed) : base(_fsm, controller, speed)
    {
    }

    public override void Enter()
    {
        
    }
    public override void Exit()
    {

    }

    public override void Update()
    {
        base.Update();

        if (!Input.GetKey(KeyCode.LeftShift))
            fsm.SetState<WalkState>();
    }
}

