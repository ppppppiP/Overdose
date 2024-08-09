using UnityEngine;

public class WoodenObject: MonoBehaviour, IFirable
{
    public GameObject _trigger;
    public void Interact(RaycastHit hit)
    {
        _trigger.SetActive(true);
        gameObject.SetActive(false);
    }
}