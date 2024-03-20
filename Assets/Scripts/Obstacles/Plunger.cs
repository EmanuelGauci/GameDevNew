using UnityEngine;

public class Plunger : MonoBehaviour {
    private const float DetectionRadius = 5f;//radius for detecting the player game object
    private const float MoveDistance = 7.0f;//distance the plunger moves when activated
    private const float MoveSpeed = 30.0f;//speed at which the plunger moves
    private bool plungerMoving = false;//flag to track if the plunger is moving
    private bool moved = false;//flag to track if the plunger has moved at least once

    private Rigidbody rb;//reference to the rigidbody component

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;//set the rigidbody to kinematic mode to prevent physics interactions
    }

    void Update() {
        CheckIfPlayerInRange();//check if the player is within the detection radius
    }

    void CheckIfPlayerInRange() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius);//use physics overlapshere to check for coliders within the detection radius

        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Player") && !plungerMoving && !moved) {//check if the collider is tagged as "Player"
                StartCoroutine(MovePlunger());
                break;
            }
        }
    }

    System.Collections.IEnumerator MovePlunger() {
        plungerMoving = true;//set flag to indicate that the plunger is moving

        //store the initial position and the target position
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + transform.forward * MoveDistance;

       
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) { //move the plunger towards the target position
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime));//move the plunger
            yield return null;
        }

        
        while (Vector3.Distance(transform.position, initialPosition) > 0.01f) { //move the plunger back to its initial position
            rb.MovePosition(Vector3.MoveTowards(transform.position, initialPosition, MoveSpeed * Time.deltaTime));
            yield return null;
        }

        //set the flag to indicate that the plunger has moved
        plungerMoving = false;
        moved = true;
    }
}
