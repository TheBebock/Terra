using Cysharp.Threading.Tasks;
using DG.Tweening;
using Terra.Managers;
using UIExtensionPackage.UISystem.Core.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class ElevatorDoors : UIObject
    {
        [SerializeField] private RectTransform _leftDoorTransform;
        [SerializeField] private RectTransform _rightDoorTransform;
        [SerializeField] private Image _leftDoor;
        [SerializeField] private Image _rightDoor;
        [SerializeField] private AudioClip _doorsSound;
        [SerializeField] private float _doorsSize;
        [SerializeField] private float _animationDuration = 2f;
        [SerializeField] private AnimationCurve _openAnimationCurve;
        [FormerlySerializedAs("_animationCurve")][SerializeField] private AnimationCurve _closeAnimationCurve;

        private Sequence _doorSequence;

        private void Awake()
        {
            _doorsSize = _leftDoorTransform.rect.width;
        }

        public void SetDoorsClosed()
        {
            _leftDoorTransform.DOAnchorPosX(_doorsSize, 0.1f);
            _rightDoorTransform.DOAnchorPosX(-_doorsSize, 0.1f);
        }
        
        public async UniTask OpenDoors() 
        {
            if (_doorsSound)
            {
                AudioManager.Instance.PlaySFX(_doorsSound);
            }
            _doorSequence?.Kill();
            _doorSequence = DOTween.Sequence();

            _doorSequence.Append(_leftDoorTransform
                    .DOAnchorPosX(0, _animationDuration)
                    .SetEase(_openAnimationCurve)
            );
                
            _doorSequence.Join(_rightDoorTransform
                .DOAnchorPosX(0, _animationDuration)
                .SetEase(_openAnimationCurve)
            );
            
            await _doorSequence.AwaitForComplete(cancellationToken: destroyCancellationToken);
        }

        public async UniTask CloseDoors()
        {
            if (_doorsSound)
            {
                AudioManager.Instance.PlaySFX(_doorsSound);
            }
            
            _doorSequence?.Kill();
            _doorSequence = DOTween.Sequence();
            
            _doorSequence.Append(_leftDoorTransform
                .DOAnchorPosX(_doorsSize, _animationDuration)
                .SetEase(_closeAnimationCurve)
            );
            
            _doorSequence.Join(_rightDoorTransform
                    .DOAnchorPosX(-_doorsSize, _animationDuration)
                    .SetEase(_closeAnimationCurve)
            );

            await _doorSequence.AwaitForComplete(cancellationToken: destroyCancellationToken);
        }
    }
}
