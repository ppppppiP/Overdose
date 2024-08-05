using Controller;
using UnityEngine;

public class ShpizGetter: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Finish.instance.Shpriz++;
            gameObject.SetActive(false);
        }
    }
}
