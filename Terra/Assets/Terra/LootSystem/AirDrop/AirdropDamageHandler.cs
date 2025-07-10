using System;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Environment;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{

    public class AirdropDamageHandler : MonoBehaviour 
    {
        
        [SerializeField] private int _collisionDamage = 15;
        
        [SerializeField] private DamageableObject _object;
  

        [SerializeField, ReadOnly] private CrateLanding _crateLanding;
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_collisionDamage);
            }
            
            _crateLanding?.ObjectHitSomething();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if(!_crateLanding) _crateLanding = _object.GetComponent<CrateLanding>();
        }
#endif
    }
}
