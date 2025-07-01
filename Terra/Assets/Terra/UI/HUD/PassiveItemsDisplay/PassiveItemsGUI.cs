using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Extensions;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.Player;
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
        [SerializeField, Range(0, 100)] private int _opacityPercent = 80;
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
        }

        public void AttachListeners()
        {
            PlayerInventoryManager.Instance.OnPassiveItemAdded += OnPassiveItemAdded;
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
        private void OnOpacityChanged(int opacityPercent)
        {
            _opacityPercent = opacityPercent;
            ForceSetObjectOpacity(_opacityPercent);
        }
                
        public void ResetObjectOpacityToDefault() => ForceSetObjectOpacity(_opacityPercent);
        
        public void ForceSetObjectOpacity(int opacityPercent)
        {
            opacityPercent = Math.Clamp(opacityPercent, 0, 100);
            _canvasGroup.alpha = opacityPercent.ToFactor();
        }
        
        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
        
        public void DetachListeners()
        {
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
