using JetBrains.Annotations;
using Terra.AI.EnemyStates;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using UnityEngine;

namespace Terra.Interactions
{
    public class BossCorpse : InteractableBase
    {
        [SerializeField] private Animator _animator;
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
