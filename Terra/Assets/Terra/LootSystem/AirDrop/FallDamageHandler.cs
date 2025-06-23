using Terra.Combat;
using Terra.Environment;
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
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_collisionDamage);
            }
            
            _crateLanding.ObjectHitSomething();
        }
    }
}
