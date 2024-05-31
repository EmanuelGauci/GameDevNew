using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.Video;

public class ToBeContinued : MonoBehaviour{
    [SerializeField] GameObject Cutscene1Controller, VideoPlayer;
    [SerializeField]private VideoPlayerController videoController;
    [SerializeField] private GameObject Background;
    private void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        StartCutscene1();
    }
    public void MainMenu() {
        SceneManager.LoadScene("MainMenuNew");
    }
    public void GameExit() {
        Application.Quit();
        //Debug.Log("Quit");
    }
    void StartCutscene1() {
        videoController = Cutscene1Controller.GetComponent<VideoPlayerController>();
        videoController.StartCutscene();
        videoController.SetOnVideoFinishedAction(() => { Cutscene1Payload(); });
    }
    void Cutscene1Payload() {
        Background.SetActive(true);
        videoController.gameObject.SetActive(false);
    }
}
