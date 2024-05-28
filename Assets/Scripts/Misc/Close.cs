using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close : MonoBehaviour
{
    [SerializeField]private GameObject Settings;
    [SerializeField] private UIHandler uihandler;

    public void DestroySettings() {
        uihandler.Continue();   
    }
}
