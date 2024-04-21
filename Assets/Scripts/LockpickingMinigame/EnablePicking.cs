using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnablePicking : MonoBehaviour {
    [SerializeField] private GameObject lockpicking;
    public bool isPicking = false;
    [SerializeField] private PinBehaviour pinBehaviour;
    [SerializeField] private RendererFeatureToggle rendererFeatureToggle;
    [SerializeField] private PlayerMovement playerMovement;
    private RigidbodyConstraints originalConstraints; // Store original Rigidbody constraints

    [SerializeField] private float TrigDistance = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject EToInteractSprite;
    private bool isInRange = false;

    void Start() {
        if (playerMovement != null && playerMovement.GetComponent<Rigidbody>() != null) {//store the original rigidbody constarints
            originalConstraints = playerMovement.GetComponent<Rigidbody>().constraints;
        }
    }

    void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, TrigDistance, playerLayer);
        if (colliders.Length > 0 && !isInRange) {
            isInRange = true;
            SetEToInteractSpriteVisibility(true);
        } else if (colliders.Length == 0 && isInRange) {
            isInRange = false;
            SetEToInteractSpriteVisibility(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && isInRange) {
            lockpicking.SetActive(true);
            isPicking = true;
            rendererFeatureToggle.activateFeature = false;
            playerMovement.isParalyzed = true;
            if (playerMovement.GetComponent<Rigidbody>() != null) {//freeze the player's rigidbody
                playerMovement.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        if (pinBehaviour.finished == true) {
            this.gameObject.SetActive(false);
            playerMovement.isParalyzed = false;
            if (playerMovement.GetComponent<Rigidbody>() != null) {//restore the player's rigidbody
                playerMovement.GetComponent<Rigidbody>().constraints = originalConstraints;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPicking = false;
            lockpicking.SetActive(false);
            playerMovement.isParalyzed = false;

            // Restore original Rigidbody constraints
            if (playerMovement.GetComponent<Rigidbody>() != null) {
                playerMovement.GetComponent<Rigidbody>().constraints = originalConstraints;
            }
        }

    }

    void SetEToInteractSpriteVisibility(bool isVisible) {
        EToInteractSprite.SetActive(isVisible);
    }
}
