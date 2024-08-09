using UnityEngine;
using static CW.Common.CwInputManager;

public class ExplodingObject: MonoBehaviour, IFirable
{
    [SerializeField] GameObject _trigger;
    
    public void Interact(RaycastHit hit)
    {
        _trigger.SetActive(true);
        gameObject.SetActive(false);
    }
}
