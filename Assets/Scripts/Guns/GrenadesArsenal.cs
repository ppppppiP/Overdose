using UnityEngine;

public class GrenadesArsenal : MonoBehaviour, IPlayerArsenal
{
    public int _grenadesCount = 10;
    public int _molotovCount = 1;
    public static System.Action<int, int> EOnGrenadeAdd;
    public static System.Action<int, int> EOnGrenadeRemove;
    public int CurrentGrenadeID;

    private void OnEnable()
    {
        if (_molotovCount == 0) _molotovCount = 1;
        if (_grenadesCount == 0) _grenadesCount = 1;
        EOnGrenadeAdd += AddItem;
        EOnGrenadeRemove += RemoveItem;
    }

    private void OnDisable()
    {
        EOnGrenadeAdd -= AddItem;
        EOnGrenadeRemove -= RemoveItem;
    }

    public void AddItem(int count, int grenadeID)
    {
        if (grenadeID == 1)
        {
            _grenadesCount += count;
        }
        else if (grenadeID == 2)
        {
            _molotovCount += count;
        }
    }

    public void RemoveItem(int count, int grenadeID)
    {
        if (grenadeID == 1)
        {
            _grenadesCount = Mathf.Max(0, _grenadesCount - count); // Не допускаем отрицательных значений
        }
        else if (grenadeID == 2)
        {
            _molotovCount = Mathf.Max(0, _molotovCount - count);
        }
    }

    public int GetGrenadeCount()
    {
        return _grenadesCount;
    }

    public int GetMolotovCount()
    {
        return _molotovCount;
    }
}