using NaughtyAttributes;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UIExtensionPackage.UISystem.UI.Elements;
using UnityEngine;
using UnityEngine.UI;


namespace UIExtensionPackage.UISystem.UI.Samples.Templates
{
    /// <summary>
    /// Class for basic template of holdable ui element with slider.
    /// All logic is supposed to be done through the Inspector.
    /// </summary>
    /// <remarks>For more advanced logic, implement new class that inherits from <see cref="HoldableElementUI"/>.</remarks>
    internal sealed class HoldableWithSliderTemplate : HoldableElementUI, IAttachListenersUI
    {
        [Foldout("General")] [SerializeField] private Image sliderFillImage;

        public void AttachListeners()
        {
            OnHoldProgressedChanged.AddListener(HoldProgressedChanged);
        }

        public void DetachListeners()
        {
            OnHoldProgressedChanged.RemoveListener(HoldProgressedChanged);
        }
        
        private void HoldProgressedChanged(float progress)
        {
            sliderFillImage.fillAmount = progress;
        }
        
    }
}