using UnityEngine;

public class EnemySpiderHP: MonoBehaviour, IDamagable
{
    [SerializeField] float m_helth;
    int armor = 1;
    
    public void GetDamage(int damage)
    {
        
        if (damage > m_helth)
        {
            m_helth = 0;
        }

        if (damage < 0)
        {
            return;
        }

        m_helth -= damage;
        Debug.LogAssertion(m_helth);
        Debug.LogAssertion(nameof(EnemySpiderHP));

    }
}
