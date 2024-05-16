using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndCollider : MonoBehaviour {
    public Collider activationCollider;
    public Collider endCollider;
    public GameObject playerGameObject;
    [SerializeField]private GameObject FallingFloor;
    [SerializeField]private float faSpeed = 30f;
    [SerializeField]private float faDist = 50f;

    private void Start() {

    }

    private void Update() {
        
        if (activationCollider.bounds.Contains(playerGameObject.transform.position)) {
            Debug.Log("Player detected by activation collider.");
            FallingLogic();
        }
        if (endCollider.bounds.Contains(playerGameObject.transform.position)) {
            Debug.Log("Player detected by end collider.");
            SceneManager.LoadScene("EndScreen");
        }
    }
    private void FallingLogic() {
        FallingFloor.transform.Translate(Vector3.down * faDist * Time.deltaTime * faSpeed);
    }
}
