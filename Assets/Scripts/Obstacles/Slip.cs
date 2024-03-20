using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slip : MonoBehaviour {
    public float originalSlipSpeed = 5f;
    private GameObject player;
    private float currentSlipSpeed;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");//find the player gameobject with the tag player
        currentSlipSpeed = originalSlipSpeed;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {//chekc if the entering collider has the tag "Plyaer and initiate slipping
            SlipPlayer(other);
        }
    }

    private void SlipPlayer(Collider playerCollider) {
        Vector3 playerDirection = playerCollider.GetComponent<Rigidbody>().velocity.normalized;
        playerDirection.y = 0;

        Vector3 slipMovement = playerDirection * currentSlipSpeed * Time.deltaTime;
        playerCollider.transform.position += slipMovement;

        DetectWallObstacle();
    }

    private void DetectWallObstacle() {
        Collider[] hitColliders = Physics.OverlapBox(player.transform.position, player.GetComponent<Collider>().bounds.extents, Quaternion.identity);

        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.CompareTag("WallObstacle")) {//if wall obstacle detected, snop slipping
                currentSlipSpeed = 0f;
                return;
            }
        }

        currentSlipSpeed = originalSlipSpeed;//if no obstacle is detected, reset the slip speed to the original speed
    }
}