using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamagable
{
    public float HP;
    public System.Action OnDie;

    public void GetDamage(float damage)
    {
        HP -= damage;
        if(HP < 0)
        {
            gameObject.SetActive(false);
        }
        OnDie?.Invoke();
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }


}

public interface IDamagable
{
   public void GetDamage(float damage);
}