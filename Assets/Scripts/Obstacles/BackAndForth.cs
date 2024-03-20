using UnityEngine;

public class BackAndForth : MonoBehaviour {
    public GameObject startPoint;//reference to the start point game object
    public GameObject endPoint;//reference to the end point game object
    public float speed = 5f;//movement speed
    public float slowdownDistance = 2f;//distance at which the object starts slowing down
    public float accelerationDistance = 2f;//distance at which the object starts accelerating 

    private Transform target;//holds the current target position
    private Rigidbody rigidBody;//reference to the rigidbody component

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;//set the rigidbody to kinematic mode to prevent physics interactions
        target = endPoint.transform;//initially set the target to the end point
    }

    private void Update() {
        MoveTowards(target.position);//move the object towards the target position
    }

    private void MoveTowards(Vector3 targetPosition) {
        float step = CalculateMovementStep(targetPosition);//calculate the movement step based on the distance of the target

        if (step > 0.001f) {//check if the step is greater than a small treshold to avoid unnecessary calculations
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);//move the object towards the target position
        } else {
            target = (target == startPoint.transform) ? endPoint.transform : startPoint.transform;//if the step is very small, switch the target position
        }
    }

    private float CalculateMovementStep(Vector3 targetPosition) {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);//calculate the distance to the target position
        float accelerationFactor = Mathf.Clamp01(distanceToTarget / accelerationDistance);//calculate the acceleration factor based on the distance of the acceleration distance
        float slowdownFactor = Mathf.Clamp01(distanceToTarget / slowdownDistance);//calculate the slowdown factor based on the distance to the slowdown distance
        float currentSpeed = speed * Mathf.Lerp(accelerationFactor, slowdownFactor, slowdownFactor);//calculate the current speed based ont he acceleeration and slowdown factors
        return currentSpeed * Time.deltaTime;//calculate the movement step based on the current speed and the time delta
    }
    

    private void OnTriggerEnter(Collider other) {
        GameObject otherGameObject = other.gameObject;

        if (otherGameObject == startPoint) {
            target = endPoint.transform;//switch the target position to the end point
        } else if (otherGameObject == endPoint) {
            target = startPoint.transform;//switch the target position to the start point
        }
    }
}
