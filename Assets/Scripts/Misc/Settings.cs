using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    public GameObject SettingsMenu;
    public Slider ResolutionSlider; // Reference to the resolution slider
    public Slider VolumeSlider; // Reference to the volume slider
    public Slider SensitivitySlider;//reference to the sensitivity slider

    private float ResolutionIndex = 1; // Variable to store the resolution slider value
    private float VolumeLevel = 0.2f; // Variable to store the volume slider value, initialized to 0.2
    [SerializeField]private GameManager gameManager; //to take gameManager.turnSpeed

    void Start() {
        // Initialize the slider values
        ResolutionIndex = ResolutionSlider.value;
        VolumeSlider.value = VolumeLevel; 

        // Add listeners to handle value changes
        ResolutionSlider.onValueChanged.AddListener(OnResolutionSliderValueChanged);
        VolumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
        SensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);
        ApplySettings();//apply initial settings

    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SettingsMenu.SetActive(false);
            // Add your interaction logic here
        }
    }

    void OnResolutionSliderValueChanged(float value) {
        ResolutionIndex = value;
        
    }

    void OnVolumeSliderValueChanged(float value) {
        VolumeLevel = value;
        ApplyVolume();
    }
    void OnSensitivitySliderValueChanged(float value) {
        ChangeSpeed();
    }

    public void ApplySettings() {
        ApplyResolution();
        ApplyVolume();
    }

    void ApplyResolution() {
        // Set resolution based on the index
        switch (Mathf.RoundToInt(ResolutionIndex)) {
            case 0:
                ChangeResolution(1280, 720); // 720p
                Debug.Log("720p");
                break;
            case 1:
                ChangeResolution(1920, 1080); // 1080p
                Debug.Log("1080p");
                break;
            case 2:
                ChangeResolution(2560, 1440); // 1440p
                Debug.Log("2k");
                break;
            case 3:
                ChangeResolution(3840, 2160); // 2160p
                Debug.Log("4k");
                break;
            default:
                break;
        }
    }

    void ApplyVolume() {
        VolumeLevel = Mathf.Clamp01(VolumeLevel);
        AudioListener.volume = VolumeLevel;
    }

    void ChangeResolution(int width, int height) {
        Screen.SetResolution(width, height, true);
    }

    void ChangeSpeed() {
        gameManager.turnSpeed = (int)SensitivitySlider.value;
    }




}
