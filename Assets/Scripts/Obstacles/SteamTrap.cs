using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTrap : MonoBehaviour {
    [SerializeField] public GameObject prefabToSpawn;//reference to the prefab object to spawn
    [SerializeField] public float spawnInterval = 3f; // Adjust the interval as needed
    private GameObject spawnedPrefab;//reference to the spawned prefab
    [SerializeField] private GameManager gameManager;//reference to the game manager
    [SerializeField] private GameObject player;//reference to the player object
    [SerializeField] private float damageDelay = 1f;//delay between damage inflicted on the player
    [SerializeField] private AudioSource steamAudioSource;//audio source for steam sound
    [SerializeField] private AudioClip steamAudioClip;//audio clip for steam sound
    [SerializeField] private float rotationAngleMultiplier = 90f; // Multiplier to convert value to rotation angle

    [SerializeField] private float rotationValue = 0f; // Value to control the rotation angle from the inspector

    private void Start() {
        SpawnPrefab();//initial spawn of the prefab
        InvokeRepeating("TogglePrefabVisibility", spawnInterval, spawnInterval);

        if (steamAudioSource && steamAudioClip) {//play steam audio if audiosource and audioclip are assigned
            steamAudioSource.clip = steamAudioClip;
        }
    }

    private void SpawnPrefab() {
        Quaternion rotation = Quaternion.Euler(0, rotationValue * rotationAngleMultiplier, 0); // Calculate rotation based on rotationValue
        spawnedPrefab = Instantiate(prefabToSpawn, transform.position, rotation); // Instantiate the prefab with rotation
        spawnedPrefab.SetActive(false); // Hide the prefab initially
    }

    private void TogglePrefabVisibility() {
        if (spawnedPrefab != null) {
            //toggle visibility of the prefabs
            bool isActive = spawnedPrefab.activeSelf;
            spawnedPrefab.SetActive(!isActive);

            if (steamAudioSource && steamAudioClip) {//play or stop audio based on prefab visibility
                if (!isActive) {
                    steamAudioSource.Play();
                } else {
                    StartCoroutine(FadeOutAudio());
                }
            }
        }
    }

    private IEnumerator FadeOutAudio() {
        float initialVolume = steamAudioSource.volume;//fade out the audio gradually
        while (steamAudioSource.volume > 0) {
            steamAudioSource.volume -= initialVolume * Time.deltaTime / damageDelay;
            yield return null;
        }
        //stop the audio and reset volume for next play
        steamAudioSource.Stop();
        steamAudioSource.volume = initialVolume;
    }


    private bool damageCooldown = false;

    private void FixedUpdate() {
        //check for collision between player and spawnedPrefab
        Collider playerCollider = player.GetComponent<Collider>();
        Collider enemyCollider = spawnedPrefab.GetComponent<Collider>();
        if (playerCollider.bounds.Intersects(enemyCollider.bounds) && !damageCooldown) {
            StartCoroutine(DealDamageWithDelay());//initiate damage dealing with a delay
        }
    }

    private IEnumerator DealDamageWithDelay() {
        damageCooldown = true;//initiate damage cooldown
        gameManager.playerHealth -= 1;//inflict damage on the player
        yield return new WaitForSeconds(damageDelay);//wait for the damage delay before allowing further damage
        damageCooldown = false;//reset damage cooldown
    }
}
