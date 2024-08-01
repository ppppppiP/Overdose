using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDieEvents: MonoBehaviour
{
    [SerializeField] List<MonoBehaviour> disableScripts;
    [SerializeField] Animator anim;
    [SerializeField] EnemyHP _hp;
    NavMeshAgent _agent;

    public void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _hp = GetComponent<EnemyHP>();
        foreach(var mono in disableScripts)
        {
            if (mono == null) disableScripts.Remove(mono);
        }
    }

    private void OnEnable()
    {
        _hp.OnDie += Die;
    }
    private void OnDisable()
    {
        _hp.OnDie -= Die;
    }

    public void Die()
    {
        foreach (var mono in disableScripts)
        {
            mono.enabled = false;
        }
        _agent.enabled = false;
        anim.SetLayerWeight(1, 0);
        anim.CrossFade("Death", 0.2f);
    }
}
