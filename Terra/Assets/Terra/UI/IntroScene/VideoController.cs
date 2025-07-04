using System.Collections.Generic;
using Terra.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Terra.UI.IntroScene
{
    public class VideoController : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public Slider skipSlider;
        public float holdDuration = 2f;

        
        [SerializeField] private List<UnityEvent> _onVideoEndAction;
        private float _holdTimer;
        private bool _isHolding;
        private bool _invokeFlag;
        void Start()
        {
            for (int i = 0; i < _onVideoEndAction.Count; i++)
            {
                int index = i;
                videoPlayer.loopPointReached += _ => _onVideoEndAction[index].Invoke();
            }
            skipSlider.gameObject.SetActive(false);
            skipSlider.minValue = 0f;
            skipSlider.maxValue = holdDuration;
            skipSlider.value = 0f;
        }

        void Update()
        {
            if (Input.anyKey)
            {
                if (!_isHolding)
                {
                    _isHolding = true;
                    skipSlider.gameObject.SetActive(true);
                }

                _holdTimer += Time.deltaTime;
                skipSlider.value = _holdTimer;

                if (_holdTimer >= holdDuration && !_invokeFlag)
                {
                    _invokeFlag = true;
                    videoPlayer.Stop();
                    InvokeCallbacks();
                }
            }
            else
            {
                if (_isHolding)
                {
                    _isHolding = false;
                    _holdTimer = 0f;
                    skipSlider.value = 0f;
                    skipSlider.gameObject.SetActive(false);
                }
            }
        }


        private void InvokeCallbacks()
        {
            for (int i = 0; i < _onVideoEndAction.Count; i++)
            {
                 _onVideoEndAction[i].Invoke();
            }
        }
        public void LoadMainMenu()
        {
            ScenesManager.Instance?.LoadMainMenu();
        }
    }
}