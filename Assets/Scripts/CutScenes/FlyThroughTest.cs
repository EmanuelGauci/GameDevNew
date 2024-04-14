using System.Collections;
using UnityEngine;
using Cinemachine;

public class FlyThroughTest : MonoBehaviour {

    public GameObject FlyThroughPoint;
    public CinemachineDollyCart dollyCart;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject Player;
    public PlayerMovement playerMovement;
    public Rigidbody playerRigidbody;
    private bool isCutsceneActive = false;

    void Start() {
        dollyCart.m_Speed = 0f;
    }

    void Update() {
        if (!isCutsceneActive) {
            CheckFlyThroughPoint();
        }
    }

    void CheckFlyThroughPoint() {
        Collider[] flyThroughColliders = FlyThroughPoint.GetComponents<Collider>();
        foreach (var flyThroughCollider in flyThroughColliders) {
            if (flyThroughCollider.bounds.Contains(Player.transform.position)) {
                FlyThroughPoint.SetActive(false);
                StartCoroutine(ActivateFlyThrough());
                break;
            }
        }
    }

    IEnumerator ActivateFlyThrough() {
        isCutsceneActive = true;
        playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        playerMovement.isParalyzed = true;

        dollyCart.m_Speed = 10f;
        virtualCamera.Priority = 100;
        yield return new WaitForSeconds(6);
        virtualCamera.Priority = 0;

        isCutsceneActive = false;
        playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePosition;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        playerMovement.isParalyzed = false;
    }
}
