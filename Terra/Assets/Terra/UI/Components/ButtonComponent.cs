using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.Components
{
    [RequireComponent(typeof(Button))]
    internal class ButtonComponent : InGameMonobehaviour
    {
        [SerializeField, ReadOnly] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(OnClickPlaySound);
        }

        private void OnClickPlaySound()
        {
            AudioManager.Instance?.PlaySFX("UI_Interaction");
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_button == null) _button = GetComponent<Button>();
        }

#endif

    }
}
