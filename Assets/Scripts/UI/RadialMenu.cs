using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public GameObject radialMenuUI; // Префаб или объект UI, который содержит круговое меню
    public Image[] menuItems; // Массив изображений для элементов меню (граната, молотов и т.д.)
    public float scaleFactor = 1.2f; // Коэффициент масштабирования для выделенного элемента
    public KeyCode toggleKey = KeyCode.G;
    public TextMeshProUGUI[] itemCounts;// Клавиша, на которой открывается меню
    [SerializeField] GrenadeController controller;
    [SerializeField] GrenadesArsenal grenades; // Ссылка на арсенал гранат

    private bool isMenuActive = false; // Активно ли меню
    private int selectedItemIndex = -1; // Индекс выбранного элемента
    private bool isAnyItemAvailable = false; // Есть ли доступные элементы для выбора

    float timer;

    async void Update()
    {
        if (Input.GetKey(toggleKey))
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                isMenuActive = true;
                radialMenuUI.SetActive(true);
                Cursor.visible = true; // Показываем курсор
                Cursor.lockState = CursorLockMode.Confined;

                // Обновляем меню с учетом доступных предметов
                UpdateMenu();
            }
        }

        if (Input.GetKeyUp(toggleKey))
        {
            timer = 0;
            await Task.Delay(100);
            isMenuActive = false;
            radialMenuUI.SetActive(false);
            Cursor.visible = false; // Скрываем курсор

            if (selectedItemIndex != -1)
            {
                HandleSelection(selectedItemIndex);
            }
            ResetMenu();
        }
    }

    void UpdateMenu()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 centerScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 direction = mousePosition - centerScreen;

        float angleStep = 360f / menuItems.Length;
        float startAngle = -90f;

        selectedItemIndex = -1;
        float minDistance = float.MaxValue;

        for (int i = 0; i < menuItems.Length; i++)
        {
            // Отображаем количество предметов
            if (i == 0)
            {
                itemCounts[i].text = grenades.GetGrenadeCount().ToString();
            }
            else if (i == 1)
            {
                itemCounts[i].text = grenades.GetMolotovCount().ToString();
            }

            float angle = startAngle + i * angleStep;
            Vector2 itemDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            float distance = Vector2.Distance(direction.normalized, itemDirection);

            if (distance < minDistance)
            {
                minDistance = distance;
                selectedItemIndex = i;
            }

            menuItems[i].transform.localScale = Vector3.one; // Сброс масштаба
        }

        if (selectedItemIndex != -1)
        {
            menuItems[selectedItemIndex].transform.localScale = Vector3.one * scaleFactor;
            // Увеличиваем выделенный элемент
        }
    }


    void HandleSelection(int index)
    {
        
        switch (index)
        {
            case 0:
                Debug.Log("Граната выбрана");
                controller.currentGrenID = 1;
                grenades.CurrentGrenadeID = 1;
                break;
            case 1:
                Debug.Log("Молотов выбран");
                controller.currentGrenID = 2;
                grenades.CurrentGrenadeID = 2;
                break;
            default:
                break;
        }
    }

    void ResetMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        selectedItemIndex = -1;
        foreach (var item in menuItems)
        {
            item.transform.localScale = Vector3.one; // Сброс масштаба всех элементов
        }
    }
}