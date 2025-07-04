using UnityEngine;

namespace Terra.Interactions
{
    public class InteractionIcon : MonoBehaviour
    {
        [SerializeField] private bool _levitateUpDown;
        [SerializeField] private Animator _animator;
        private void Awake()
        {
            if(!_levitateUpDown) _animator.enabled = false;
        }

#if UNITY_EDITOR
        private  void OnValidate()
        {
            if(!_animator) _animator = GetComponent<Animator>();
        }
#endif
        
    }
}
