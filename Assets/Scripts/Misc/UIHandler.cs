using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour {
    [SerializeField] public GameObject Cig1, Cig2, Cig3;//references to cigarette game objects
    [SerializeField] public GameObject Pick1, Pick2, Pick3, Pick4;//references to pick game objects
    public GameManager gameManager;//reference to the game manager
    public GameObject PausePanel;//referenmce to the pause panel game object
    public PlayerMovement playerMovement;//reference to theplayer movement script
    public bool isPaused = false;//flag to track if the game is paused

    private float originalVolume; // Store the original volume

    // Update is called once per frame
    void Update() {
        UpdateCigaretteVisibility();//update the visibility of cigarette game objects
        UpdatePickVisibility();//update the visibility of pick game objects

        if (Input.GetKeyDown(KeyCode.Escape)) {//check for the esc keypress to pause or continue the game
            if (isPaused) {
                Continue();//continue the game
            } else {
                Pause();//pause the game
            }
        }
    }

    void UpdateCigaretteVisibility() {//update the visibility of the cigarete game objects based on player health
        Cig1.SetActive(gameManager.playerHealth >= 1);
        Cig2.SetActive(gameManager.playerHealth >= 2);
        Cig3.SetActive(gameManager.playerHealth >= 3);
    }

    void UpdatePickVisibility() {//update the visibility of pick game objects based on the pick amounts
        Pick1.SetActive(gameManager.PickAmount >= 1);
        Pick2.SetActive(gameManager.PickAmount >= 2);
        Pick3.SetActive(gameManager.PickAmount >= 3);
        Pick4.SetActive(gameManager.PickAmount >= 4);
    }

    public void Pause() {//pause the game
        PausePanel.SetActive(true);//set the pause panel to active
        originalVolume = AudioListener.volume; // Store the original volume
        AudioListener.volume = 0; // Set volume to 0 when paused
        Time.timeScale = 0;//set the time scale to 0 to pause the game
        isPaused = true;//set the isPaused flag to true
        playerMovement.SelectWithMouse();//call the select with mouse method from the player movement script
    }

    public void Continue() {//continue the game
        PausePanel.SetActive(false);//set the pause panel to inactive
        AudioListener.volume = originalVolume; // Restore the original volume
        Time.timeScale = 1;//set the time scale to 1 to continue the game
        isPaused = false;//set the isPaused flag to false
        playerMovement.TurnWithMouse();//call the TurnWithMouse method from the player movement script
    }
}
