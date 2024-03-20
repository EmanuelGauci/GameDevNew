using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalDamage : MonoBehaviour{
    public int HealthReduction = 1;//amount of health reduced per seconds
    public GameManager gameManager;//reference to the gamemanager script
    private float contactTime = 0f;//time player has been in contact
    private bool isPlayerInContact = false;//flag indicating player is in contact

    // Update is called once per frame
    void Update() {
        RepeatOnProlongedContact();
    }

    private void RepeatOnProlongedContact() {//method to apply damage over time when player stays in contact
        if (isPlayerInContact) {
            contactTime += Time.deltaTime;

            if (contactTime >= 1f) {
                gameManager.playerHealth -= HealthReduction;
                contactTime = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInContact = true;
            gameManager.playerHealth -= HealthReduction; // Immediate damage upon entering
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInContact = false;
            contactTime = 0f;
        }
    }
}
