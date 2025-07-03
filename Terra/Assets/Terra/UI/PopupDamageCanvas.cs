using Terra.Core.Generics;
using TMPro;
using UnityEngine;

namespace Terra.UI
{
    public class PopupDamageCanvas : InGameMonobehaviour
    {
        public TMP_Text popupDamage;
        public Transform target;
        public Vector3 offset = Vector3.zero;

        private Quaternion _quaternionZero = Quaternion.Euler(0, 0, 0);
        
        public void SetPopupPosition()
        {
            transform.SetPositionAndRotation(target.position, _quaternionZero);
        }
        
    }
}