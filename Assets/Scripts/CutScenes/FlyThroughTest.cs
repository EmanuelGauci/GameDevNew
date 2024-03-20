using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FlyThroughTest : MonoBehaviour {
    
    public GameObject[] FlyThroughPoints;//array of gameobjects representing the points through which the flythrough will occur
    [SerializeField] private CinemachineDollyCart[] dollyCarts;//array of cinemachine dolly carts controlling the movement along the paths
    public CinemachineVirtualCamera[] virtualCameras;//array of cinemachine virtual cameras for different flythroughs
    public GameObject Player;//refernce to the player gameobject
    [SerializeField] private PlayerMovement playerMovement;//reference to the playermovement script
    [SerializeField]private Rigidbody playerRigidbody;//reference to the player rigidbody component
    private bool isCutsceneActive = false;//flag to check if a cutscene is currently active

    void Start() {
        foreach (var dollyCart in dollyCarts) {//initialize dolly cart speeds to 0 at the start
            dollyCart.m_Speed = 0f;
        }
    }

    void Update() {
        if (!isCutsceneActive) {//check for flythrough points only if a cutscene is not currently active
            CheckFlyThroughPoints();
        }
    }

    void CheckFlyThroughPoints() {//check if the player has reached any of the flythrough points
        for (int i = 0; i < FlyThroughPoints.Length; i++) {
            Collider[] flyThroughColliders = FlyThroughPoints[i].GetComponents<Collider>();
            foreach (var flyThroughCollider in flyThroughColliders) {
                if (flyThroughCollider.bounds.Contains(Player.transform.position)) {//disable the curent flythrough point and start the corresponding cutscene if in contact with player
                    FlyThroughPoints[i].SetActive(false);
                    StartCoroutine(ActivateFlyThroughMethod(i));
                    break;
                }
            }
        }
    }

    IEnumerator ActivateFlyThroughMethod(int index) {//coroutine to activate the flythrogh sequence for a specific point
        isCutsceneActive = true;
        //freeze player position and movement during the cutscene
        playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        playerMovement.isParalyzed = true;

        switch (index) {//execute the corresponding flythrough sequencebased on the index
            case 0:
                yield return StartCoroutine(FlyThrough1());
                break;
            case 1:
                yield return StartCoroutine(FlyThrough2());
                break;
            case 2:
                yield return StartCoroutine(FlyThrough3());
                break;
            default:
                yield break;
        }

        //restore player movement and constraints after the cutscene ends
        isCutsceneActive = false;
        playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePosition;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        playerMovement.isParalyzed = false;
    }

    IEnumerator FlyThrough1() {//coroutine for the first flythrough sequence
        //set the speed of the first dolly cart and adjust camera priorities
        dollyCarts[0].m_Speed = 10f;
        virtualCameras[0].Priority = 0;
        virtualCameras[1].Priority = 100;
        yield return new WaitForSeconds(10);
        //restore camera prioities after the sequence ends
        virtualCameras[0].Priority = 100;
        virtualCameras[1].Priority = 0;
    }

    IEnumerator FlyThrough2() {//coroutine for the second flythrough sequence
        //set the speed of the second dolly cart and  adjust camera priorities
        dollyCarts[1].m_Speed = 10f;
        virtualCameras[0].Priority = 0;
        virtualCameras[2].Priority = 100;
        yield return new WaitForSeconds(10);
        //restore camera priorities after the sequence ends
        virtualCameras[0].Priority = 100;
        virtualCameras[2].Priority = 0;
    }

    IEnumerator FlyThrough3() {//coroutine for the third flythrough sequence
        //set the speed of the third dolly cart and adjust camera priorities 
        dollyCarts[2].m_Speed = 10f;
        virtualCameras[0].Priority = 0;
        virtualCameras[3].Priority = 100;
        yield return new WaitForSeconds(15);
        //restore camera priorities after the sequence ends
        virtualCameras[0].Priority = 100;
        virtualCameras[3].Priority = 0;
    }
    
}
