using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.Components
{
    [RequireComponent(typeof(Toggle))]
    internal class ToggleComponent : InGameMonobehaviour
    {
        [SerializeField, ReadOnly] private Toggle _toggle;

        private void Awake()
        {
            _toggle.onValueChanged.AddListener(OnClickPlaySound);
        }

        private void OnClickPlaySound(bool value)
        {
            if(value) AudioManager.Instance?.PlaySFX("UI_Interaction");
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_toggle == null) _toggle = GetComponent<Toggle>();
        }

#endif

    }
}
