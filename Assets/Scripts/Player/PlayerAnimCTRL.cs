using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCTRL : MonoBehaviour {
    [SerializeField] private Animator anim;
    private bool isPlayingAnimation = false;
    [SerializeField]private PlayerMovement playerMovement;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        UpdateAnimations();
    }

    private void UpdateAnimations() {
        if (!isPlayingAnimation) {
            bool isJumping = Input.GetKey(KeyCode.Space);
            anim.SetBool("isJumping", isJumping);
            bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);
            anim.SetBool("isWalking", isWalking);
            bool isIdle = !(isWalking || isJumping);
            anim.SetBool("isIdle", isIdle);

            bool isClimbing = playerMovement.isOnLadder && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A));
            anim.SetBool("isClimbing", isClimbing);
            
        }
    }

    public void SetAnimationPlaying(bool isPlaying) {
        isPlayingAnimation = isPlaying;
        if (!isPlaying) {
            // Reset animation states when animation stops playing
            anim.SetBool("isClimbing", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
        }
    }
}
