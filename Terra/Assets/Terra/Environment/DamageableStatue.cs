using Terra.AI.EnemyStates;
using Terra.Enums;
using Terra.Managers;
using Terra.Utils;
using UnityEngine;

namespace Terra.Environment
{
    public class DamageableStatue : DamageableObject
    {
        [SerializeField] private Texture _leftEmissiveTexture;
        [SerializeField] private Texture _rightEmissiveTexture;
        [SerializeField] private Sprite _leftSprite;
        [SerializeField] private Sprite _rightSprite;
        
        [SerializeField] private Sprite _destroyedLeftSprite;
        [SerializeField] private Sprite _destroyedRightSprite;
        [SerializeField] private int _amountOfCrystalsToSpawn = 4;
        [SerializeField] private float _maxSpawnRadius = 2f;
        [SerializeField] LayerMask _obstacleMask;
        int _targetLayerIndex;
        FacingDirection _facingDirection;
        
        public void Init(FacingDirection facingDirection, AudioClip deathSFX)
        {
            _facingDirection = facingDirection;
            switch (facingDirection)
            {
                case FacingDirection.Left:
                    VFXcontroller.SetMaterialEmissiveMap(_leftEmissiveTexture);
                    VFXcontroller.SetModelSprite(_leftSprite);
                    _propAnimator.CrossFade(AnimationHashes.DeathLeft, 0.1f);
                    break;
                case FacingDirection.Right:
                    VFXcontroller.SetMaterialEmissiveMap(_rightEmissiveTexture);
                    VFXcontroller.SetModelSprite(_rightSprite);
                    _propAnimator.CrossFade(AnimationHashes.DeathRight, 0.1f);
                    break;
            }
            
            // AudioManager.Instance?.PlaySFXAtSource(deathSFX, _audioSource);
        }
        
        protected override void OnDeath()
        {
            base.OnDeath();

            if (_facingDirection == FacingDirection.Left)
            {
                VFXcontroller.SetModelSprite(_destroyedLeftSprite);
            }
            else
            {
                VFXcontroller.SetModelSprite(_destroyedRightSprite);
            }
            _targetLayerIndex = LayerMask.NameToLayer("Ground");
            for (int i = 0; i < _amountOfCrystalsToSpawn; i++)
            {
                Vector3 spawnPos = RaycastExtension.GetPositionInCircle(transform.position, 
                    _maxSpawnRadius, _obstacleMask, _targetLayerIndex);
                LootManager.Instance.SpawnCrystalPickup(spawnPos);

            }
        }
    }
}
