using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Настройки тряски
    public float shakeIntensity = 0.2f; // Сила тряски
    public float shakeDuration = 0.2f; // Продолжительность тряски
    public float dampingSpeed = 5f; // Скорость затухания тряски

    private Vector3 originalPosition; // Исходное положение камеры
    private float elapsedTime = 0f; // Прошедшее время с начала тряски

    public static CameraShake Instance;

    void Start()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    // Вызывается при попадании пули
    public void ShakeCamera()
    {
        elapsedTime = 0f; // Сброс времени
    }

    // Обновление состояния камеры
    void Update()
    {
        if (elapsedTime < shakeDuration)
        {
            // Генерация случайного смещения для тряски
            float xOffset = Random.Range(-shakeIntensity, shakeIntensity);
            float yOffset = Random.Range(-shakeIntensity, shakeIntensity);

            // Применение смещения к положению камеры
            transform.localPosition = originalPosition + new Vector3(xOffset, yOffset, 0f);

            // Увеличение времени
            elapsedTime += Time.deltaTime;
        }
        else
        {
            // Плавно возвращаем камеру в исходное положение
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, dampingSpeed * Time.deltaTime);
        }
    }
}