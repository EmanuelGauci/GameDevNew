using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
    public GameManager GameManager;//reference to the game manager
    public GameObject[] SpawnPoints;//array of spawn points
    public GameObject Player;//reference to the player object
    public PlayerMovement PlayerMovement;//reference to the player movement script
    private int lastSpawnPointIndex = 0;//index of the last spawn point used
    private bool isTouchingSpawn = false;//flag indicating the player is touching a spawn point
    [SerializeField] private AudioSource SpawnAudioSource;//audio source for spawning
    [SerializeField] private AudioClip deathSound;//sound played upon player death
    private bool hasPlayerHitSpawn = false;//flag indicating if the player has hit a spawn point
    [SerializeField] private GameObject RespawnCutscene;//reference to the respawn cutscene object

    void Update() {//update is called once per  frame
        UpdateIsTouchingSpawn();//checkk if the player is touching a spawn point
        detectLastSpawnObject();//detect the last spawn point reached by the player
        if (GameManager.playerHealth <= 0 && !isTouchingSpawn) { // if player dies, respawn the player
            Spawn();
        }
    }

    private void Spawn() {
        PlayDeathSound();//play death sound
        MovePlayerToLastSpawnPoint();//move player to the last spawn point
        GameManager.playerHealth = 3;//reset player health
        LoadingScreen();//display loading screen
    }

    private void PlayDeathSound() {//plays death sound
        if (SpawnAudioSource != null && deathSound != null) {
            SpawnAudioSource.PlayOneShot(deathSound);
        }
    }

    private void detectLastSpawnObject() {// detects the last spawn point reached by the player
        for (int i = 0; i < SpawnPoints.Length; i++) {
            Collider spawnCollider = SpawnPoints[i].GetComponent<Collider>();

            if (spawnCollider.bounds.Contains(Player.transform.position) && i > 0 && !hasPlayerHitSpawn) {
                VideoPlayerController videoController = RespawnCutscene.GetComponent<VideoPlayerController>();

                if (videoController != null) {
                    videoController.StartCutscene();//start respawn cutscene
                } else {
                    Debug.LogError("VideoPlayerController script not found on Cutscene1Controller GameObject.");
                }
                lastSpawnPointIndex = i;//update last spawn point index
                hasPlayerHitSpawn = true;//set player hit spawn flag true
            }
        }
    }

    private void MovePlayerToLastSpawnPoint() {//moves the player to the last spawn point
        Rigidbody playerRigidbody = Player.GetComponent<Rigidbody>();
        playerRigidbody.position = SpawnPoints[lastSpawnPointIndex].transform.position;
    }

    private void UpdateIsTouchingSpawn() {//updates whether the player is touching a spawn point
        isTouchingSpawn = false;
        foreach (GameObject spawnPoint in SpawnPoints) {
            Collider spawnCollider = spawnPoint.GetComponent<Collider>();
            if (spawnCollider.bounds.Contains(Player.transform.position)) {
                isTouchingSpawn = true;//set isTouchingSpawn to true if player is touching a spawn point
                return;
            }
        }
    }

    private void LoadingScreen() {
       //not done yet
    }
}
