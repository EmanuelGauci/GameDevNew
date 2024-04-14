using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnablePicking : MonoBehaviour
{
    [SerializeField]private GameObject lockpicking;
    public bool isPicking = false;
    [SerializeField]private PinBehaviour pinBehaviour;
    [SerializeField]private RendererFeatureToggle rendererFeatureToggle;
    [SerializeField] private PlayerMovement playerMovement;
    

    void Update() {//if the player clicks on the object the lockpicking box is enabled
        if (Input.GetMouseButtonDown(0)) {//check for left mouse button click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//cast a ray from the mouse position into the scene
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {//check if the ray hits a collider
                if (hit.collider != null && hit.collider.gameObject == gameObject) {//check if the collider belongs to a 3D object
                    lockpicking.SetActive(true);//log a message to the console
                    isPicking = true;
                    rendererFeatureToggle.activateFeature = false;
                    playerMovement.isParalyzed = true;
                }
            }
            
        }
        if(pinBehaviour.finished == true) {
             this.gameObject.SetActive(false);
            playerMovement.isParalyzed = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            isPicking = false;
            lockpicking.SetActive(false);
            playerMovement.isParalyzed = false;
        }
        
    }

}
