using System;
using JetBrains.Annotations;
using Terra.AI.EnemyStates;
using Terra.Enums;
using Terra.Managers;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class AcidBulletSplashComponent : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] AudioClip _splashSFX;
        [SerializeField] AudioSource _audioSource;
        public void Init(Vector3 direction)
        {
            AudioManager.Instance?.PlaySFXAtSource(_splashSFX, _audioSource);
            FacingDirection facingDirection = direction.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            
            int animHash = facingDirection == FacingDirection.Left ? AnimationHashes.DeathLeft : AnimationHashes.DeathRight;
            _animator.CrossFade(animHash, 0f);
        }

        [UsedImplicitly]
        public void OnAnimationEnd()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if(!_audioSource) _audioSource = GetComponent<AudioSource>();
        }
#endif
    }
}
