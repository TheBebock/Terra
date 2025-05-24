using Terra.Core.Generics;
using Terra.Interfaces;
using TMPro;
using UnityEngine;

namespace Terra.UI
{
    public class PopupDamageCanvas : InGameMonobehaviour, IWithSetUp
    {
        public TMP_Text popupDamage = default;
        public Transform target = default;
        public Vector3 offset = Vector3.zero;

        private Quaternion _quaternionZero = Quaternion.Euler(0, 0, 0);

        

        public void SetPopupPosition()
        {
            transform.SetPositionAndRotation(target.position, _quaternionZero);
        }

        public void SetUp()
        {
            
        }

        public void TearDown()
        {
            
        }
    }
}