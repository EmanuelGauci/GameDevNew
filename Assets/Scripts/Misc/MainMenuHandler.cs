using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuHandler : MonoBehaviour {
    [SerializeField] GameObject Options, Credits, SkipCutsceneGameObject, Background, loadingScreen;
    [SerializeField] GameObject Cutscene1Controller, Cutscene2Controller, VideoPlayer;
    [SerializeField] Scene nextScene;

    // Variables for managing the current cutscene and audio
    private VideoPlayerController videoController;
    [SerializeField] private AudioSource audioSource;
    private int currentCutsceneIndex = 1;

    [SerializeField] private AudioClip backgroundMusic; // Add a field for the background music

    private void Start() {
        StartCutscene1(); // Start the initial cutscene when the main menu loads
    }

    void PlayAudioClip(AudioClip clip) {
        if (clip != null && audioSource != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    void StartCutscene1() {
        currentCutsceneIndex = 1;
        videoController = Cutscene1Controller.GetComponent<VideoPlayerController>();
        videoController.StartCutscene();
        SkipCutsceneGameObject.SetActive(true);
        videoController.SetOnVideoFinishedAction(() => { Cutscene1Payload(); });
    }

    void Cutscene1Payload() {
        Background.SetActive(true);
        SkipCutsceneGameObject.SetActive(false);
        videoController.gameObject.SetActive(false);
        // Play background music after the cutscene
        PlayAudioClip(backgroundMusic);
    }

    public void StartGame() {
        StartCoroutine(LoadSceneAsync("Level1"));
    }

    public void StartCutscene2() {
        currentCutsceneIndex = 2;
        videoController = Cutscene2Controller.GetComponent<VideoPlayerController>();
        videoController.StartCutscene();
        SkipCutsceneGameObject.SetActive(true);
        videoController.SetOnVideoFinishedAction(() => { Cutscene2Payload(); });
    }

    void Cutscene2Payload() {
        ShowLoadingScreen();
        SkipCutsceneGameObject.SetActive(false);
        StartCoroutine(LoadSceneAsync("Level1"));
        videoController.gameObject.SetActive(false);
        // Play background music after the cutscene
        PlayAudioClip(backgroundMusic);
    }

    IEnumerator LoadSceneAsync(string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) {
            ShowLoadingScreen();
            yield return null;
        }
    }

    void ShowLoadingScreen() {
        loadingScreen.SetActive(true);
    }

    public void OpenOptions() {
        Options.SetActive(true);
    }

    public void CloseOptions() {
        Options.SetActive(false);
    }

    public void OpenCredits() {
        Credits.SetActive(true);
    }

    public void GameExit() {
        Application.Quit();
    }

    public void SkipCutscene() {
        VideoPlayer.SetActive(false);
        SkipCutsceneGameObject.SetActive(false);

        if (currentCutsceneIndex == 1) {
            Cutscene1Payload();
        } else if (currentCutsceneIndex == 2) {
            Cutscene2Payload();
        }
    }
}
