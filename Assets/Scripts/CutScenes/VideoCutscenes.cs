using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour {
    [SerializeField] private GameObject videoPlayerGameObject;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private RenderTexture renderTexture;


    public void StartCutscene() {
        // Make sure all required components are assigned
        if (videoPlayer == null || rawImage == null || videoClip == null || renderTexture == null) {
            Debug.LogError("Please assign all components in the inspector.");
            return;
        }
        videoPlayer.clip = videoClip;//assign video clip to the video player
        videoPlayer.targetTexture = renderTexture;//assign render texture to the target texture of the video player
        rawImage.texture = renderTexture;//assign render texture to the raw image texture
        videoPlayer.loopPointReached += OnVideoFinished;//add a listener for the videoplayer to call a method when the video is finished
        videoPlayerGameObject.SetActive(true);

        videoPlayer.Play();//play the video
    }
    
    private System.Action onVideoFinishedAction;

    public void SetOnVideoFinishedAction(System.Action action) {
        onVideoFinishedAction = action;
    }

    private void OnVideoFinished(VideoPlayer vp) {
        videoPlayerGameObject.SetActive(false);//deactivate the videoPlayer GameObject when the video is finished
        videoPlayer.loopPointReached -= OnVideoFinished;//remove the listener to avoid potential issues
        onVideoFinishedAction?.Invoke();//perform the custom action if set
    }
}
