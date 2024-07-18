using BlackBoardSystem;
using UnityEngine;
using UnityServiceLocator;

public class E_KabanDetection: MonoBehaviour, IExpert
{
    [SerializeField] LayerMask m_wallLayer;

    Blackboard blackboard;
    BlackboardKey isPlayerDetected;
    Transform player;

    bool flag = true;

    public bool detectedSensor;
    bool isEnter;
    void Start()
    {
        blackboard = ServiceLocator.For(this).Get<BlackboardController>().GetBlackboard();
        ServiceLocator.For(this).Get<BlackboardController>().RegisterExpert(this);
        isPlayerDetected = blackboard.GetOrRegisterKey("isPlayerDetected");
    }

    public int GetInstance(Blackboard blackboard)
    {
        return detectedSensor ? 100 : 0;
    }

    public void Execute(Blackboard blackboard)
    {
        blackboard.AddAction(() => {
            if (blackboard.TryGetValue(isPlayerDetected, out bool isDetected))
            {
                blackboard.SetValue(isPlayerDetected, !isDetected);
                Debug.Log(isDetected);
            }
            detectedSensor = false;
        });
    }

    void Update()
    {
        if (isEnter)
        {
            if (Physics.Linecast(transform.position, player.position, ~m_wallLayer))
            {

                if (flag)
                {
                    flag = false;
                    detectedSensor = !detectedSensor;
                }
            }
            else
            {
                if (!flag)
                {
                   // flag = true;
                   // detectedSensor = !detectedSensor;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController pla))
        {
            player = pla.gameObject.transform;
            isEnter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController pla))
        {
            isEnter = false;
        }
    }
}