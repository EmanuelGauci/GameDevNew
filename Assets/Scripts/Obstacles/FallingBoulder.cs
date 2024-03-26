using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;

public class FallingBoulder : MonoBehaviour {
    public float fallSpeed = 5f; // Speed of the falling object
    public Transform hitPoint; // Reference to the hit point transform
    public GameObject boulder; // Reference to the boulder game object
    public GameObject dialogueBox; // Reference to the dialogue box game object
    public TMP_Text textDisplay; // Reference to the TMP_Text component for displaying text
    public float displayInterval = 2f; // Interval for displaying text
    public TextAsset textFile; // Text file to display text from

    private bool isFalling = false; // Flag to indicate if the boulder is falling
    private bool hasActivated = false; // Flag to indicate if the boulder has been activated

    void Update() {
        if (isFalling) {
            // Move the boulder downwards
            boulder.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // Check if the boulder has reached the hit point
            if (boulder.transform.position.y <= hitPoint.position.y) {
                // Stop falling
                isFalling = false;
            }
        }

        // If the boulder has activated once, stop further updates
        if (hasActivated) {
            return;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !isFalling && !hasActivated) {
            // Start falling
            isFalling = true;

            // Set the activated flag to true
            hasActivated = true;

            // Display dialogue
            StartCoroutine(DisplayDialogue());
        }
    }

    IEnumerator DisplayDialogue() {
        if (dialogueBox != null && textDisplay != null && textFile != null) {
            dialogueBox.SetActive(true); // Show dialogue box
            string[] lines = textFile.text.Split('\n'); // Split text into lines
            foreach (string line in lines) {
                textDisplay.text = line; // Display current line
                yield return new WaitForSeconds(displayInterval); // Wait for display interval
            }
            dialogueBox.SetActive(false); // Hide dialogue box
            textDisplay.text = ""; // Clear text display
        } else {
            Debug.LogWarning("Dialogue box, text display component, or text file is not assigned.");
        }
    }
}
