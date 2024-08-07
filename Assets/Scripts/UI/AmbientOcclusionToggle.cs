using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

using UnityEngine.Rendering.HighDefinition;
public class AmbientOcclusionToggle : MonoBehaviour
{
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] Toggle aoToggle;

    ScreenSpaceAmbientOcclusion ambientOcclusion;

    void Start()
    {
        if (volumeProfile != null)
        {
            volumeProfile.TryGet(out ambientOcclusion);
        }

        aoToggle.isOn = PlayerPrefs.GetInt("AOToggle", ambientOcclusion != null && ambientOcclusion.active ? 1 : 0) == 1;
        aoToggle.onValueChanged.AddListener(SetAmbientOcclusion);
        SetAmbientOcclusion(aoToggle.isOn);
    }

    public void SetAmbientOcclusion(bool isOn)
    {
        if (ambientOcclusion != null)
        {
            ambientOcclusion.active = isOn;
            PlayerPrefs.SetInt("AOToggle", isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void OnDestroy()
    {
        aoToggle.onValueChanged.RemoveListener(SetAmbientOcclusion);
    }
}