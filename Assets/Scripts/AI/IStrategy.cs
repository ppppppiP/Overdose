using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


public interface IStrategy
{
    Node.Status Process();

    void Reset()
    {
        // Noop
    }
}

public class ActionStrategy : IStrategy
{
    readonly Action doSomething;

    public ActionStrategy(Action doSomething)
    {
        this.doSomething = doSomething;
    }

    public Node.Status Process()
    {
        doSomething();
        return Node.Status.Success;
    }
}

public class Condition : IStrategy
{
    readonly Func<bool> predicate;

    public Condition(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
}

public class PatrolStrategy : IStrategy
{
    readonly Transform entity;
    readonly NavMeshAgent agent;
    readonly List<Transform> patrolPoints;
    readonly float patrolSpeed;
    int currentIndex;
    bool isPathCalculated;

    public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
    {
        this.entity = entity;
        this.agent = agent;
        this.patrolPoints = patrolPoints;
        this.patrolSpeed = patrolSpeed;
    }

    public Node.Status Process()
    {
        if (currentIndex == patrolPoints.Count) return Node.Status.Success;

        var target = patrolPoints[currentIndex];
        agent.SetDestination(target.position);

        if (isPathCalculated && agent.remainingDistance < 0.1f)
        {
            currentIndex++;
            isPathCalculated = false;
        }

        if (agent.pathPending)
        {
            isPathCalculated = true;
        }

        return Node.Status.Running;
    }

    public void Reset() => currentIndex = 0;
}
public class WorkStrategy : IStrategy
{
    private Transform transform;
    private NavMeshAgent agent;
    private List<Transform> workPoints;
    private WorckerInventory inventory;
    private float workDuration;
    private float currentWorkTime =0;
    private int currentPointIndex = -1;

    public WorkStrategy(Transform transform, NavMeshAgent agent, List<Transform> workPoints, WorckerInventory inventory, float workDuration)
    {
        this.transform = transform;
        this.agent = agent;
        this.workPoints = workPoints;
        this.inventory = inventory;
        this.workDuration = workDuration;
    }

  
    public Node.Status Process()
    {
       // Debug.Log($"Current Work Time: {currentWorkTime}, Current Point Index: {currentPointIndex}");

        if (currentPointIndex == -1)
        {
            // Выбираем новую рабочую точку
            currentPointIndex = ChooseNextWorkPoint();
            if (currentPointIndex == -1)
            {
                // Нет доступных рабочих точек
                return Node.Status.Failure;
            }
            agent.SetDestination(workPoints[currentPointIndex].position);
            return Node.Status.Running;
        }

        // Проверяем, достиг ли агент текущей рабочей точки
        if (Vector3.Distance(transform.position, workPoints[currentPointIndex].position) < 2f)
        {
            // Агент на рабочей точке, увеличиваем время работы
            currentWorkTime += Time.deltaTime;

            if (currentWorkTime >= workDuration)
            {
                // Работа на текущей точке завершена
                workPoints[currentPointIndex].gameObject.SetActive(false);
                inventory.AddItem();
                currentWorkTime = 0;
                currentPointIndex = -1;
                return Node.Status.Success;
            }
        }
        else
        {
            // Агент все еще движется к рабочей точке
            if (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                return Node.Status.Running;
            }
        }

        return Node.Status.Running;
    }

    private int ChooseNextWorkPoint()
    {
        List<int> availablePoints = new List<int>();
        for (int i = 0; i < workPoints.Count; i++)
        {
            if (workPoints[i].gameObject.activeSelf)
            {
                availablePoints.Add(i);
            }
        }

        if (availablePoints.Count == 0)
        {
            return -1;
        }

        return availablePoints[UnityEngine.Random.Range(0, availablePoints.Count)];
    }

    public void Reset()
    {
        currentPointIndex = -1;
        currentWorkTime = 0;
    }
}
public class MoveToTarget : IStrategy
{
    readonly Transform entity;
    readonly NavMeshAgent agent;
    readonly Transform target;
    bool isPathCalculated;

    public MoveToTarget(Transform entity, NavMeshAgent agent, Transform target)
    {
        this.entity = entity;
        this.agent = agent;
        this.target = target;
    }

    public Node.Status Process()
    {
        if (Vector3.Distance(entity.position, target.position) < 2f)
        {
            return Node.Status.Success;
        }

        agent.SetDestination(target.position);
        

        if (agent.pathPending)
        {
            isPathCalculated = true;
        }
        return Node.Status.Running;
    }

    public void Reset() => isPathCalculated = false;
}