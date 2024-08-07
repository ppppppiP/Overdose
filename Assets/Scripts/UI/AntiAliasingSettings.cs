using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AntiAliasingSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown antiAliasingDropdown;

    void Start()
    {
        List<string> options = new List<string> { "Off", "2x MSAA", "4x MSAA", "8x MSAA" };
        antiAliasingDropdown.ClearOptions();
        antiAliasingDropdown.AddOptions(options);

        int savedAA = PlayerPrefs.GetInt("AntiAliasing", 0);
        antiAliasingDropdown.value = savedAA;
        antiAliasingDropdown.RefreshShownValue();

        SetAntiAliasing(savedAA);
        antiAliasingDropdown.onValueChanged.AddListener(SetAntiAliasing);
    }

    public void SetAntiAliasing(int index)
    {
        switch (index)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }
        PlayerPrefs.SetInt("AntiAliasing", index);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        antiAliasingDropdown.onValueChanged.RemoveListener(SetAntiAliasing);
    }
}