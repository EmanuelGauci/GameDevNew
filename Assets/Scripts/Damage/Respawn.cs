using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
    public GameManager GameManager; // Reference to the game manager
    public GameObject[] SpawnPoints; // Array of spawn points
    public GameObject Player; // Reference to the player object
    public PlayerMovement playerMovement; // Reference to the player movement script
    public Rigidbody playerRigidbody;
    private int lastSpawnPointIndex = 0; // Index of the last spawn point used
    private bool isTouchingSpawn = false; // Flag indicating the player is touching a spawn point
    [SerializeField] private AudioSource SpawnAudioSource; // Audio source for spawning
    [SerializeField] private AudioClip deathSound; // Sound played upon player death
    private bool hasPlayerHitSpawn = false; // Flag indicating if the player has hit a spawn point
    [SerializeField] private GameObject RespawnCutscene; // Reference to the respawn cutscene object
    [SerializeField] private float paralysisDuration = 3f; // Duration of player paralysis after respawn cutscene
    private RigidbodyConstraints originalConstraints;

    void Update() {
        UpdateIsTouchingSpawn(); // Check if the player is touching a spawn point
        detectLastSpawnObject(); // Detect the last spawn point reached by the player
        if (GameManager.playerHealth <= 0 && !isTouchingSpawn) { // If player dies, respawn the player
            Spawn();
        }
    }
    
    private void Spawn() {
        PlayDeathSound(); // Play death sound
        MovePlayerToLastSpawnPoint(); // Move player to the last spawn point
        LoadingScreen(); // Display loading screen
    }

    private void PlayDeathSound() {
        if (SpawnAudioSource != null && deathSound != null) {
            SpawnAudioSource.PlayOneShot(deathSound);
        }
    }

    private void detectLastSpawnObject() {
        for (int i = 0; i < SpawnPoints.Length; i++) {
            Collider spawnCollider = SpawnPoints[i].GetComponent<Collider>();

            if (spawnCollider.bounds.Contains(Player.transform.position)) {
                if (i > lastSpawnPointIndex) {
                    VideoPlayerController videoController = RespawnCutscene.GetComponent<VideoPlayerController>();

                    if (videoController != null) {
                        videoController.StartCutscene(); // Start respawn cutscene
                        StartCoroutine(ParalyzePlayer()); // Paralyze the player
                    } else {
                        Debug.LogError("VideoPlayerController script not found on Cutscene1Controller GameObject.");
                    }

                    lastSpawnPointIndex = i; // Update last spawn point index
                    hasPlayerHitSpawn = true; // Set player hit spawn flag true
                }
            }
        }
    }

    private IEnumerator ParalyzePlayer() {
        playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        playerMovement.isParalyzed = true;
        yield return new WaitForSeconds(paralysisDuration); // Wait for paralysis duration
        playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePosition;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        playerMovement.isParalyzed = false;
    }

    private void MovePlayerToLastSpawnPoint() {
        Rigidbody playerRigidbody = Player.GetComponent<Rigidbody>();
        playerRigidbody.position = SpawnPoints[lastSpawnPointIndex].transform.position;
        GameManager.playerHealth = 3; // Reset player health
    }

    private void UpdateIsTouchingSpawn() {
        isTouchingSpawn = false;
        foreach (GameObject spawnPoint in SpawnPoints) {
            Collider spawnCollider = spawnPoint.GetComponent<Collider>();
            if (spawnCollider.bounds.Contains(Player.transform.position)) {
                isTouchingSpawn = true; // Set isTouchingSpawn to true if player is touching a spawn point
                return;
            }
        }
    }

    private void LoadingScreen() {
        // Not done yet
    }
}
