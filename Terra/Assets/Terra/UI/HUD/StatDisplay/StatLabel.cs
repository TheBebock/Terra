using TMPro;
using UIExtensionPackage.UISystem.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD.StatDisplay
{
    public class StatLabel : UIElement
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _main;
        [SerializeField] private TMP_Text _descriptionText;


        public void Init(Sprite icon, string main, string description)
        {
            _image.sprite = icon;
            SetMain(main);
            SetDescription(description);
        }
        public void SetMain(string text)
        {
            _main.SetText(text);
        }
        
        public void SetDescription(int value)
        {
            _descriptionText.SetText(value.ToString());
        }

        public void SetTextsColor(Color color)
        {
            _main.color = color;
            _descriptionText.color = color;
        }

        public void SetDescription(string text)
        {
            _descriptionText.SetText(text);
        }
    }
}
