using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AITypeThree : MonoBehaviour{
    public Transform player;
    public float fleeDistance = 10f;
    public float stopFleeDistance = 30f;
    public float exploreRadius = 5f;
    public float ventDetectionRadius = 2;
    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private bool isPlayerInSight = false;
    private bool isTouchingVent = false;
    public LayerMask ventLayer;


    private void Start(){
        agent = GetComponent<NavMeshAgent>();//get the navmesh agent component
    }
    private void Update(){
        isPlayerInSight = IsPlayerInSight();//turn the bool method into a bool variable
        
        //if player is in sight and is not touching the vent, perform actions
        if(isPlayerInSight && !isTouchingVent) {//check if the player is in sight and if it is touching the vent
            PlayerInSight();
            ExploreRandomly();
        } else {
            Destroy(gameObject);//destroy AI if conditions are not met
        }

        CheckForVent();//check for vents in AI's path
        IsTouchingVent();//check if AI is touching a vent
    }
    void PlayerInSight(){//things to do when the player is in sight
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance < fleeDistance) {//if player is within flee distance, flee from the player
            Vector3 fleeDirection = transform.position - player.position;
            Vector3 newPosition = transform.position + fleeDirection.normalized * fleeDistance;
            agent.SetDestination(newPosition);
        }else if(distance> stopFleeDistance) {//if player is outside stop flee distance, stop fleeing
            agent.ResetPath();
        }
    }
    void ExploreRandomly(){//explore randomly within a specified radius
        if(!agent.pathPending && agent.remainingDistance < 0.5f) {//if AI is not alrady navigating and reached the destination, choose a new random destination
            Vector2 randomPoint = Random.insideUnitCircle * exploreRadius;//generate randomp oint within circle
            targetPosition = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);//calculate target position
            agent.SetDestination(targetPosition);//set AI destination
        }
    }
    bool IsPlayerInSight(){//check if the player is in direct line of sight
        RaycastHit hit;
        if(Physics.Raycast(transform.position, player.position - transform.position, out hit)) {
            return hit.collider.gameObject == player.gameObject;
        }
        return false;
    }

    
    void CheckForVent(){//check for vents in AI's path
        RaycastHit ventDetect;//raycast forward to detect vents 
        if(Physics.Raycast(transform.position, transform.forward, out ventDetect, ventDetectionRadius, ventLayer)){
            agent.SetDestination(ventDetect.point);//set destination to vent
            isPlayerInSight = false;//reset player in sight flag
        }
    }

    void IsTouchingVent(){//check if AI is touching vent
        RaycastHit ventHit;//cast a short ray forward to detect vents
        if(Physics.Raycast(transform.position, transform.forward, out ventHit, 0.1f, ventLayer)){
            isTouchingVent = true;//set touching vent flag
        }
    }
        
}
