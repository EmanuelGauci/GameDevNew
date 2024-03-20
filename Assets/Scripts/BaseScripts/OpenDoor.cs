using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : Interactable{
    [SerializeField]private GameObject Example;//reference to the example gameobject
    private bool isCoroutineRunning = false;//flag to track if coroutine is running

    public override void Use() {//override method tohandle usage of opening the door
        if (!isCoroutineRunning) {//check if coroutine is not already running
            StartCoroutine(HideExample());//start coroutine to hide example object
        }
        Debug.Log("OpenDoor");//log that the door is being opened
    }
    private IEnumerator HideExample() {//coroutine to hide the example object after a delay
        Example.SetActive(true);//activate the example object
        yield return new WaitForSeconds(2f);//wait for 2 seconds
        Example.SetActive(false);//deactivate the example object after delay
        isCoroutineRunning = false;//set coroutine flag to false
    }
}
