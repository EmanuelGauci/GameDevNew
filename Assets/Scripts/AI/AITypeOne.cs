using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AITypeOne : MonoBehaviour{
    public Transform PlayerObj;
    public GameObject EnemyPrefab;
    [System.NonSerialized] public float detectionRadius = 30f;
    [System.NonSerialized] public float detectionInterval = 4f;
    [System.NonSerialized] public float spawnDelay = 2f;
    private float lastDetectionTime;
    private Vector3 detectedPlayerPosition;

    private void Update() {
        if(Time.time- lastDetectionTime >= detectionInterval) {//check if time elapsed since the last detection is greater or equal to the detection interaval
            lastDetectionTime = Time.time;//update the last detection time to the current time
            
            if (isPlayerWithinRadius()) {//check if the palyer is within the detection radius
                detectedPlayerPosition = PlayerObj.position;//record the position of the last detected player
                StartCoroutine(SpawnEnemyAfterDelay(spawnDelay, detectedPlayerPosition));
            }
        }
    }

    IEnumerator SpawnEnemyAfterDelay(float delay, Vector3 playerPosition) {//coroutine to spawn an enemy after a delay
        yield return new WaitForSeconds(delay);
        Instantiate(EnemyPrefab, playerPosition, Quaternion.identity);
    }
    bool isPlayerWithinRadius() {//checks if the player is within the detection radius
        float distance = Vector3.Distance(PlayerObj.position, transform.position);
        return distance <= detectionRadius;

    }
}
