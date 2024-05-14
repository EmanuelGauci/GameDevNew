using System.Collections;
using UnityEngine;

public class SteamTrap : MonoBehaviour {
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float spawnInterval = 3f;
    private GameObject spawnedPrefab;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject player;
    [SerializeField] private float damageDelay = 1f;
    [SerializeField] private AudioSource steamAudioSource;
    [SerializeField] private AudioClip steamAudioClip;
    [SerializeField] private float rotationAngleMultiplier = 90f;
    [SerializeField] private float rotationValue = 0f;
    [SerializeField] private float scaleDuration = 0.5f;

    private void Start() {
        SpawnPrefab();
        InvokeRepeating("TogglePrefabVisibility", spawnInterval, spawnInterval);

        if (steamAudioSource && steamAudioClip) {
            steamAudioSource.clip = steamAudioClip;
        }
    }

    private void SpawnPrefab() {
        Quaternion rotation = Quaternion.Euler(0, rotationValue * rotationAngleMultiplier, 0);
        spawnedPrefab = Instantiate(prefabToSpawn, transform.position, rotation);
        spawnedPrefab.transform.localScale = Vector3.zero; // Set initial scale to zero
    }

    private void TogglePrefabVisibility() {
        if (spawnedPrefab != null) {
            if (!spawnedPrefab.activeSelf) {
                StartCoroutine(ScalePrefabIn());
                spawnedPrefab.SetActive(true);
                if (steamAudioSource && steamAudioClip) {
                    steamAudioSource.Play();
                }
            } else {
                StartCoroutine(ScalePrefabOutAndHide());
                if (steamAudioSource && steamAudioClip) {
                    StartCoroutine(FadeOutAudio());
                }
            }
        }
    }

    private IEnumerator FadeOutAudio() {
        float initialVolume = steamAudioSource.volume;
        while (steamAudioSource.volume > 0) {
            steamAudioSource.volume -= initialVolume * Time.deltaTime / damageDelay;
            yield return null;
        }
        steamAudioSource.Stop();
        steamAudioSource.volume = initialVolume;
    }

    private IEnumerator ScalePrefabIn() {
        float t = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        while (t < scaleDuration) {
            t += Time.deltaTime;
            spawnedPrefab.transform.localScale = Vector3.Lerp(initialScale, targetScale, t / scaleDuration);
            yield return null;
        }
        spawnedPrefab.transform.localScale = targetScale;
    }

    private IEnumerator ScalePrefabOutAndHide() {
        float t = 0f;
        Vector3 initialScale = spawnedPrefab.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        while (t < scaleDuration) {
            t += Time.deltaTime;
            spawnedPrefab.transform.localScale = Vector3.Lerp(initialScale, targetScale, t / scaleDuration);
            yield return null;
        }
        spawnedPrefab.transform.localScale = targetScale;
        spawnedPrefab.SetActive(false); // Hide the prefab after scaling out
    }

    private bool damageCooldown = false;

    private void FixedUpdate() {
        Collider playerCollider = player.GetComponent<Collider>();
        Collider enemyCollider = spawnedPrefab.GetComponent<Collider>();
        if (playerCollider.bounds.Intersects(enemyCollider.bounds) && !damageCooldown) {
            StartCoroutine(DealDamageWithDelay());
        }
    }

    private IEnumerator DealDamageWithDelay() {
        damageCooldown = true;
        gameManager.playerHealth -= 1;
        yield return new WaitForSeconds(damageDelay);
        damageCooldown = false;
    }
}
