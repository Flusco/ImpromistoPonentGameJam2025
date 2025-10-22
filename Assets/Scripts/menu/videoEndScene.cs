using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class videoEndScene : MonoBehaviour
{
    public string nextScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VideoPlayer video = GetComponent<VideoPlayer>();
        video.loopPointReached += onVideoEnd;
    }

    // Update is called once per frame
    void onVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }
}
