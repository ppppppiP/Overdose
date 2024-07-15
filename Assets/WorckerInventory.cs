using BlackBoardSystem;
using UnityEngine;
using UnityServiceLocator;

public class WorckerInventory : MonoBehaviour, IExpert
{
    public int InventorySize;
    public int MaxSize;

    Blackboard blackboard;
    BlackboardKey isFullInventoryKey;

    private void Start()
    {
        blackboard = ServiceLocator.For(this).Get<BlackboardController>().GetBlackboard();
        ServiceLocator.For(this).Get<BlackboardController>().RegisterExpert(this);
        isFullInventoryKey = blackboard.GetOrRegisterKey("IsFullInventory");
        //UpdateInventoryStatus();
    }

    public void AddItem()
    {
        InventorySize++;
        UpdateInventoryStatus();
    }

    public void ClearInventory()
    {
        InventorySize = 0;
        UpdateInventoryStatus();
    }

    private void UpdateInventoryStatus()
    {
        bool isFull = InventorySize >= MaxSize;
        blackboard.SetValue(isFullInventoryKey, !isFull);
    }

    public void Execute(Blackboard blackboard)
    {
        // Этот метод может быть пустым, так как обновление происходит в UpdateInventoryStatus
    }

    public int GetInstance(Blackboard blackboard)
    {
        return blackboard.TryGetValue(isFullInventoryKey, out bool isFull) && isFull ? 10 : 0;
    }
}
