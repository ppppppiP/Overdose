using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllers : MonoBehaviour
{
    [SerializeField] float m_WalkSpeed;
    [SerializeField] float m_RunSpeed;
    [SerializeField] float m_GravityStrengh;
    [SerializeField] float m_JumpHeight;
    [SerializeField] float m_SweemSpeed;

    Vector3 _velocity;
    FSMPlayer _fsm;
    [SerializeField] CharacterController _controller;

    bool isGrounded;

    private void OnValidate()
    {
        _controller = GetComponent<CharacterController>();
        
    }

    private void Awake()
    {   
        

        _fsm = new FSMPlayer();
        _fsm.AddState(new IdleState(_fsm));
        _fsm.AddState(new WalkState(_fsm, _controller, m_WalkSpeed));
        _fsm.AddState(new RunState(_fsm, _controller, m_RunSpeed));
        _fsm.SetState<IdleState>();
        
    }

    private void FixedUpdate()
    {
         isGrounded = _controller.isGrounded;
    }

    void Update()
    {
        _fsm.Update();

        if(isGrounded)
            Jump();

        Gravity(m_GravityStrengh);

        _controller.Move(_velocity);

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _velocity.y = Mathf.Sqrt(m_JumpHeight * -1 * m_GravityStrengh);
        }
    }

    void Gravity(float gravityStrangth)
    {
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        _velocity.y += gravityStrangth * Time.deltaTime;
        
    }
}

