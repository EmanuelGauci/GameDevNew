using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemHandler : MonoBehaviour {
    //will be changed to a different way to handle the inventory soon
    
    [SerializeField] private GameObject[] InventoryPoints;
    private bool[] itemPicked = new bool[4]; // Assuming 4 pickable items
    private string[] itemTags = { "Mug", "ashTray", "Glass", "Plate" };

    private void OnTriggerEnter(Collider other) {
        for (int i = 0; i < itemTags.Length; i++) {
            if (other.CompareTag(itemTags[i]) && !itemPicked[i]) {
                Debug.Log("Collision with item of tag: " + itemTags[i]);
                Sprite itemSprite = other.GetComponent<SpriteRenderer>().sprite;//get the sprite directly from the collided item
                int inventoryIndex = FindEmptyInventorySlot();//find the first available inventory point
                if (inventoryIndex != -1) {
                    Image inventoryImage = InventoryPoints[inventoryIndex].GetComponent<Image>();//replace the source image of the current active inventory point
                    if (inventoryImage != null) {
                        inventoryImage.sprite = itemSprite;
                        Color imageColor = inventoryImage.color;
                        imageColor.a = 1.0f; // Alpha value set to 100%
                        inventoryImage.color = imageColor;
                        itemPicked[i] = true;//makr the item as picked
                        Destroy(other.gameObject);//destroy the collided item gameobject
                    }
                }
                break; // Exit the loop once a match is found
            }
        }
    }
    private int FindEmptyInventorySlot() {//method to find the first available inventory slot
        for (int i = 0; i < InventoryPoints.Length; i++) {
            Image inventoryImage = InventoryPoints[i].GetComponent<Image>();
            if (inventoryImage != null && inventoryImage.sprite == null) {
                return i; // Found an empty slot
            }
        }
        return -1; // No empty slot found
    }
}
