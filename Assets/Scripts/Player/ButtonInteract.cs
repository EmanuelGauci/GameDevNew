using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour {
    [SerializeField] private float triggerDistance = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject EToInteractSprite;

    private bool isInRange = false;

    void Update() {
        // Check if player is within trigger distance
        Collider[] colliders = Physics.OverlapSphere(transform.position, triggerDistance, playerLayer);
        if (colliders.Length > 0 && !isInRange) {
            isInRange = true;
            SetEToInteractSpriteVisibility(true);
        } else if (colliders.Length == 0 && isInRange) {
            isInRange = false;
            SetEToInteractSpriteVisibility(false);
        }

        // Check for interaction when E key is pressed
        if (Input.GetKeyDown(KeyCode.E) && isInRange) {
            Debug.Log("Player interacted with object: " + gameObject.name);
            // Add your interaction logic here
        }
    }

    void SetEToInteractSpriteVisibility(bool isVisible) {
        EToInteractSprite.SetActive(isVisible);
    }
}
