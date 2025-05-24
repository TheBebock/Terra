using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.Combat
{
    public class PopupDamageManager : MonoBehaviourSingleton<PopupDamageManager>
    {
        [SerializeField] private TextMesh popupPrefab = default;
        [SerializeField] private float destroyTime = default;
        [SerializeField] private Vector3 popupOffset = default;
        [SerializeField] private Vector3 randomizerdPosition = default;

        [Foldout("Debug")][SerializeField, ReadOnly] private List<TextMesh> pooledPopups = new();
        [SerializeField] private int amountToPool = default;

        private void Start()
        {
            TextMesh popupTemp;

            for(int i = 0; i < amountToPool; i++)
            {
                popupTemp = Instantiate(popupPrefab);
                popupTemp.transform.parent = gameObject.transform;
                popupTemp.gameObject.SetActive(false);
                pooledPopups.Add(popupTemp);
            }
        }

        private TextMesh GetPooledPopup()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if (!pooledPopups[i].gameObject.activeInHierarchy) return pooledPopups[i];
            }
            return null;
        }

        public void UsePopup(Vector3 position, Quaternion rotation, float value)
        {
            TextMesh popup = GetPooledPopup();
            if(popup != null)
            {
                popup.transform.SetPositionAndRotation(position, rotation);
                SetupAdditionalPositionPopup(popup);

                popup.text = value.ToString();
                popup.gameObject.SetActive(true);

                StartCoroutine(ReturnToPoolCoroutine(popup));
            }
        }

        private void SetupAdditionalPositionPopup(TextMesh popup)
        {
            popup.transform.localPosition += popupOffset;
            popup.transform.localPosition += new Vector3(
                Random.Range(-randomizerdPosition.x, randomizerdPosition.x), 
                Random.Range(-randomizerdPosition.y, randomizerdPosition.y), 
                Random.Range(-randomizerdPosition.z, randomizerdPosition.z)
                );
        }

        private IEnumerator ReturnToPoolCoroutine(TextMesh popup)
        {
            yield return new WaitForSeconds(destroyTime);
            ReturnToPool(popup);
            
        }

        public void ReturnToPool(TextMesh popup)
        {
            popup.gameObject.SetActive(false);
        }
    }
}