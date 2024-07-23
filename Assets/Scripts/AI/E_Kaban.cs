using BlackBoardSystem;
using Controller;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityServiceLocator;

public class E_Kaban : MonoBehaviour, IExpert
{
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    [SerializeField] GameObject playerTarget;
    [SerializeField] float minAttackDistance = 10f;
    [SerializeField] float detectionDistance = 20f;
    [SerializeField] Transform gunPosition;
    NavMeshAgent agent;
    BehaviourTree tree;
    Blackboard blackboard;
    BlackboardKey isPlayerDetectedKey;
    BlackboardKey isWithinAttackRangeKey;

    public LayerMask Layer;

    private int currentAmmo = 0;
    [SerializeField] int maxAmmo;
    private float spread = 0.5f;
    private bool isReloading;
    [SerializeField] float reloadTime;
    [SerializeField] GameObject Tracer;

    bool reloadFlag = true;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        blackboard = ServiceLocator.For(this).Get<BlackboardController>().GetBlackboard();
        ServiceLocator.For(this).Get<BlackboardController>().RegisterExpert(this);

        isPlayerDetectedKey = blackboard.GetOrRegisterKey("isPlayerDetected");
        isWithinAttackRangeKey = blackboard.GetOrRegisterKey("isWithinAttackRange");

        blackboard.SetValue(isPlayerDetectedKey, false);
        blackboard.SetValue(isWithinAttackRangeKey, false);

        SetupBehaviourTree();
    }

    void SetupBehaviourTree()
    {
        tree = new BehaviourTree("Enemy Behaviour Tree");

        PrioritySelector rootSelector = new PrioritySelector("Root");

        Sequence detectAndMoveSequence = new Sequence("Detect and Move Sequence", 100);
        detectAndMoveSequence.AddChild(new Leaf("Is player detected?", new Condition(() => blackboard.TryGetValue(isPlayerDetectedKey, out bool isDetected) && isDetected)));
        detectAndMoveSequence.AddChild(new Leaf("Move to player", new MoveToTarget(transform, agent, playerTarget.transform, minAttackDistance)));

        Sequence attackSequence = new Sequence("Attack Sequence", 110);
        attackSequence.AddChild(new Leaf("Is within attack range", new Condition(() => blackboard.TryGetValue(isWithinAttackRangeKey, out bool withinRange) && withinRange)));
        attackSequence.AddChild(new Leaf("Attack player", new ActionStrategy(() => AttackPlayer())));

        rootSelector.AddChild(detectAndMoveSequence);
        rootSelector.AddChild(attackSequence);

        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints), 10);
        rootSelector.AddChild(patrol);

        tree.AddChild(rootSelector);
    }

    void Update()
    {
        tree.Process();

       
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);

        if (Physics.Linecast(playerTarget.transform.position, transform.position, Layer))
            blackboard.SetValue(isPlayerDetectedKey, false);
        else 
            blackboard.SetValue(isPlayerDetectedKey, distanceToPlayer <= detectionDistance);
           

        if (Physics.Linecast(playerTarget.transform.position, transform.position, Layer))
            blackboard.SetValue(isWithinAttackRangeKey, false);
        else 
            blackboard.SetValue(isWithinAttackRangeKey, distanceToPlayer <= minAttackDistance);
            


    }

    public int GetInstance(Blackboard blackboard)
    {
        return 1; // Всегда активен
    }

    public void Execute(Blackboard blackboard)
    {
        // Логика выполняется в Update
    }

    void AttackPlayer()
    {
   transform.DOLookAt(playerTarget.transform.position, 0.2f, AxisConstraint.Y);
            if (currentAmmo < maxAmmo&& !isReloading)
            {
                currentAmmo++;
               
                Vector3 spreadVector = Random.insideUnitSphere * spread;
             

                // Исправлено направление для Raycast
                Vector3 direction = (playerTarget.transform.position - gunPosition.position).normalized + spreadVector;
                if (Physics.Raycast(gunPosition.position, direction, out RaycastHit hit))
                {
                    UnityEngine.Debug.LogAssertion("SHOOOOOOOOOOOOOOOOOOOOOOT");
                if (hit.transform.TryGetComponent<PlayerController>(out PlayerController damage))
                {
                    CameraShake.Instance.ShakeCamera();
                }
               ;
                    // логика попадания по врагам будет тут
                    StartCoroutine(TracerRenderer(gunPosition.position, hit.point));
                }
            }
            else if(reloadFlag)
            {
            reloadFlag = false;
               Reload();
            }
        
    }
    public IEnumerator TracerRenderer(Vector3 start, Vector3 target)
    {
        float duration = 0.1f; // Длительность полета трейсера
        float elapsedTime = 0f;

        GameObject tracer = Instantiate(Tracer, gunPosition.position, Quaternion.LookRotation(target - start));

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Нормализованное время

            // Используем Lerp для плавного перемещения
            tracer.transform.position = Vector3.Lerp(start, target, t);

            yield return null; // Ждем следующего кадра
        }

        // Убедимся, что трейсер достиг конечной точки
        tracer.transform.position = target;

        // Опционально: уничтожаем трейсер после небольшой задержки
        yield return new WaitForSeconds(1f);
        Destroy(tracer);
    }
    private async void  Reload()
    {
        isReloading = true;
        

        await Task.Delay((int)reloadTime* 1000);
        reloadFlag = true;
        currentAmmo = 0;
        isReloading = false;
    }
}