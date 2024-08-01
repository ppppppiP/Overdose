using BlackBoardSystem;

using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

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
    public float spread = 0.5f;
    private bool isReloading;
    public float Damage;
    [SerializeField] float reloadTime;
    [SerializeField] GameObject Tracer;
    [SerializeField] GameObject SpawnVFX;
    public float arcRadius = 5f; // Радиус дуги
    public float arcAngle = 45f; // Угол дуги

    [SerializeField] Animator _anim;

    int EnemyID;

    bool reloadFlag = true;
    private Vector3 targetPoint;
   public float patrolRadius;

    void Awake()
    {
        EnemyID = GetInstanceID();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        blackboard = ServiceLocator.For(this).Get<BlackboardController>().GetBlackboard();
        ServiceLocator.For(this).Get<BlackboardController>().RegisterExpert(this);

        isPlayerDetectedKey = blackboard.GetOrRegisterKey("isPlayerDetected" + EnemyID);
        isWithinAttackRangeKey = blackboard.GetOrRegisterKey("isWithinAttackRange" + EnemyID);

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
        detectAndMoveSequence.AddChild(new Leaf("Set no locomotion anim", new ActionStrategy(() => _anim.SetBool("GoForward", true))));

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
               
                Vector3 spreadVector = UnityEngine.Random.insideUnitSphere * spread;
             

                
                Vector3 direction = (playerTarget.transform.position - gunPosition.position).normalized + spreadVector;
                if (Physics.Raycast(gunPosition.position, direction, out RaycastHit hit))
                {
                Instantiate(SpawnVFX, gunPosition.transform.position, gunPosition.transform.rotation);
                  
                
                if (hit.transform.TryGetComponent<PlayerHP>(out PlayerHP damage))
                {
                    damage.GetDamage(Damage);
                    CameraShake.Instance.ShakeCamera();
                }
               
                    
                    StartCoroutine(TracerRenderer(gunPosition.position, hit.point));
                }
            }
            else if(reloadFlag)
            {
            reloadFlag = false;
               Reload();
            }
        _anim.SetBool("GoForward", false);
        _anim.SetFloat("XDir", agent.velocity.normalized.x);
        _anim.SetFloat("YDir", agent.velocity.normalized.z);
        Patrol();
    }
    private void Patrol()
    {
      
        if (!isStopped)
        {
            StartCoroutine(StopRoutine());

            Vector3 start = transform.position;
            Vector3 end = GetRandomPointInArc();
            float radius = 5f; 
            float angle = 45f;
            float duration = 5f;

            StartCoroutine(MoveAlongArc(start, end, radius, angle, duration));
        }
    }
    private IEnumerator MoveAlongArc(Vector3 start, Vector3 end, float radius, float angle, float duration)
    {
        List<Vector3> arcPoints = GenerateRandomArcPoints(start, radius, angle, 10);
        arcPoints.Add(end);

        float timeElapsed = 0f;
        Vector3 previousPoint = start;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            
            int segment = Mathf.FloorToInt(t * (arcPoints.Count - 1));
            float segmentT = (t * (arcPoints.Count - 1)) - segment;

            Vector3 currentPoint = Vector3.Lerp(arcPoints[segment], arcPoints[segment + 1], segmentT);

            
            agent.SetDestination(currentPoint);

            
            UnityEngine.Debug.Log($"Agent Position: {agent.transform.position}, Target Point: {currentPoint}");

            yield return null;
        }

       
        agent.SetDestination(end);
    }
    private List<Vector3> GenerateRandomArcPoints(Vector3 center, float radius, float angle, int numPoints)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < numPoints; i++)
        {
            float t = (float)i / (numPoints - 1);
            float currentAngle = Mathf.Lerp(-angle / 2f, angle / 2f, t);

         
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 point = center + Quaternion.Euler(0, currentAngle, 0) * new Vector3(randomDirection.x, 0, randomDirection.y);

            points.Add(point);
        }

        return points;
    }
    bool isStopped;

    IEnumerator StopRoutine()
    {
        isStopped = true;
        yield return new WaitForSeconds(5f);
        isStopped = false;
    }

    
    private Vector3 GetRandomPointInArc()
    {
        
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * arcRadius;
        Vector3 randomPoint = new Vector3(randomDirection.x, 0, randomDirection.y);

        
        float angle = UnityEngine.Random.Range(-arcAngle / 2f, arcAngle / 2f);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        randomPoint = rotation * randomPoint;

        
        randomPoint += (Vector3)UnityEngine.Random.insideUnitCircle * patrolRadius;
        randomPoint.y = transform.position.y; 

        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }
    public IEnumerator TracerRenderer(Vector3 start, Vector3 target)
    {
        float duration = 0.1f; 
        float elapsedTime = 0f;

        GameObject tracer = Instantiate(Tracer, gunPosition.position, Quaternion.LookRotation(target - start));

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; 

           
            tracer.transform.position = Vector3.Lerp(start, target, t);

            yield return null; 
        }

       
        tracer.transform.position = target;

        
        yield return new WaitForSeconds(1f);
        Destroy(tracer);
    }
    private async void  Reload()
    {
        isReloading = true;
        

        await Task.Delay(((int)(reloadTime* 1000)));
        reloadFlag = true;
        currentAmmo = 0;
        isReloading = false;
    }
}