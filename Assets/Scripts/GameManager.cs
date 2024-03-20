using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    //responsible for handling global and stored variables such as health, game status, inventory status, etc...
    
    public int playerHealth;
    public int PickAmount;
    
    void Start()
    {
        playerHealth = 3;//set the player health to 3 every time the game re starts
        PickAmount = 4;
    }

}
