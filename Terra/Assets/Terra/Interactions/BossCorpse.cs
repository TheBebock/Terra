using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Terra.AI.EnemyStates;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.GameStates;
using Terra.Managers;
using Terra.UI.HUD;
using UnityEngine;

namespace Terra.Interactions
{
    public class BossCorpse : InteractableBase
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _fadeInDurationOnInteract = 2.5f;
        private void Awake()
        {
            ChangeInteractibility(false);
        }
        
        public void Init(FacingDirection facingDirection)
        {
            int animHash = facingDirection == FacingDirection.Left ?
                AnimationHashes.DeathLeft : AnimationHashes.DeathRight;
            
            _animator.CrossFade(animHash, 0.1f);
        }

        [UsedImplicitly]
        public void OnAnimationEnded()
        {
            ChangeInteractibility(true);
        }
        public override void OnInteraction()
        {
            ChangeInteractibility(false);
            
            EventsAPI.Invoke<OnBossCorpseInteractedEvent>();
            _ = OnInteractAsync();
        }

        private async UniTaskVoid OnInteractAsync()
        {
            HUDManager.Instance.HideGameplayHUD();
            GameManager.Instance.SwitchToGameState<DefaultGameState>();
            await HUDManager.Instance.FadeInDarkScreen(_fadeInDurationOnInteract);
            AudioManager.Instance.StopMusic(true);
            await ScenesManager.Instance.LoadOutro();
        }

#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(!_animator) _animator = GetComponent<Animator>();
        }
#endif

    }
}
