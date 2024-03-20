using UnityEngine;

public class FanAudioManager : MonoBehaviour {
    [SerializeField] private AudioSource fanAudioSource;
    [SerializeField] private AudioClip fanSliceAudioClip;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            PlayFanSliceAudio();
        }
    }

    private void PlayFanSliceAudio() {
        if (fanAudioSource != null && fanSliceAudioClip != null) {
            fanAudioSource.clip = fanSliceAudioClip;
            fanAudioSource.Play();
        }
    }
}
