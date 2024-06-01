using System.IO;
using UnityEngine;

[System.Serializable]
public class GameManager : MonoBehaviour {
    public int playerHealth;
    public int turnSpeed;
    public float VolumeLevel = 0.2f;
    public float ResolutionIndex = 3;
    public TextAsset resolutionTextFile; // Drag your text file here

    private static GameManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager object when loading new scenes
        } else {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    void Start() {
        playerHealth = 3;
        turnSpeed = 30;
        LoadResolution(); // Load resolution when the game starts
    }

    public void ApplyResolution() {
        ApplyResolution((int)ResolutionIndex);
        SaveResolution(); // Save resolution after applying
    }

    void ApplyResolution(int index) {
        switch (index) {
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

    void ChangeResolution(int width, int height) {
        Screen.SetResolution(width, height, true);
    }

    void SaveResolution() {
        if (resolutionTextFile != null) {
            string filePath = Application.persistentDataPath + "/" + resolutionTextFile.name + ".txt";
            // Write resolution information to the text file
            using (StreamWriter writer = new StreamWriter(filePath)) {
                writer.WriteLine(Screen.width); // Write width
                writer.WriteLine(Screen.height); // Write height
            }
        } else {
            Debug.LogError("Resolution text file reference is missing!");
        }
    }

    void LoadResolution() {
        if (resolutionTextFile != null) {
            string filePath = Application.persistentDataPath + "/" + resolutionTextFile.name + ".txt";
            // Check if the file exists
            if (File.Exists(filePath)) {
                // Read resolution information from the text file
                using (StreamReader reader = new StreamReader(filePath)) {
                    int width = int.Parse(reader.ReadLine()); // Read width
                    int height = int.Parse(reader.ReadLine()); // Read height
                    Screen.SetResolution(width, height, true); // Apply resolution
                }
            }
        } else {
            Debug.LogError("Resolution text file reference is missing!");
        }
    }
}
