using System.Collections;
using UnityEngine;

public class MolotovTrigger : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] float _timer;

    private void Start()
    {
        Destroy(gameObject, _timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            StartCoroutine(MolotovRoutine(damagable));
        }

    }private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            StopCoroutine(MolotovRoutine(damagable));
        }

    }


    IEnumerator MolotovRoutine(IDamagable damagable)
    {
        while(gameObject.activeSelf == true)
        {
            yield return new WaitForSeconds(0.25f);
            damagable.GetDamage(_damage);
        }
    }


}