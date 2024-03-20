using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAtSetSpeed : MonoBehaviour {
    public float rotationSpeed = 25f; // Set the rotation speed in the Unity Editor

    void Update() {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Rotate the object around its up axis (Y axis)
    }
}
