using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Player;
using TMPro;
using UnityEngine;

namespace Terra.UI.HUD
{
    public class AmmoGUI : InGameMonobehaviour, IAttachListeners
    {
        [SerializeField] private TMP_Text _currentAmmoText;
        [SerializeField] private TMP_Text _maxAmmoText;
        [SerializeField] private TMP_Text _tempValueText;

        

        [SerializeField] private float _moveFadeDuration = 1f;
        [SerializeField] private float _delayAfterGoldChange = 2f;
        [SerializeField] private float _punchDuration = 0.25f;
        [SerializeField] private Vector3 _punchUpScale = new(1.2f,1.2f,1.2f);

        [Foldout("Debug")][SerializeField] private int _currentAmmoAmount;
        [Foldout("Debug")][SerializeField] private int _maxAmmoAmount;
        [Foldout("Debug"),ReadOnly][SerializeField] private int _tempAmmoIncreaseAmount;
        private Vector3 _tempValueTextOriginalPosition;
        private Vector3 _tempValueTextOriginalScale;
        private CancellationTokenSource _animationCts;
        private CancellationTokenSource _linkedCts;
        private Tween _punchTween;
        private void Awake()
        {
            _tempValueTextOriginalPosition = _tempValueText.rectTransform.localPosition;
            _tempValueTextOriginalScale = _tempValueText.transform.localScale;

        }
        
        public void AttachListeners()
        {
            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnCurrentAmmoChanged += HandleCurrentAmmoChanged;
                PlayerInventoryManager.Instance.OnMaxAmmoChanged += HandleMaxAmmoAmountChanged;
            }
        }

        private void HandleMaxAmmoAmountChanged(int maxAmmo)
        {
            _maxAmmoAmount = maxAmmo;
            _maxAmmoText.text = _maxAmmoAmount.ToString();
        }
        
        private void HandleCurrentAmmoChanged(int currentAmmo)
        {
            int difference = currentAmmo - _currentAmmoAmount;
            _currentAmmoAmount = currentAmmo;

            if (difference <= 0)
            {
                _currentAmmoText.text = _currentAmmoAmount.ToString();
                return;
            }

            StopAnimation();
            _animationCts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_animationCts.Token, CancellationToken);
            
            _tempValueText.rectTransform.localPosition = _tempValueTextOriginalPosition;
            _tempValueText.DOFade(1, _punchDuration);
    
            _punchTween?.Kill();
            
            _punchTween = _tempValueText.transform.DOPunchScale(_punchUpScale, _punchDuration, elasticity:0)
                .OnKill(() => _tempValueText.transform.localScale = _tempValueTextOriginalScale)
                .OnComplete(() => _tempValueText.transform.localScale = _tempValueTextOriginalScale);
            
            _tempAmmoIncreaseAmount += difference;
            _tempValueText.SetText($"+{_tempAmmoIncreaseAmount}");
            _ = AmmoIncreaseAnimation(_linkedCts.Token);
        }
        
        private async UniTaskVoid AmmoIncreaseAnimation(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_delayAfterGoldChange, cancellationToken:token);
         
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_tempValueText.rectTransform.DOLocalMove(_currentAmmoText.rectTransform.localPosition,
                _moveFadeDuration)
                .SetEase(Ease.InSine));
            sequence.Join(_tempValueText.DOFade(0, _moveFadeDuration)
                .SetEase(Ease.InOutSine));
            
            try
            {
                await sequence.WithCancellation(token);
            }
            catch (OperationCanceledException)
            {
                _tempValueText.rectTransform.localPosition = _tempValueTextOriginalPosition;
                return;
            }

            if (!token.IsCancellationRequested)
            {
                _currentAmmoText.text = _currentAmmoAmount.ToString();
                _tempValueText.rectTransform.localPosition = _tempValueTextOriginalPosition;

                _tempAmmoIncreaseAmount = 0;
            }
        }

        private void StopAnimation()
        {
            if (_animationCts is { IsCancellationRequested: false })
            {
                _animationCts?.Cancel();
            }
            
            _linkedCts?.Dispose();
            _animationCts?.Dispose();
        }
        
        public void DetachListeners()
        {
            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnCurrentAmmoChanged -= HandleCurrentAmmoChanged;
                PlayerInventoryManager.Instance.OnMaxAmmoChanged -= HandleMaxAmmoAmountChanged;
            }
        }

        private void OnValidate()
        {
            _currentAmmoText.text = _currentAmmoAmount.ToString();
            _maxAmmoText.text = _maxAmmoAmount.ToString();
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            
            _linkedCts?.Dispose();
            _animationCts?.Dispose();
            DOTween.KillAll();
        }
    }
}
