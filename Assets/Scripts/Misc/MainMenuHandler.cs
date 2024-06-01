using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuHandler : MonoBehaviour {
    [SerializeField] GameObject Options, Credits, SkipCutsceneGameObject, Background, loadingScreen;
    [SerializeField] GameObject Cutscene1Controller, Cutscene2Controller, VideoPlayer;
    [SerializeField] Scene nextScene;

    [SerializeField] GameObject CreditsImage;
    [SerializeField] float scrollSpeed = 10f; // Adjust the scroll speed as needed

    private bool isCreditsOpen = false;

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
        isCreditsOpen = true;
    }

    public void CloseCredits() {
        Credits.SetActive(false );
        isCreditsOpen = false;
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

    private void Update() {
        if (isCreditsOpen) {
            // Scroll the CreditsImage along the Y axis using the mouse wheel
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * -1;
            CreditsImage.transform.Translate(Vector3.up * scrollAmount * scrollSpeed);

            // Constrain the position within the specified range
            Vector3 currentPosition = CreditsImage.transform.localPosition;
            currentPosition.y = Mathf.Clamp(currentPosition.y, -873f, 1832f);
            CreditsImage.transform.localPosition = currentPosition;
        }
    }
}
