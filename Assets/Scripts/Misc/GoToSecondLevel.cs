using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSecondLevel : MonoBehaviour {
    [SerializeField] private GameObject loadingScreen;

    private void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void GoToLevelTwo() {
        StartCoroutine(LoadLevel("Level2"));
    }

    public void GoToLevelOne() {
        StartCoroutine(LoadLevel("Level1"));
    }

    private IEnumerator LoadLevel(string levelName) {
        loadingScreen.SetActive(true); // Activate loading screen
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        while (!asyncLoad.isDone) {
            yield return null; // Wait until the scene is fully loaded
        }
        loadingScreen.SetActive(false); // Deactivate loading screen after loading
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenuNew");
    }

    public void GameExit() {
        Application.Quit();
        //Debug.Log("Quit");
    }
}
