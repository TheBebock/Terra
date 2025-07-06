using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Managers;
using TMPro;
using UnityEngine;

namespace Terra.UI.HUD
{
    public class CrystalPickupUI : InGameMonobehaviour, IAttachListeners, IWithSetUp
    {
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private TMP_Text _tempValueText;

        

        [SerializeField] private float _moveFadeDuration = 1f;
        [SerializeField] private float _delayAfterGoldChange = 2f;
        [SerializeField] private float _punchDuration = 0.25f;
        [SerializeField] private Vector3 _punchUpScale = new(1.2f,1.2f,1.2f);

        private int _currentCounterAmount;
        private int _tempCrystalIncreaseAmount;
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
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnGoldChanged += HandleGoldChanged;
            }
        }
        
        public void SetUp()
        {
            if(!EconomyManager.Instance) return;

            _currentCounterAmount = EconomyManager.Instance.CurrentGold;
            _counterText.text = _currentCounterAmount.ToString();
        }
        
        private void HandleGoldChanged(int currentGold)
        {
            int difference = currentGold - _currentCounterAmount;
            _currentCounterAmount = currentGold;

            if (difference < 0)
            {
                _counterText.text = _currentCounterAmount.ToString();
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
            
            _tempCrystalIncreaseAmount += difference;
            _tempValueText.SetText($"+{_tempCrystalIncreaseAmount}");
            _ = GoldIncreaseAnimation(_linkedCts.Token);
        }
        
        private async UniTaskVoid GoldIncreaseAnimation(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_delayAfterGoldChange, cancellationToken:token);
         
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_tempValueText.rectTransform.DOLocalMove(_counterText.rectTransform.localPosition,
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
                _counterText.text = _currentCounterAmount.ToString();
                _tempValueText.rectTransform.localPosition = _tempValueTextOriginalPosition;

                _tempCrystalIncreaseAmount = 0;
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
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnGoldChanged -= HandleGoldChanged;
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            
            _linkedCts?.Dispose();
            _animationCts?.Dispose();
            DOTween.KillAll();
        }



        public void TearDown()
        {
            
        }
    }
}
