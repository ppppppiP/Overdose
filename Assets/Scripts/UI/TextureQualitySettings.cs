using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextureQualitySettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown textureQualityDropdown;

    void Awake()
    {
        
        textureQualityDropdown.ClearOptions();
        List<string> options = new List<string> { "Low", "Medium", "High", "Ultra" };
        textureQualityDropdown.AddOptions(options);

       
        int savedQuality = PlayerPrefs.GetInt("TextureQuality", 1);
        textureQualityDropdown.value = savedQuality;
        textureQualityDropdown.RefreshShownValue();

        
        SetTextureQuality(savedQuality);

        textureQualityDropdown.onValueChanged.AddListener(SetTextureQuality);
    }

    public void SetTextureQuality(int qualityIndex)
    {
        QualitySettings.globalTextureMipmapLimit = 3 - qualityIndex; // 3 - Low, 2 - Medium, 1 - High, 0 - Ultra
        PlayerPrefs.SetInt("TextureQuality", qualityIndex);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        textureQualityDropdown.onValueChanged.RemoveListener(SetTextureQuality);
    }
}
