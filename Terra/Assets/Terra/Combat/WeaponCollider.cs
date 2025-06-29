using Terra.Core.Generics;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Managers;
using UnityEngine;

namespace Terra.Combat
{
    public class WeaponCollider : MonoBehaviour
    {
        private Entity _entity;
        private MeleeWeaponData _weaponData;
        
        public void Init(Entity entity, MeleeWeaponData weaponData)
        {
            _entity = entity;
            _weaponData = weaponData;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                CombatManager.Instance.PerformAttack(_entity, damageable, _weaponData.effects);
            }
        }
    }
}