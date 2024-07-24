using System.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour, IDamagable
{
    [SerializeField] float HP;

    public static event System.Action OnPlayerDie;

    
    public void GetDamage(float damage)
    {
        HP -= damage;

        if(HP< 0)
        {
            OnPlayerDie?.Invoke();
        }
            
    }
}
