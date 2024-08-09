using System.Collections;
using UnityEngine;

public class GrenadeTrigger: MonoBehaviour
{
    [SerializeField] float _damage;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.GetDamage(_damage);
        }
        
    }

}