using UnityEngine;
using UnityServiceLocator;

namespace BlackBoardSystem
{
    public class Scout : MonoBehaviour, IExpert
    {
        Blackboard blackboard;
        BlackboardKey isSafeKey;

        bool dangerSensor;

        void Start()
        {
            blackboard = ServiceLocator.For(this).Get<BlackboardController>().GetBlackboard();
            ServiceLocator.For(this).Get<BlackboardController>().RegisterExpert(this);
            isSafeKey = blackboard.GetOrRegisterKey("IsSafe");
        }

        public int GetInstance(Blackboard blackboard)
        {
            return dangerSensor ? 100 : 0;
        }

        public void Execute(Blackboard blackboard)
        {
            blackboard.AddAction(() => {
                if (blackboard.TryGetValue(isSafeKey, out bool isSafe))
                {
                    blackboard.SetValue(isSafeKey, !isSafe);
                    Debug.Log(isSafe);
                }
                dangerSensor = false;
            });
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dangerSensor = !dangerSensor;
            }
        }
    }
}
