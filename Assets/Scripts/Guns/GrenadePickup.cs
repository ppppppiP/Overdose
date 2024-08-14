using UnityEngine;

public class GrenadePickup : MonoBehaviour, ICollectableObject
{
    [SerializeField] private int grenadeID = 1; // ID гранаты, которая будет подобрана
    [SerializeField] private int amount = 1; // Количество гранат, которые будут добавлены

    public void Collect()
    {
        // Генерируем событие добавления гранаты
        GrenadesArsenal.EOnGrenadeAdd?.Invoke(amount, grenadeID);
        // Уничтожаем объект гранаты после подбора
        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // Проверяем, столкнулись ли мы с игроком
    //    IPlayerArsenal playerArsenal = other.GetComponent<IPlayerArsenal>();
    //    if (playerArsenal != null)
    //    {
    //        Collect();
    //    }
    //}
}
