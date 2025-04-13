using Terra.Core.Generics;
using UnityEngine;

namespace Terra.Combat
{
    public class PopupDamageManager:MonoBehaviourSingleton<PopupDamageManager>
    {
        [SerializeField] private TextMesh popupPrefab = default;
        [SerializeField] private float destroyTime = default;
        [SerializeField] private Vector3 popupOffset = default;
        [SerializeField] private Vector3 randomizerdPosition = default;

        public void CreatePopup(Vector3 position, Quaternion rotation, float value)
        {
            // TODO: Move to Pooling

            TextMesh newPopup = Instantiate(popupPrefab, position, rotation);
            newPopup.text = value.ToString();
            SetupPopup(newPopup);
        }

        private void SetupPopup(TextMesh popup)
        {
            popup.transform.localPosition += popupOffset;
            popup.transform.localPosition += new Vector3(
                Random.Range(-randomizerdPosition.x, randomizerdPosition.x), 
                Random.Range(-randomizerdPosition.y, randomizerdPosition.y), 
                Random.Range(-randomizerdPosition.z, randomizerdPosition.z)
                );

            Destroy(popup, destroyTime);
        }

        public void DestroyPopup()
        {

        }
    }
}