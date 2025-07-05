using System.Collections.Generic;
using NaughtyAttributes;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Itemization.Items;
using Terra.Player;
using Terra.Utils;
using UIExtensionPackage.Core.Interfaces;
using UIExtensionPackage.UISystem.Core.Base;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD.PassiveItemsDisplay
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PassiveItemsGUI : UIObject, IWithSetup, IAttachListeners, IShowHide
    {

        [SerializeField] private Image _iconPrefab;
        [SerializeField, Range(0, 1)] private float _opacityPercent = 0.3f;
        [Foldout("References")][SerializeField] private CanvasGroup _canvasGroup;
        [Foldout("References")][SerializeField] private Transform _itemsIconsContainer;
        [Foldout("Debug"), ReadOnly] private List<Image> _itemIcons = new();
        public void SetUp()
        {
            if (PlayerInventoryManager.Instance)
            {
                for (int i = 0; i < PlayerInventoryManager.Instance.GetPassiveItems.Count; i++)
                {
                    OnPassiveItemAdded(PlayerInventoryManager.Instance.GetPassiveItems[i]);
                }
            }
            
            _opacityPercent = GameSettings.DefaultItemsOpacity;
        }

        public void AttachListeners()
        {
            PlayerInventoryManager.Instance.OnPassiveItemAdded += OnPassiveItemAdded;
            EventsAPI.Register<ItemsOpacityChangedEvent>(OnOpacityChanged);
        }


        private void OnPassiveItemAdded(PassiveItem passiveItem)
        {
            Image newIcon = Instantiate(_iconPrefab, _itemsIconsContainer);
            newIcon.sprite = passiveItem.ItemIcon;
            _itemIcons.Add(newIcon);
        }
        public void Show()
        {
            _canvasGroup.alpha = 1;
        }
        
        private void OnOpacityChanged(ref ItemsOpacityChangedEvent opacity)
        {
            OnOpacityChanged(opacity.alfa);
        }
        
        private void OnOpacityChanged(float opacityPercent)
        {
            _opacityPercent = Mathf.Clamp01(opacityPercent);
            ForceSetObjectOpacity(_opacityPercent);
        }
                
        private void ForceSetObjectOpacity(float opacity)
        {
            _canvasGroup.alpha = opacity;
        }
                
        public void ResetObjectOpacityToDefault() => ForceSetObjectOpacity(_opacityPercent);
        

        
        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
        
        public void DetachListeners()
        {
            EventsAPI.Unregister<ItemsOpacityChangedEvent>(OnOpacityChanged);

            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnPassiveItemAdded -= OnPassiveItemAdded;
            }
        }
        public void TearDown()
        {
            //Noop
        }

        
        private void OnValidate()
        {
            if(!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();
            
            OnOpacityChanged(_opacityPercent);
        }
    }
}
