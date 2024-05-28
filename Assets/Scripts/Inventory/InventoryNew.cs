using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class InventoryNew : MonoBehaviour {
    //3 collectables in levle 1 and 3 in level 2
    public GameObject[] Collectables = new GameObject[3]; //array to store the collecttables to be collected
    public GameObject[] CollectableInv = new GameObject[3]; //array to store the corresponding inventory slot image to enable
    public GameObject[] PicturePieces = new GameObject[3]; //array to store the picture pieces to be shown
    [SerializeField] private GameObject OpenedWallet;
    [SerializeField] private GameObject ClosedWallet;
    private bool invEnabled = false;


    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            invEnabled = !invEnabled;//toggle the inventory state
            if (invEnabled) {//call appropriate method based on the inventory state
                OpenInventory();
            } else {
                CloseInventory();
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        for (int i = 0; i < Collectables.Length; i++) {
            if (other.gameObject == Collectables[i]) {
                PicturePieces[i].SetActive(true); // Activate the corresponding picture piece
                Collectables[i].SetActive(false); // Deactivate the collected item
                CollectableInv[i].SetActive(true);
                Destroy(other.gameObject); // Optionally destroy the collected item
                break; // Exit the loop once the collectible is found
            }
        }
    }
    public void OpenInventory() {
        OpenedWallet.SetActive(true);
        ClosedWallet.SetActive(false);
    }
    public void CloseInventory() {
        ClosedWallet.SetActive(true);
        OpenedWallet.SetActive(false);
    }
}
    