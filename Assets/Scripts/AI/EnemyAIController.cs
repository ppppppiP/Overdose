
using System.Collections.Generic;
using BlackBoardSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityServiceLocator;
using System.Linq;
//public class EnemyAIController : MonoBehaviour, IExpert
//{
//    [SerializeField] List<Transform> waypoints = new();
//    [SerializeField] GameObject safeSpot;
//    [SerializeField] SpawnerFarm _workPoints;
//    [SerializeField] WorckerInventory inventory;
//    [SerializeField] GameObject _finishWorkTarget;
//    int[] a = new int[10] {10, 2439, 1, 1, 1,1 ,1, 1, 1, 1};
//    NavMeshAgent agent;
//    BehaviourTree tree;
//    Blackboard blackboard;
//    BlackboardKey isSafeKey;
//    BlackboardKey isFullInventory;

//    void Awake()
//    {
//        agent = GetComponent<NavMeshAgent>();
//    }

//    void Start()
//    {
//        blackboard = ServiceLocator.For(this).Get<BlackboardController>().GetBlackboard();
//        ServiceLocator.For(this).Get<BlackboardController>().RegisterExpert(this);

    
//        isSafeKey = blackboard.GetOrRegisterKey("IsSafe");
//        isFullInventory = blackboard.GetOrRegisterKey("IsFullInventory");
//        blackboard.SetValue(isSafeKey, true);

//        SetupBehaviourTree();
//    }

//    void SetupBehaviourTree()
//    {
//        tree = new BehaviourTree("Farmer");

//        PrioritySelector rootSelector = new PrioritySelector("Root");

//        Sequence runToSafetySeq = new Sequence("RunToSafety", 100);
//        runToSafetySeq.AddChild(new Leaf("IsDangerous", new Condition(() => !blackboard.TryGetValue(isSafeKey, out bool isSafe) || !isSafe)));
//        //runToSafetySeq.AddChild(new Leaf("GoToSafety", new MoveToTarget(transform, agent, safeSpot.transform)));
//        runToSafetySeq.AddChild(new Leaf("fdgdf", new ActionStrategy(() => agent.SetDestination(safeSpot.transform.position))));
//        rootSelector.AddChild(runToSafetySeq);

//        Sequence goToFinishWork = new Sequence("Finish Work", 60);
//        goToFinishWork.AddChild(new Leaf("is inventory fool?", new Condition(() => !blackboard.TryGetValue(isFullInventory, out bool isFool) || !isFool)));
//        goToFinishWork.AddChild(new Leaf("Go to finish work", new MoveToTarget(transform, agent, _finishWorkTarget.transform)));
//        goToFinishWork.AddChild(new Leaf("On work finished", new ActionStrategy(() => inventory.ClearInventory())));

//        rootSelector.AddChild(goToFinishWork);

//        Leaf work = new Leaf("Work", new WorkStrategy(transform, agent, _workPoints.workTransforms, inventory, 3f), 10);
//        rootSelector.AddChild(work);

//        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints), 10);
//        //rootSelector.AddChild(patrol);

//        tree.AddChild(rootSelector);
//    }

//    void Update()
//    {
//        tree.Process();
//    }

//    public int GetInstance(Blackboard blackboard)
//    {
//        return 1; // Всегда активен
//    }

//    public void Execute(Blackboard blackboard)
//    {
//        // Ничего не делаем здесь, так как логика выполняется в Update
//    }
//}

public static class ListExtensions
{
    static System.Random Rand;

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        if (Rand == null) Rand = new();

        int count = list.Count;
        while(count > 1)
        {
            --count;
            int index = Rand.Next(count + 1);
            (list[index], list[count]) = (list[count], list[index]);
        }
        return list;
    }
}