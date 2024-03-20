using System.Collections;
using UnityEngine;

public class TimeoutPlatform : MonoBehaviour {
    public GameObject Player;//reference to the player gameobject
    public float shakeDuration = 0.5f;//duration of the shaking animation
    [System.NonSerialized] public float shakeMagnitude = 0.5f;//magnitude of the shaking effect
    private bool isShaking = false;//flag indicating if the platform is currently shaking
    [SerializeField] private float TimeOut = 3f;//time until the platform self destructs
    [SerializeField] GameManager gameManager;//reference to the game manager


    private void TimerElapsed() {//function called when the timer elapses
        this.gameObject.SetActive(false); // Destroy the platform when the timer elapses
    }

    private void OnCollisionEnter(Collision collision) {//function called upon colision with another object
        if (collision.gameObject == Player && !isShaking) {
            StartCoroutine(ShakePlatform());//initiate shaking animation
            Invoke("TimerElapsed", TimeOut);//schedule platform destruction
        }
    }

    private IEnumerator ShakePlatform() {//coroutine to animate the platform shaking
        isShaking = true;
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;
        while (elapsed < shakeDuration) {
            float x = originalPosition.x + Random.Range(0, shakeMagnitude);
            transform.position = new Vector3(x, originalPosition.y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;//reset platform position after shaking
        isShaking = false;
        TimerElapsed();//call function to destroy the platform after shaking
    }

    /*
     * code summary:
     * - this controls a platform's behaviour in response to collisions with a specified player GameObject. 
     * - When a collision occurs, the platform initiates a shaking animation and sets a timer to destroy itself after 2 seconds. 
     * - The shaking effect is achieved through a coroutine, changing the platform's position randomly within a specified range for a set duration. 
     * - After the shaking, the platform is destroyed, removing it from the scene.
     * - The script ensures that the platform only shakes and destroys itself once, preventing continuous shaking. 
     * - The implementation uses Unity's MonoBehaviour and coroutine functionalities for controlled animations and actions.
    */

}
