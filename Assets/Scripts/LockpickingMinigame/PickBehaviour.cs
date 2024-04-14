using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBehaviour : MonoBehaviour {
    [SerializeField] EnablePicking enablePicking; // Reference to enable picking script for determining if picking is enabled
    [SerializeField] PinBehaviour pinBehaviour; // Reference to pinbehaviour script for handling pin behaviour

    public float moveSpeed = 15f; // Speed at which the pick moves
    private Vector3 originalPosition; // Original position of the pick

    // Distances from original location in each direction
    public float distanceLeft = 2f;
    public float distanceRight = 2f;
    public float distanceUp = 2f;
    public float distanceDown = 2f;

    private void Start() {
        originalPosition = transform.position; // Store the original position at the start
    }

    private void Update() {
        if (enablePicking.isPicking == true) {// Check if picking is enabled
            MovePick(); // Move the pick
        }
    }

    private void MovePick() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (pinBehaviour.IsAnyPinned()) {// Check if any pin is pinned
            horizontalInput = 0f;// If pinned, only allow vertical movement
        }

        // Calculate new position based on input and speed
        Vector3 newPosition = transform.position + new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime;

        // Apply constraints to new position
        newPosition.x = Mathf.Clamp(newPosition.x, originalPosition.x - distanceLeft, originalPosition.x + distanceRight);
        newPosition.y = Mathf.Clamp(newPosition.y, originalPosition.y - distanceDown, originalPosition.y + distanceUp);

        // Move the pick
        transform.position = newPosition;
    }

    public void ResetPickPosition() {// Reset the pick's position to its original position
        transform.position = originalPosition;
    }
}
