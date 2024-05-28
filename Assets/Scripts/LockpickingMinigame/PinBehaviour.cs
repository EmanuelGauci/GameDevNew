using UnityEngine;
using System.Collections.Generic;

public class PinBehaviour : MonoBehaviour {
    [SerializeField] private GameObject PuzzleParent;
    [SerializeField] private GameObject Pick;
    [SerializeField] private List<GameObject> PinEndColliders;
    [SerializeField] private List<GameObject> CloseToPointColliders;
    [SerializeField] private List<GameObject> PickPointColliders;
    [SerializeField] private PickBehaviour pickBehaviour;
    [SerializeField] private GameObject lockpicking;
    private List<bool> isPinnedList = new List<bool>();
    private List<bool> successfullyPickedList = new List<bool>();
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject payloadObject;
    [SerializeField] private EnablePicking enablePicking;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AudioSource PinAudioSource;
    [SerializeField] private AudioClip pinCorrectSlot;
    [SerializeField] private AudioClip pinMoving;
    public bool finished = false;

    private void Start() {
        for (int i = 0; i < PinEndColliders.Count; i++) {
            isPinnedList.Add(false);
            successfullyPickedList.Add(false);
        }
    }

    public void Update() {
        bool allPinsSuccessfullyPicked = true;
        for (int i = 0; i < PinEndColliders.Count; i++) {
            if (CheckCollision(i) && !isPinnedList[i] && !Input.GetKey(KeyCode.Space)) {
                ParentPin(i);
                PlayPinMovingSound();
            }
            if (Input.GetKeyDown(KeyCode.Space) && isPinnedList[i]) {
                UnparentPin(i);
            }
            CheckIfCloseToPick(i);
            CheckIfPicked(i);
            if (!successfullyPickedList[i]) {
                allPinsSuccessfullyPicked = false;
            }
        }
        if (allPinsSuccessfullyPicked) {
            finished = true;
            PlayPinCorrectSlotSound();
            AccessPayload();
            lockpicking.SetActive(false);
            playerMovement.isParalyzed = false;
            foreach (GameObject pinEnd in PinEndColliders) {
                Destroy(pinEnd);
            }
            Destroy(PuzzleParent);
        }
    }


    private void AccessPayload() {
        if (payloadObject != null) {
            MonoBehaviour[] scripts = payloadObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts) {
                System.Reflection.MethodInfo method = script.GetType().GetMethod("RunPayload");
                if (method != null) {
                    method.Invoke(script, null);
                    return;
                }
            }
        }
    }

    public bool IsAnyPinned() {
        return isPinnedList.Contains(true);
    }

    private bool CheckCollision(int index) {
        BoxCollider2D pickEndBoxCollider = Pick.GetComponent<BoxCollider2D>();
        BoxCollider2D pinEndBoxCollider = PinEndColliders[index].GetComponent<BoxCollider2D>();

        if (pickEndBoxCollider != null && pinEndBoxCollider != null) {
            return pickEndBoxCollider.bounds.Intersects(pinEndBoxCollider.bounds);
        }
        return false;
    }

    private void ParentPin(int index) {
        if (!isPinnedList[index] && !successfullyPickedList[index]) {
            PinEndColliders[index].transform.parent = Pick.transform;
            isPinnedList[index] = true;
        }
    }

    private void UnparentPin(int index) {
        if (CheckIfPicked(index)) {
            successfullyPickedList[index] = true;
            PlayPinCorrectSlotSound(); // Play the sound when the pin is successfully picked
        }
        PinEndColliders[index].transform.parent = null;
        pickBehaviour.ResetPickPosition();
        isPinnedList[index] = false;
    }

    private void CheckIfCloseToPick(int index) {
        BoxCollider2D closeToPointBoxCollider = CloseToPointColliders[index].GetComponent<BoxCollider2D>();
        if (closeToPointBoxCollider != null && closeToPointBoxCollider.bounds.Intersects(PinEndColliders[index].GetComponent<BoxCollider2D>().bounds)) {
            //to be replaced with shaking
        }
    }

    private bool CheckIfPicked(int index) {
        BoxCollider2D pickPointBoxCollider = PickPointColliders[index].GetComponent<BoxCollider2D>();
        if (pickPointBoxCollider != null && pickPointBoxCollider.bounds.Intersects(PinEndColliders[index].GetComponent<BoxCollider2D>().bounds)) {
            return true;
        }
        return false;
    }

    private void PlayPinMovingSound() {
        if (PinAudioSource != null && pinMoving != null && !PinAudioSource.isPlaying) {
            PinAudioSource.clip = pinMoving;
            PinAudioSource.Play();
        }
    }

    private void PlayPinCorrectSlotSound() {
        if (PinAudioSource != null && pinCorrectSlot != null && !PinAudioSource.isPlaying) {
            PinAudioSource.clip = pinCorrectSlot;
            PinAudioSource.Play();
        }
    }
}
