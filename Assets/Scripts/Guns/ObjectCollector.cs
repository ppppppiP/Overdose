using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Ссылка на камеру игрока
    [SerializeField] private float rayDistance = 5f; // Дистанция рейкаста
    [SerializeField] private KeyCode collectKey = KeyCode.E; // Клавиша для подбора объекта

    private void Update()
    {
        if (Input.GetKeyDown(collectKey))
        {
            CollectObject();
        }
    }

    private void CollectObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            ICollectableObject collectableObject = hit.collider.GetComponent<ICollectableObject>();
            if (collectableObject != null)
            {
                collectableObject.Collect();
            }
        }
    }
}