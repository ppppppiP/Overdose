using UnityEngine;

public class ShootGunBullets : MonoBehaviour, IPlayerArsenal
{
    public int Count { get; private set; }
    public static System.Action<int> EOnBulletAdd;
    public static System.Action<int> EOnBulletRemove;

    private void OnEnable()
    {
        EOnBulletAdd += AddItem;
        EOnBulletRemove += RemoveItem;
    }
    private void OnDisable()
    {
        EOnBulletAdd -= AddItem;
        EOnBulletRemove -= RemoveItem;
    }
    public void AddItem(int count) => Count += count;
    public void RemoveItem(int count) => Count -= count;
}
