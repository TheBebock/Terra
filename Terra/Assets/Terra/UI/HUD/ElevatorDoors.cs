using Cysharp.Threading.Tasks;
using DG.Tweening;
using Terra.Extensions;
using Terra.Managers;
using UIExtensionPackage.UISystem.Core.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class ElevatorDoors : UIObject
    {
        [SerializeField] private Image _leftDoor;
        [SerializeField] private Image _rightDoor;
        [SerializeField] private AudioClip _doorsSound;
        [SerializeField] private float _doorsOffset = 480;
        [SerializeField] private float _animationDuration = 2f;
        [SerializeField] private AnimationCurve _openAnimationCurve;
        [FormerlySerializedAs("_animationCurve")][SerializeField] private AnimationCurve _closeAnimationCurve;

        private Sequence _doorSequence;

        public void ForceSetDoorOpenPercentage(float openPercentage)
        {
            openPercentage = Mathf.Clamp(openPercentage, 0f, 100f);
            float newOffset = openPercentage.ToFactor() * _doorsOffset;
            
            _leftDoor.rectTransform.DOMoveX(-newOffset + _doorsOffset , 0.1f);
            _rightDoor.rectTransform.DOMoveX(newOffset + _doorsOffset*3, 0.1f);
        }
        
        public async UniTask OpenDoors() 
        {
            if (_doorsSound)
            {
                AudioManager.Instance.PlaySFX(_doorsSound);
            }
            _doorSequence?.Kill();
            _doorSequence = DOTween.Sequence();

            _doorSequence.Append(_leftDoor.rectTransform
                    .DOMoveX(-_doorsOffset, _animationDuration)
                    .SetEase(_openAnimationCurve)
            );
                
            _doorSequence.Join(_rightDoor.rectTransform
                .DOMoveX(_doorsOffset*5, _animationDuration)
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
            
            _doorSequence.Append(_leftDoor.rectTransform
                .DOMoveX(_doorsOffset, _animationDuration)
                .SetEase(_closeAnimationCurve)
            );
            
            _doorSequence.Join(_rightDoor.rectTransform
                    .DOMoveX(_doorsOffset*3, _animationDuration)
                    .SetEase(_closeAnimationCurve)
            );

            await _doorSequence.AwaitForComplete(cancellationToken: destroyCancellationToken);
        }
    }
}
