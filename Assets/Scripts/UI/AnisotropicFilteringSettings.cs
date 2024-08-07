using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class AnisotropicFilteringSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown anisotropicFilteringDropdown;

    void Start()
    {
        List<string> options = new List<string> { "Disable", "X8", "X16" };
        anisotropicFilteringDropdown.ClearOptions();
        anisotropicFilteringDropdown.AddOptions(options);

        int savedAF = PlayerPrefs.GetInt("AnisotropicFiltering", (int)AnisotropicFiltering.Enable);
        anisotropicFilteringDropdown.value = savedAF;
        anisotropicFilteringDropdown.RefreshShownValue();

        SetAnisotropicFiltering(savedAF);
        anisotropicFilteringDropdown.onValueChanged.AddListener(SetAnisotropicFiltering);
    }

    public void SetAnisotropicFiltering(int index)
    {
        QualitySettings.anisotropicFiltering = (AnisotropicFiltering)index;
        PlayerPrefs.SetInt("AnisotropicFiltering", index);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        anisotropicFilteringDropdown.onValueChanged.RemoveListener(SetAnisotropicFiltering);
    }
}
