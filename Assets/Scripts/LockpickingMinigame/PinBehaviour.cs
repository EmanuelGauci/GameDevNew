using UnityEngine;
using System.Collections.Generic;

public class PinBehaviour : MonoBehaviour {
    [SerializeField] private GameObject Pick;//reference to pick game object
    [SerializeField] private List<GameObject> PinEndColliders;//list of colliders representing pin ends
    [SerializeField] private List<GameObject> CloseToPointColliders;//list of colliders used to detect proximity to pins
    [SerializeField] private List<GameObject> PickPointColliders;//list of colliders used to detect successful picking
    [SerializeField] private PickBehaviour pickBehaviour;//reference to the pick behaviour script
    [SerializeField] private GameObject lockpicking;//reference to the lockpicking game object

    //lists to track pin states and unsuccessful pick attempts
    private List<bool> isPinnedList = new List<bool>();
    private List<bool> successfullyPickedList = new List<bool>();
    private List<int> unsuccessfulPickingsList = new List<int>();

    [SerializeField] private GameManager gameManager;//reference to the game manager script
    [SerializeField] private GameObject payloadObject;//reference to the paylod game object
    [SerializeField] private EnablePicking enablePicking;//reference to the enable picking script
    [SerializeField] private PlayerMovement playerMovement;//reference to the playermovement script


    [SerializeField] private AudioSource PinAudioSource;//reference to the audio source component for pin
    [SerializeField] private AudioClip pinCorrectSlot;//sound clip for correct pin placement
    [SerializeField] private AudioClip pinMoving;//sound clip for pin movement


    public bool finished = false;//flag indicating if lock picking is finished

    private void Start() {
        for (int i = 0; i < PinEndColliders.Count; i++) {//initialize lists for each pin
            isPinnedList.Add(false);
            successfullyPickedList.Add(false);
            unsuccessfulPickingsList.Add(0);
        }
        playerMovement.isParalyzed = true;//disable player movement initially
    }

    public void Update() {
        bool allPinsSuccessfullyPicked = true;

        for (int i = 0; i < PinEndColliders.Count; i++) {
            if (CheckCollision(i) && !isPinnedList[i] && !Input.GetKey(KeyCode.Space)) {//check for collision with pack and handle pin movement
                ParentPin(i);
                PlayPinMovingSound();  // Play pin moving sound
            }
            if (Input.GetKeyDown(KeyCode.Space) && isPinnedList[i]) {//handle pin unpinning
                UnparentPin(i);
                //StopPinMovingSound();  // Play pin moving sound again
            }
            CheckIfCloseToPick(i);//check if player is close to pick
            CheckIfPicked(i);//check if pin is successfully picked
            if (!successfullyPickedList[i]) {//check if the current pin is not successfully picked
                allPinsSuccessfullyPicked = false;
            }
        }
        if (allPinsSuccessfullyPicked) {//if all pins are successfullyy picked do this
            finished = true;//set finished flag
            playerMovement.isParalyzed = false;//enable player movement
            PlayPinCorrectSlotSound();//play sound for correct pin placement
            AccessPayload();//access payload associated with lock
            lockpicking.SetActive(false);//disable lock picking game object
        }
    }

    private void AccessPayload() {//method to access payload attached to the lock
        if (payloadObject != null) {
            MonoBehaviour[] scripts = payloadObject.GetComponents<MonoBehaviour>();//get all scripts attached to the gameobject
            foreach (MonoBehaviour script in scripts) {//iterate through each script and check for the runpayload method
                System.Reflection.MethodInfo method = script.GetType().GetMethod("RunPayload");
                if (method != null) {//invoke the runpayload method dynamically
                    method.Invoke(script, null);
                    return;
                }
            }

        }
    }

    public bool IsAnyPinned() {
        return isPinnedList.Contains(true);//check if any pin is currently pinned
    }

    private bool CheckCollision(int index) {//jmethod to check collision between pick and pin
        BoxCollider2D pickEndBoxCollider = Pick.GetComponent<BoxCollider2D>();
        BoxCollider2D pinEndBoxCollider = PinEndColliders[index].GetComponent<BoxCollider2D>();

        if (pickEndBoxCollider != null && pinEndBoxCollider != null) {
            return pickEndBoxCollider.bounds.Intersects(pinEndBoxCollider.bounds);
        }
        return false;
    }

    private void ParentPin(int index) {//method to parent the pin to the pick
        if (!isPinnedList[index] && !successfullyPickedList[index]) {
            PinEndColliders[index].transform.parent = Pick.transform;
            isPinnedList[index] = true;
        }
    }

    private void UnparentPin(int index) {//method to unparent the pin from the pick
        if (CheckIfPicked(index)) {
            PlayPinCorrectSlotSound();//play sound for correct pin placement
            successfullyPickedList[index] = true;
            
        } else {
            unsuccessfulPickingsList[index]++;//counts how many unsuccessful pickings there are
            if (unsuccessfulPickingsList[index] % 5 == 0) {//check if 5 unsuccessful pickings have occured
                gameManager.PickAmount --;
            }
        }
        PinEndColliders[index].transform.parent = null;//unparent the pin
        pickBehaviour.ResetPickPosition();
        isPinnedList[index] = false;
    }

    private void CheckIfCloseToPick(int index) {//method to check if the player is close to the pick
        BoxCollider2D closeToPointBoxCollider = CloseToPointColliders[index].GetComponent<BoxCollider2D>();
        if (closeToPointBoxCollider != null && closeToPointBoxCollider.bounds.Intersects(PinEndColliders[index].GetComponent<BoxCollider2D>().bounds)) {
            //to be replaced with shaking
        }
    }

    private bool CheckIfPicked(int index) {//method to check if the pin is successfully picked
        BoxCollider2D pickPointBoxCollider = PickPointColliders[index].GetComponent<BoxCollider2D>();
        if (pickPointBoxCollider != null && pickPointBoxCollider.bounds.Intersects(PinEndColliders[index].GetComponent<BoxCollider2D>().bounds)) {
            return true;
        }
        return false;
    }

    private void PlayPinMovingSound() {//method to play the pin moving sound
        if (PinAudioSource != null && pinMoving != null && !PinAudioSource.isPlaying) {
            PinAudioSource.clip = pinMoving;
            PinAudioSource.Play();
        }
    }


    private void PlayPinCorrectSlotSound() {//jmethod to play the pin correct slot sound
        if (PinAudioSource != null && pinCorrectSlot != null && !PinAudioSource.isPlaying) {
            PinAudioSource.clip = pinCorrectSlot;
            PinAudioSource.Play();
        }
    }
}
