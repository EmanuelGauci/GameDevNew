using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCTRL : MonoBehaviour {
    [SerializeField] private Animator anim;
    private bool isPlayingAnimation = false;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        UpdateAnimations();
    }

    private void UpdateAnimations() {
        if (!isPlayingAnimation) {
            // Set the isWalking parameter when either 'W' or 'S' key is pressed
            bool isWalkingForward = Input.GetKey(KeyCode.W);
            bool isWalkingBackward = Input.GetKey(KeyCode.S);
            anim.SetBool("isWalking", isWalkingForward || isWalkingBackward);

            // Set the isWalkingLeft and isWalkingRight parameters based on 'A' and 'D' keys
            bool isWalkingLeft = Input.GetKey(KeyCode.A);
            bool isWalkingRight = Input.GetKey(KeyCode.D);

            // Check for left shift key press and 'A' or 'D' keys to trigger dash animations
            bool isDashingLeft = Input.GetKey(KeyCode.LeftShift) && isWalkingLeft;
            bool isDashingRight = Input.GetKey(KeyCode.LeftShift) && isWalkingRight;

            // Set the isDashingLeft and isDashingRight parameters
            anim.SetBool("isDashingLeft", isDashingLeft);
            anim.SetBool("isDashingRight", isDashingRight);

            // Set the isWalkingLeft and isWalkingRight parameters based on 'A' and 'D' keys
            anim.SetBool("isWalkingLeft", isWalkingLeft && !isDashingLeft && !isWalkingRight);
            anim.SetBool("isWalkingRight", isWalkingRight && !isDashingRight && !isWalkingLeft);

            // Set the isIdle parameter based on whether any movement key is pressed
            bool isIdle = !(isWalkingForward || isWalkingLeft || isWalkingBackward || isWalkingRight);
            anim.SetBool("isIdle", isIdle && !isDashingLeft && !isDashingRight);
        }
    }

    public void SetAnimationPlaying(bool isPlaying) {//call this method from other parts of your code when an animation starts playing
        isPlayingAnimation = isPlaying;
    }
}
