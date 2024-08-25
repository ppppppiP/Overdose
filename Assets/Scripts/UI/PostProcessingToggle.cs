using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessingToggle : MonoBehaviour
{
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] Toggle bloomToggle;
    [SerializeField] Toggle fogToggle;
    [SerializeField] Toggle ssrToggle;
    [SerializeField] Slider filmGrainSlider;
    [SerializeField] Slider gammaSlider;

    Bloom bloom;
    Fog fog;
    ScreenSpaceReflection ssr;
    FilmGrain filmGrain;
    Exposure exposure;
    LiftGammaGain liftGammaGain;

    void Awake()
    {
      
        if (volumeProfile != null)
        {
            volumeProfile.TryGet(out bloom);
            volumeProfile.TryGet(out fog);
            volumeProfile.TryGet(out ssr);
            volumeProfile.TryGet(out filmGrain);
            volumeProfile.TryGet(out liftGammaGain);
        }

        
        if (bloomToggle != null)
        {
            bloomToggle.isOn = PlayerPrefs.GetInt("BlurToggle", bloom != null && bloom.active ? 1 : 0) == 1;
            bloomToggle.onValueChanged.AddListener(OnBlurToggleChanged);
        }

        if (fogToggle != null)
        {
            fogToggle.isOn = PlayerPrefs.GetInt("FogToggle", fog != null && fog.active ? 1 : 0) == 1;
            fogToggle.onValueChanged.AddListener(OnFogToggleChanged);
        }

        if (ssrToggle != null)
        {
            ssrToggle.isOn = PlayerPrefs.GetInt("SSRToggle", ssr != null && ssr.active ? 1 : 0) == 1;
            ssrToggle.onValueChanged.AddListener(OnSSRToggleChanged);
        }

        if (filmGrainSlider != null)
        {
            filmGrainSlider.value = PlayerPrefs.GetFloat("FilmGrainIntensity", filmGrain != null ? filmGrain.intensity.value : 0f);
            filmGrainSlider.onValueChanged.AddListener(OnFilmGrainSliderChanged);
        }
        if (gammaSlider != null)
        {
            gammaSlider.minValue = -0.5f; 
            gammaSlider.maxValue = 1f; 
            gammaSlider.value = PlayerPrefs.GetFloat("GammaValue", liftGammaGain != null ? liftGammaGain.gamma.value.x : 1f);
            gammaSlider.onValueChanged.AddListener(OnGammaSliderChanged);
        }

       
        OnBlurToggleChanged(bloomToggle.isOn);
        OnFogToggleChanged(fogToggle.isOn);
        OnSSRToggleChanged(ssrToggle.isOn);
        OnFilmGrainSliderChanged(filmGrainSlider.value);
        OnGammaSliderChanged(gammaSlider.value);
    }

    void OnBlurToggleChanged(bool isOn)
    {
        if (bloom != null)
        {
            bloom.active = isOn;
            PlayerPrefs.SetInt("BlurToggle", isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void OnFogToggleChanged(bool isOn)
    {
        if (fog != null)
        {
            fog.active = isOn;
            PlayerPrefs.SetInt("FogToggle", isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void OnSSRToggleChanged(bool isOn)
    {
        if (ssr != null)
        {
            ssr.active = isOn;
            PlayerPrefs.SetInt("SSRToggle", isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void OnFilmGrainSliderChanged(float value)
    {
        if (filmGrain != null)
        {
            filmGrain.intensity.value = value;
            PlayerPrefs.SetFloat("FilmGrainIntensity", value);
            PlayerPrefs.Save();
        }
    }
    void OnGammaSliderChanged(float value)
    {
        if (liftGammaGain != null)
        {
            liftGammaGain.gamma.value = new Vector4(2, 2, 2, value);
            PlayerPrefs.SetFloat("GammaValue", value);
            PlayerPrefs.Save();
        }
    }

    void OnDestroy()
    {
        
        if (bloomToggle != null)
        {
            bloomToggle.onValueChanged.RemoveListener(OnBlurToggleChanged);
        }

        if (fogToggle != null)
        {
            fogToggle.onValueChanged.RemoveListener(OnFogToggleChanged);
        }

        if (ssrToggle != null)
        {
            ssrToggle.onValueChanged.RemoveListener(OnSSRToggleChanged);
        }

        if (filmGrainSlider != null)
        {
            filmGrainSlider.onValueChanged.RemoveListener(OnFilmGrainSliderChanged);
        }

       
    }
}
