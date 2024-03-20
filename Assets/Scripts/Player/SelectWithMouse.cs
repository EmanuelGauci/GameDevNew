using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWithMouse : MonoBehaviour {


    public PlayerMovement playermovement;

    void Update() {
        if (playermovement.selectwithmouse) {
            DetectWhenMouseIsOverInteractableObject();
        }
    }

    private void DetectWhenMouseIsOverInteractableObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Interactable")) {
            
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null && Input.GetMouseButton(0)) {
                    interactable.Use();
                } else {
                    //hovering but not pressing: provide visual indication
                }
        }else{
          //Debug.Log("NotTouching");
        }
    }
}
