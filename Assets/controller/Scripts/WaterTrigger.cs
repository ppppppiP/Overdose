using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private float waterSurfaceLevel = 0.0f;  // Уровень поверхности воды
    [SerializeField] private float swimDepthThreshold = 2.0f;  // Глубина, на которой игрок может плавать

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerSwimming>())
        {
            PlayerSwimming playerSwimming = other.GetComponent<PlayerSwimming>();
            if (playerSwimming != null)
            {
                playerSwimming.SetWaterParameters(true);
                Debug.Log("Sweem");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerSwimming>())
        {
            PlayerSwimming playerSwimming = other.GetComponent<PlayerSwimming>();
            if (playerSwimming != null)
            {
                playerSwimming.SetWaterParameters(false);
            }
        }
    }
}