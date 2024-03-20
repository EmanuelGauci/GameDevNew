using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITypeTwo : MonoBehaviour {
    public GameObject player;
    public GameObject enemyPrefab;
    public float detectionRadiusOne = 15f;
    public float detectionRadiusTwo = 20f;
    private Vector3 epicenter; 
    [SerializeField] private GameManager gameManager;
    private bool enemySpawned = false;
    private NavMeshAgent enemyNavMeshAgent;
    bool hitPlayer;
    [SerializeField] private AudioSource ai2AudioSource;
    [SerializeField] private AudioClip ai2AudioClip;

    private void Start(){
        epicenter = transform.position; // Set the epicenter position at the start
    }

    private void Update(){
        DetectionRadiusOne();//check detection radius one for player presence
        DetectionRadiusTwo();//check detection radius two for player presence
        DetectObjectBetween();//detect if there's any object between AI type two and the player
    }

    void DetectObjectBetween(){

        Vector3 direction = player.transform.position - epicenter;
        Vector3 offsetStartPoint = epicenter + direction.normalized * 0.1f;//adding a small offse to the starting point to avoid self intersection issues

        float sphereRadius = 0.2f; // Adjust this radius based on your game's scale
        float maxDistance = Vector3.Distance(epicenter, player.transform.position);

        if (Physics.SphereCast(offsetStartPoint, sphereRadius, direction, out RaycastHit hit, maxDistance)){
            if (hit.collider != null && hit.collider.CompareTag("Player")){
                hitPlayer = true;
            } else{
                hitPlayer = false;
            }
        }
    }



    void DetectionRadiusOne(){//create an overlap sphere to check the close point where the enemy sptarts spawning
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadiusOne);

        foreach (Collider collider in colliders){
            if (collider.gameObject == player && !enemySpawned && hitPlayer) {
                SpawnEnemy();//spawn enemy if player is detected within radius one
            }
        }

        if (enemySpawned){
            enemyNavMeshAgent.destination = player.transform.position;//set enemy destination towards player
        }
    }

    void DetectionRadiusTwo(){//create an overlap sphere to check the farthest point away the player can be before the enemy goes backa to its place
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadiusTwo);

        bool playerOutsideRadiusTwo = true;

        foreach (Collider collider in colliders){
            if (collider.gameObject == player){
                playerOutsideRadiusTwo = false;
                break;
            }
        }

        if (playerOutsideRadiusTwo && enemySpawned){//if player is outside of radius two and enemy is spawned, set enemy destination back to epicenter
            enemyNavMeshAgent.destination = epicenter;
        }
    }

    private void SpawnEnemy(){//instantiate the enemy prfab and starts moving the enemy towards the player
        enemyPrefab = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemySpawned = true;
        enemyNavMeshAgent = enemyPrefab.GetComponent<NavMeshAgent>();
        enemyNavMeshAgent.destination = player.transform.position;

        if (ai2AudioSource != null && ai2AudioClip != null){//play audio clip if audiosource and audiClip are assigned
            ai2AudioSource.clip = ai2AudioClip;
            ai2AudioSource.Play();
        }
    }

    private void FixedUpdate(){//check for small overlap between player and enemy
        Collider playerCollider = player.GetComponent<Collider>();
        Collider enemyCollider = enemyPrefab.GetComponent<Collider>();

        if (playerCollider.bounds.Intersects(enemyCollider.bounds)){//damage player if there's an overlap between the player and the enemy
            gameManager.playerHealth -= 3;
        }
    }
}
