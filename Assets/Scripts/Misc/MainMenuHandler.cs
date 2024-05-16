using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuHandler : MonoBehaviour {
    [SerializeField] GameObject Options, Credits, SkipCutsceneGameObject, Background, loadingScreen;
    [SerializeField] GameObject Cutscene1Controller, Cutscene2Controller, VideoPlayer;
    [SerializeField] Scene nextScene;


    //variables for managing the current cutscene and audio
    private VideoPlayerController videoController;
    private AudioSource audioSource;
    private int currentCutsceneIndex = 1;



    private void Start() {
        StartCutscene1();//start the initial cutscene when the main menu loads
    }

    void PlayAudioClip(AudioClip clip) {//method to play an audio clip if available
        if (clip != null && audioSource != null) {//check if the audio clip and audio source are not null
            audioSource.clip = clip;//set the audio clip to the audio source and play it
            audioSource.Play();
        }
    }

    void StartCutscene1() {
        currentCutsceneIndex = 1;//set the current cutscene index to 1(used for the skip)
        videoController = Cutscene1Controller.GetComponent<VideoPlayerController>();//get the video player controller component from the first cutscene controller
        videoController.StartCutscene();//start the first cutscene
        SkipCutsceneGameObject.SetActive(true);//activate the skip button gameobject
        videoController.SetOnVideoFinishedAction(() => { Cutscene1Payload(); });//set the action to be performed when the first cutscene finishes
  
    }

    void Cutscene1Payload() {//ran after the first cutscene finishes
        Background.SetActive(true);//activate the background and title gameobjects
        SkipCutsceneGameObject.SetActive(false);//deactivate the skip button gameobject
        videoController.gameObject.SetActive(false); // Deactivate the Video Player component
    }

    public void StartGame() {
        //StartCutscene2();
        StartCoroutine(LoadSceneAsync("Level1"));//start loading the enxt scene asynchronously
    }

    public void StartCutscene2() {
        currentCutsceneIndex = 2;
        videoController = Cutscene2Controller.GetComponent<VideoPlayerController>();
        
        videoController.StartCutscene();
        SkipCutsceneGameObject.SetActive(true);
        videoController.SetOnVideoFinishedAction(() => { Cutscene2Payload(); });
    }

    void Cutscene2Payload() {
        ShowLoadingScreen();//show the loading screen
        SkipCutsceneGameObject.SetActive(false);//deactivate the skip button gameobject
        StartCoroutine(LoadSceneAsync("Level1"));//start loading the enxt scene asynchronously
        videoController.gameObject.SetActive(false); // Deactivate the Video Player component
    }

    IEnumerator LoadSceneAsync(string sceneName) {//coroutine to load the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);//start loading the scene asynchronously
        while (!operation.isDone) {//wait until the operation is done
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
        VideoPlayer.SetActive(false);//deactivate the videoplayer gameobject
        SkipCutsceneGameObject.SetActive(false);//deactivate the skip button game object
        
        if(currentCutsceneIndex == 1) {//check the current cutscene index
            Cutscene1Payload();// perform payload for the first cutscene
        }else if(currentCutsceneIndex == 2) {
            Cutscene2Payload();//perform payload for the second cutscene
        }
    }
}
