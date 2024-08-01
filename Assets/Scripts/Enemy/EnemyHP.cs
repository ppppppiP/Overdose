using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamagable
{
    public float HP;
    public event System.Action OnDie;

    public void GetDamage(float damage)
    {
        HP -= damage;
        if(HP < 0)
        {
             OnDie?.Invoke();
        }
       
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