using BlackBoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityServiceLocator;

public class E_Kaban : MonoBehaviour, IExpert
{
    [SerializeField] List<Transform> waypoints = new();
    [SerializeField] GameObject _tracer;
   
    [SerializeField] GameObject _playerTarget;
    
    NavMeshAgent agent;
    BehaviourTree tree;
    Blackboard blackboard;
    BlackboardKey isPlayerDetected;
    BlackboardKey isNormalShootDistance;
   

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        blackboard = ServiceLocator.For(this).Get<BlackboardController>().GetBlackboard();
        ServiceLocator.For(this).Get<BlackboardController>().RegisterExpert(this);


        isPlayerDetected = blackboard.GetOrRegisterKey("isPlayerDetected");
        isNormalShootDistance = blackboard.GetOrRegisterKey("isNotNormalShootDistance");
       
        blackboard.SetValue(isPlayerDetected, false);

        SetupBehaviourTree();
    }

    void SetupBehaviourTree()
    {
        tree = new BehaviourTree("Kaban enemy");

        PrioritySelector rootSelector = new PrioritySelector("Root");

      

        Sequence goToAttack = new Sequence(" go to Attack seq", 60);
        goToAttack.AddChild(new Leaf("is player detected", new Condition(() => blackboard.TryGetValue(isPlayerDetected, out bool isDetected) || !isDetected)));
        goToAttack.AddChild(new Leaf("isNotNormalShootDistance", new Condition(() => !blackboard.TryGetValue(isNormalShootDistance, out bool isNotNormal) || !isNotNormal)));
        goToAttack.AddChild(new Leaf("Go to finish work", new MoveToTarget(transform, agent, _playerTarget.transform)));
        rootSelector.AddChild(goToAttack);

        Sequence Attack = new Sequence("Attack seq", 100);
        goToAttack.AddChild(new Leaf("isNotNormalShootDistance", new Condition(() => !blackboard.TryGetValue(isNormalShootDistance, out bool isNotNormal) || !isNotNormal)));
        goToAttack.AddChild(new Leaf("Go to finish work", new MoveToTarget(transform, agent, _playerTarget.transform)));

        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints), 10);
        //rootSelector.AddChild(patrol);

        tree.AddChild(rootSelector);
    }

    void Update()
    {
        tree.Process();
    }

    public int GetInstance(Blackboard blackboard)
    {
        return 1; // Всегда активен
    }

    public void Execute(Blackboard blackboard)
    {
        // Ничего не делаем здесь, так как логика выполняется в Update
    }
}