using JetBrains.Annotations;
using Terra.AI.EnemyStates;
using Terra.Enums;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class AcidBulletSplashComponent : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        public void Init(Vector3 direction)
        {
            FacingDirection facingDirection = direction.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            
            int animHash = facingDirection == FacingDirection.Left ? AnimationHashes.DeathLeft : AnimationHashes.DeathRight;
            _animator.CrossFade(animHash, 0f);
        }

        [UsedImplicitly]
        public void OnAnimationEnd()
        {
            Destroy(gameObject);
        }
    }
}
