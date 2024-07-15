using UnityEngine;

public class PlayerAttack: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable enemy))
        {
            enemy.GetDamage(1);

            if(enemy is EnemyTarakanHP)
            {
                enemy = new EnemySpiderHP();
            }
           
        }


    }
}

