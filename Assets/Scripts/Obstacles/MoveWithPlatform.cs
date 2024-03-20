using UnityEngine;

public class MoveWithPlatform : MonoBehaviour {
    [SerializeField] private GameManager gameManager;

    private bool isPlayerParented = false;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            Rigidbody playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.interpolation = RigidbodyInterpolation.None;
            playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            other.transform.SetParent(transform);
            isPlayerParented = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            Rigidbody playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.interpolation = RigidbodyInterpolation.None;
            playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            other.transform.SetParent(null);
            isPlayerParented = false;
        }
    }

    private void Update() {
        if (isPlayerParented && gameManager.playerHealth <= 0) {
            Transform playerTransform = transform.Find("Player"); 
            if (playerTransform != null) {
                playerTransform.SetParent(null);
                isPlayerParented = false;
            }
        }
    }
}
