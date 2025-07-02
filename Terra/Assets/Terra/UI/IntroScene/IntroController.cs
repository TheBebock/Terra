using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Slider skipSlider;
    public float holdDuration = 2f;
    public string nextScene = "MainMenuScene";

    private float holdTimer = 0f;
    private bool isHolding = false;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        skipSlider.gameObject.SetActive(false);
        skipSlider.maxValue = holdDuration;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isHolding)
            {
                isHolding = true;
                skipSlider.gameObject.SetActive(true);
            }

            holdTimer += Time.deltaTime;
            skipSlider.value = holdTimer;

            if (holdTimer >= holdDuration)
            {
                SkipToMenu();
            }
        }
        else
        {
            if (isHolding)
            {
                isHolding = false;
                holdTimer = 0f;
                skipSlider.value = 0f;
                skipSlider.gameObject.SetActive(false);
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SkipToMenu();
    }

    void SkipToMenu()
    {
        SceneManager.LoadScene(nextScene);
    }
}