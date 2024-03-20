using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour{ //this script defines the base functionality for interactable objects in the game
    public Rigidbody rb { get; private set; }//the rigidbody component attached to the interactable object
    
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();//getting the rigidbody component attached to this object
    }

    public abstract void Use();//abstract method for defining how the interactable object is used
}
