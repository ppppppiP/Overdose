using UnityEngine;

public class EnemyTarakanHP: MonoBehaviour, IDamagable
{
    [SerializeField] float m_helth;


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
        Debug.LogAssertion(nameof(EnemyTarakanHP));

    }
}