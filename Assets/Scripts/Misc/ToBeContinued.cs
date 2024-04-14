using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBeContinued : MonoBehaviour{
    private void Start() {
        
    }
    public void MainMenu() {
        SceneManager.LoadScene("MainMenuNew");
    }
    public void GameExit() {
        Application.Quit();
        //Debug.Log("Quit");
    }

}
