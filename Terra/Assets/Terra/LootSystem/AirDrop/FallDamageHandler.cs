using System;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Environment;
using Terra.Extensions;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{

    public class AirdropDamageHandler : MonoBehaviour 
    {
        
        [SerializeField] private int _collisionDamage = 15;
        
        [SerializeField] private DamageableObject _object;
  

        private CrateLanding _crateLanding;
        private void Awake()
        {
            _crateLanding = _object.GetComponent<CrateLanding>();
        }
        
        void OnTriggerEnter(Collider other)
        {
            IDamageable otherDamageable = other.GetComponent<IDamageable>();
            if (otherDamageable != null)
            {
                otherDamageable.TakeDamage(_collisionDamage);
            }
            
            _crateLanding.ObjectHitSomething();
        }
    }
}
