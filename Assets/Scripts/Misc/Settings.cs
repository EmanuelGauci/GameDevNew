using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    public GameObject SettingsMenu;
    public Slider ResolutionSlider;
    public Slider VolumeSlider;
    public Slider SensitivitySlider;
    private float VolumeLevel = 0.2f;
    [SerializeField] private GameManager gameManager;

    void Start() {
        LoadSettings(); // Load settings from GameManager
                        // Initialize the slider values
        VolumeSlider.value = VolumeLevel;
        ResolutionSlider.value = gameManager.ResolutionIndex;

        // Add listeners to handle value changes
        ResolutionSlider.onValueChanged.AddListener(OnResolutionSliderValueChanged);
        VolumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
        SensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);

        UpdateResolutionSliderValue(); // Update resolution slider position based on current resolution index

        // Call ChangeSpeed to apply initial sensitivity value
        ChangeSpeed();
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SettingsMenu.SetActive(false);
        }
    }

    void OnResolutionSliderValueChanged(float value) {
        gameManager.ResolutionIndex = value;
        ApplyResolution(); // Update resolution based on the slider value
    }

    void OnVolumeSliderValueChanged(float value) {
        VolumeLevel = value;
        ApplyVolume();
    }

    void OnSensitivitySliderValueChanged(float value) {
        ChangeSpeed();
    }

    void LoadSettings() {
        VolumeLevel = gameManager.VolumeLevel;
    }

    void ApplyVolume() {
        VolumeLevel = Mathf.Clamp01(VolumeLevel);
        AudioListener.volume = VolumeLevel;
        gameManager.VolumeLevel = VolumeLevel; // Save volume level to GameManager
    }

    void ApplyResolution() {
        gameManager.ApplyResolution(); // Let GameManager handle resolution change
        UpdateResolutionSliderValue(); // Update resolution slider position based on current resolution index
    }

    void ChangeSpeed() {
        gameManager.turnSpeed = (int)SensitivitySlider.value;
    }

    void UpdateResolutionSliderValue() {
        ResolutionSlider.value = gameManager.ResolutionIndex; // Update slider position based on current resolution index
    }
}

