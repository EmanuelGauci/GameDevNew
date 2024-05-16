using UnityEngine;

public class MoveWithPlatform : MonoBehaviour {
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerMovement playerMovement;

    private bool isPlayerParented = false;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            // Check if the player is grounded using the PlayerMovement script
            if (playerMovement.isGrounded) {
                Rigidbody playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
                playerRigidbody.interpolation = RigidbodyInterpolation.None;
                playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                other.transform.SetParent(transform);
                isPlayerParented = true;
            }
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            if (isPlayerParented) {
                Rigidbody playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
                playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                other.transform.SetParent(null);
                isPlayerParented = false;
            }
        }
    }

    private void Update() {
        // Check if the player is still grounded
        if (isPlayerParented && !playerMovement.isGrounded) {
            Transform playerTransform = transform.Find("Player");
            if (playerTransform != null) {
                playerTransform.SetParent(null);
                isPlayerParented = false;
            }
        }

        // Check if the player's health is zero and unparent them if needed.
        if (isPlayerParented && gameManager.playerHealth <= 0) {
            Transform playerTransform = transform.Find("Player");
            if (playerTransform != null) {
                playerTransform.SetParent(null);
                isPlayerParented = false;
            }
        }
    }
}
