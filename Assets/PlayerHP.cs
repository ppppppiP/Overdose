
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
            // ������������� �������� ��������, ���� ����������
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


        UpdateVignette1();
    }



    void UpdateVignette()
    {
        if (vignette != null)
        {
            // ����������� HP �� �������� ����� 0 � 1
            float normalizedHP = Mathf.Clamp01(HP / MaxHP); // ��������������, ��� ������������ HP - 100

            // ����������� ��������, ����� ������������� ������������� ��� ���������� HP
            float vignetteIntensity = (1 - normalizedHP) * 1;

            // ��������� �������� ������������� ��������
            vignette.intensity.value = vignetteIntensity;
        }
    }void UpdateVignette1()
    {
        if (vignette != null)
        {
            
            vignette.intensity.value = 0;
        }
    }
}