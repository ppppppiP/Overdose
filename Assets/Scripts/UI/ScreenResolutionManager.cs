using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    private const string ResolutionWidthKey = "ResolutionWidth";
    private const string ResolutionHeightKey = "ResolutionHeight";
    private const string FullscreenKey = "Fullscreen";

    void Start()
    {
        
        resolutions = Screen.resolutions;
        RefreshRate rate = Screen.currentResolution.refreshRateRatio;
        Debug.Log(rate.Equals(resolutions[1].refreshRateRatio));
        HashSet<string> resolutionOptionsSet = new HashSet<string>();

      
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            if (!resolutionOptionsSet.Contains(option))
            {
                resolutionOptionsSet.Add(option);
                options.Add(option);
                uniqueResolutions.Add(resolutions[i]);

               
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

      
        if (!PlayerPrefs.HasKey(ResolutionWidthKey) || !PlayerPrefs.HasKey(ResolutionHeightKey))
        {
            
            PlayerPrefs.SetInt(ResolutionWidthKey, Screen.currentResolution.width);
            PlayerPrefs.SetInt(ResolutionHeightKey, Screen.currentResolution.height);
            PlayerPrefs.SetInt(FullscreenKey, Screen.fullScreen ? 1 : 0);
        }

      
        int savedWidth = PlayerPrefs.GetInt(ResolutionWidthKey);
        int savedHeight = PlayerPrefs.GetInt(ResolutionHeightKey);
        bool isFullscreen = PlayerPrefs.GetInt(FullscreenKey) == 1;

       
        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            if (uniqueResolutions[i].width == savedWidth && uniqueResolutions[i].height == savedHeight)
            {
                currentResolutionIndex = i;
                break;
            }
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = isFullscreen;
        Screen.SetResolution(savedWidth, savedHeight, isFullscreen);
    }

    public void SetResolution(int resolutionIndex)
    {
        resolutionIndex = resolutionDropdown.value;
        Resolution resolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log(resolution.width + " " + resolution.height);
       
        PlayerPrefs.SetInt(ResolutionWidthKey, resolution.width);
        PlayerPrefs.SetInt(ResolutionHeightKey, resolution.height);
        PlayerPrefs.Save(); 
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
       
        PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();  
        Debug.Log(isFullscreen ? 1 : 0);
    }
}