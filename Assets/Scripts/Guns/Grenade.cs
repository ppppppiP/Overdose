using System.Collections;
using UnityEngine;

public class Grenade: MonoBehaviour
{
    [SerializeField] float _strength;
    [SerializeField] float _timer;
    
    [SerializeField] GameObject _trigger;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if(rb!= null)
        {
            Vector3 targetDirection = Camera.main.transform.forward;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = targetRotation;

            rb.AddForce(transform.forward * _strength);
            StartCoroutine(ExploudeRoutine());
        }
    }

    

    IEnumerator ExploudeRoutine()
    {
        yield return new WaitForSeconds(_timer);

        _trigger.SetActive(true);
        gameObject.SetActive(false);
    }


}
