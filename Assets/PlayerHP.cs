
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerHP : MonoBehaviour, IDamagable
{
    [SerializeField] float HP = 100f;
    [SerializeField] VolumeProfile profile;

    float MaxHP;
    public static event System.Action OnPlayerDie;

    Vignette vignette;

    void Start()
    {
        if (profile.TryGet(out vignette))
        {
            // Инициализация значения виньетки, если необходимо
            vignette.intensity.value = 0f;
        }
        MaxHP = HP;
    }

    public void GetDamage(float damage)
    {
        HP -= damage;
        UpdateVignette();

        if (HP <= 0)
        {
            OnPlayerDie?.Invoke();
        }
    }

    public void Heal(float d)
    {
        if(HP + d > MaxHP)
            HP= MaxHP;
        else
            HP += d;


        UpdateVignette();
    }

    void UpdateVignette()
    {
        if (vignette != null)
        {
            // Нормализуем HP до значения между 0 и 1
            float normalizedHP = Mathf.Clamp01(HP / MaxHP); // Предполагается, что максимальное HP - 100

            // Инвертируем значение, чтобы интенсивность увеличивалась при уменьшении HP
            float vignetteIntensity = (1 - normalizedHP) * 1;

            // Обновляем значение интенсивности виньетки
            vignette.intensity.value = vignetteIntensity;
        }
    }
}