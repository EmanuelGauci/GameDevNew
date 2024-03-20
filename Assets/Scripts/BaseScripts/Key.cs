using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    [SerializeField] private GameObject Example;//reference to the example gameobject
    private bool isCoroutineRunning = false;//flag to track if coroutine is running

    public override void Use() {//override method to handle usage of the key
        if (!isCoroutineRunning) { //check if coroutine is not already rnning
            StartCoroutine(HideExample()); //start coroutine to hide example object
        }
        Debug.Log("FoundKey"); //log that the key has been found
    }
    private IEnumerator HideExample() { //coroutine to hide the example object after a delay
        Example.SetActive(true); //activate the example object
        yield return new WaitForSeconds(2f); //wait for 2 seconds
        Example.SetActive(false);//deactivate the example object after delay
        isCoroutineRunning = false;//set coroutine flag to false
    }
}
