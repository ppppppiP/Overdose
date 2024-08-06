using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aptechka : MonoBehaviour
{
    public float HealNumber;
    public GameObject Sound;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHP>(out PlayerHP pla))
        {
            Instantiate(Sound, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        
            pla.Heal(HealNumber);
        }
    }
}
