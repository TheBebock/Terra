using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.UI;
using TMPro;
using UnityEngine;

namespace Terra.Combat
{
    public class PopupDamageManager : MonoBehaviourSingleton<PopupDamageManager>
    {
        [SerializeField] private TextMesh popupPrefab = default;
        [SerializeField] private TMP_Text popupTMPPrefab = default;
        [SerializeField] private PopupDamageCanvas popupCanvasPrefab = default;
        [SerializeField] private float destroyTime = default;
        [SerializeField] private Vector3 popupOffset = default;
        [SerializeField] private Vector3 randomizerdPosition = default;

        [Foldout("Debug")][SerializeField, ReadOnly] private List<PopupDamageCanvas> pooledPopups = new();
        [SerializeField] private int amountToPool = default;

        private void Start()
        {
            //TextMesh popupTemp;
            PopupDamageCanvas popupCanvas;
            TMP_Text popupTemp;

            for(int i = 0; i < amountToPool; i++)
            {
                popupCanvas = Instantiate(popupCanvasPrefab);
                popupCanvas.transform.parent = gameObject.transform;
                popupCanvas.gameObject.SetActive(false);
                pooledPopups.Add(popupCanvas);
            }
        }

        private PopupDamageCanvas GetPooledPopup()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if (!pooledPopups[i].gameObject.activeInHierarchy) return pooledPopups[i];
            }
            return null;
        }

        public void UsePopup(Transform position, Quaternion rotation, float value)
        {
            PopupDamageCanvas popupCanvas = GetPooledPopup();
            TMP_Text popup = popupCanvas.popupDamage;
            if(popup != null)
            {
                popupCanvas.target = position;
                //popup.transform.SetPositionAndRotation(position, rotation);
                SetupAdditionalPositionPopup(popupCanvas);

                popup.text = ((int)value).ToString();
                popupCanvas.gameObject.SetActive(true);

                StartCoroutine(ReturnToPoolCoroutine(popupCanvas));
            }
        }

        private void SetupAdditionalPositionPopup(PopupDamageCanvas popup)
        {
            Vector3 addictionalPosition = Vector3.zero;

            addictionalPosition += popupOffset;
            addictionalPosition += new Vector3(
                Random.Range(-randomizerdPosition.x, randomizerdPosition.x), 
                Random.Range(-randomizerdPosition.y, randomizerdPosition.y), 
                Random.Range(-randomizerdPosition.z, randomizerdPosition.z)
                );

            popup.offset = addictionalPosition;
            popup.SetPopupPosition();
        }

        private IEnumerator ReturnToPoolCoroutine(PopupDamageCanvas popup)
        {
            yield return new WaitForSeconds(destroyTime);
            ReturnToPool(popup);
            
        }

        public void ReturnToPool(PopupDamageCanvas popup)
        {
            popup.target = default;
            popup.gameObject.SetActive(false);
        }
    }
}