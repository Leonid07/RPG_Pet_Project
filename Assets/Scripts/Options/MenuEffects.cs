using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MenuEffects : MonoBehaviour
{
    public Toggle AmbientOcclusionToggle;
    public Toggle AutoExposureToggle;
    public Toggle BloomToggle;
    public Toggle DepthOfFieldToggle;
    public Toggle ColorGradingToggle;
    public Toggle GrainToggle;

    public PostProcessVolume PostProcessVolume;
    AmbientOcclusion AmbientOcclusion;
    AutoExposure AutoExposure;
    Bloom Bloom;
    DepthOfField DepthOfField;
    ColorGrading ColorGrading;
    Grain Grain;

    private const string AMBIENT_OCCLUSION_KEY = "AmbientOcclusion";
    private const string AUTO_EXPOSURE_KEY = "AutoExposure";
    private const string BLOOM_KEY = "Bloom";
    private const string DEPTH_OF_FIELD_KEY = "DepthOfField";
    private const string COLOR_GRADING_KEY = "ColorGrading";
    private const string GRAIN_KEY = "Grain";
    void Start()
    {
        PostProcessVolume.profile.TryGetSettings(out AmbientOcclusion);
        PostProcessVolume.profile.TryGetSettings(out AutoExposure);
        PostProcessVolume.profile.TryGetSettings(out Bloom);
        PostProcessVolume.profile.TryGetSettings(out DepthOfField);
        PostProcessVolume.profile.TryGetSettings(out ColorGrading);
        PostProcessVolume.profile.TryGetSettings(out Grain);

        //Установка состояния переключателей и применение этих настроек к эффектам
        bool isAmbientOcclusionEnabled = PlayerPrefs.GetInt(AMBIENT_OCCLUSION_KEY, AmbientOcclusion.active ? 1 : 0) == 1;
        AmbientOcclusionToggle.isOn = isAmbientOcclusionEnabled;
        AmbientOcclusion.active = isAmbientOcclusionEnabled; // Обновление активности эффекта

        bool isAutoExposureEnabled = PlayerPrefs.GetInt(AUTO_EXPOSURE_KEY, AutoExposure.active ? 1 : 0) == 1;
        AutoExposureToggle.isOn = isAutoExposureEnabled;
        AutoExposure.active = isAutoExposureEnabled; // Обновление активности эффекта

        bool isBloomEnabled = PlayerPrefs.GetInt(BLOOM_KEY, Bloom.active ? 1 : 0) == 1;
        BloomToggle.isOn = isBloomEnabled;
        Bloom.active = isBloomEnabled; // Обновление активности эффекта

        bool isDepthOfFieldEnabled = PlayerPrefs.GetInt(DEPTH_OF_FIELD_KEY, DepthOfField.active ? 1 : 0) == 1;
        DepthOfFieldToggle.isOn = isDepthOfFieldEnabled;
        DepthOfField.active = isDepthOfFieldEnabled; // Обновление активности эффекта

        bool isColorGradingEnabled = PlayerPrefs.GetInt(COLOR_GRADING_KEY, ColorGrading.active ? 1 : 0) == 1;
        ColorGradingToggle.isOn = isColorGradingEnabled;
        ColorGrading.active = isColorGradingEnabled; // Обновление активности эффекта

        bool isGrainEnabled = PlayerPrefs.GetInt(GRAIN_KEY, Grain.active ? 1 : 0) == 1;
        GrainToggle.isOn = isGrainEnabled;
        Grain.active = isGrainEnabled; // Обновление активности эффекта

        AmbientOcclusionToggle.onValueChanged.AddListener(SetAmbientOcclusion);
        AutoExposureToggle.onValueChanged.AddListener(SetAutoExposure);
        BloomToggle.onValueChanged.AddListener(SetBloom);
        DepthOfFieldToggle.onValueChanged.AddListener(SetDepthOfField);
        ColorGradingToggle.onValueChanged.AddListener(SetColorGrading);
        GrainToggle.onValueChanged.AddListener(SetGrain);
    }

    public void SetAmbientOcclusion(bool isEnabled)
    {
        AmbientOcclusion.active = isEnabled;
        PlayerPrefs.SetInt(AMBIENT_OCCLUSION_KEY, isEnabled ? 1 : 0);
    }
    public void SetAutoExposure(bool isEnabled)
    {
        AutoExposure.active = isEnabled;
        PlayerPrefs.SetInt(AUTO_EXPOSURE_KEY, isEnabled ? 1 : 0);
    }
    public void SetBloom(bool isEnabled)
    {
        Bloom.active = isEnabled;
        PlayerPrefs.SetInt(BLOOM_KEY, isEnabled ? 1 : 0);
    }
    public void SetDepthOfField(bool isEnabled)
    {
        DepthOfField.active = isEnabled;
        PlayerPrefs.SetInt(DEPTH_OF_FIELD_KEY, isEnabled ? 1 : 0);
    }
    public void SetColorGrading(bool isEnabled)
    {
        ColorGrading.active = isEnabled;
        PlayerPrefs.SetInt(COLOR_GRADING_KEY, isEnabled ? 1 : 0);
    }
    public void SetGrain(bool isEnabled)
    {
        Grain.active = isEnabled;
        PlayerPrefs.SetInt(GRAIN_KEY, isEnabled ? 1 : 0);
    }
}
