using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofAndWalls : MonoBehaviour
{
    [SerializeField]private bool RoofEnabled = true;
    [SerializeField]private GameObject RoofAndWallsGameobject;

    void Start(){
        if (RoofEnabled) {
            RoofAndWallsGameobject.SetActive(true);
        }
    }

}
