using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBehaviour : MonoBehaviour {
    [SerializeField] EnablePicking enablePicking; //reference to enable picking script for determining if picking is enabled
    [SerializeField] PinBehaviour pinBehaviour; //reference to pinbehaviour script for handling pin behaviour

    public float moveSpeed = 15f;//speed at which the pick moves
    private Vector3 originalPosition;//original psition of the pick

    private void Start() {
        originalPosition = transform.position;//store the original position at the start
    }

    private void Update() {
        if (enablePicking.isPicking == true) {//check if picking is enabled
            MovePick();//move the pick
        }
    }

    private void MovePick() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (pinBehaviour.IsAnyPinned()) {//check if any pin is pinned
            horizontalInput = 0f;//if pinned, only allow vertical movement
        }

        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;//calculate movement direction and normalize it
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime * 100);//move the pick based on input and speed
    }

    public void ResetPickPosition() {//reset the pick's position toits original position
        transform.position = originalPosition;
    }
}
