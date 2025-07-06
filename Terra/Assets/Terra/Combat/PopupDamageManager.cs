using NaughtyAttributes;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Terra.Core.Generics;
using Terra.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Combat
{
    public class PopupDamageManager : MonoBehaviourSingleton<PopupDamageManager>
    {
        [FormerlySerializedAs("popupTMPPrefab")] [SerializeField] private TMP_Text _popupTMPPrefab;
        [FormerlySerializedAs("popupCanvasPrefab")] [SerializeField] private PopupDamageCanvas _popupCanvasPrefab;
        [FormerlySerializedAs("destroyTime")] [SerializeField] private float _destroyTime;
        [FormerlySerializedAs("popupOffset")] [SerializeField] private Vector3 _popupOffset;
        [FormerlySerializedAs("randomizerdPosition")] [SerializeField] private Vector3 _randomizerdPosition;


        [FormerlySerializedAs("pooledPopups")] [Foldout("Debug")][SerializeField, ReadOnly] private List<PopupDamageCanvas> _pooledPopups = new();
        [FormerlySerializedAs("amountToPool")] [SerializeField] private int _amountToPool;

        
        private void Start()
        {
            PopupDamageCanvas popupCanvas;

            for(int i = 0; i < _amountToPool; i++)
            {
                popupCanvas = Instantiate(_popupCanvasPrefab, gameObject.transform, true);
                popupCanvas.gameObject.SetActive(false);
                _pooledPopups.Add(popupCanvas);
            }
        }

        private PopupDamageCanvas GetPooledPopup()
        {
            for(int i = 0; i < _amountToPool; i++)
            {
                if (!_pooledPopups[i].gameObject.activeInHierarchy) return _pooledPopups[i];
            }
            return null;
        }

        public void UsePopup(Transform position, Quaternion rotation, float value, Color color)
        {
            PopupDamageCanvas popupCanvas = GetPooledPopup();
            TMP_Text popup = popupCanvas.popupDamage;
            if(popup != null)
            {
                popupCanvas.target = position;
                SetupAdditionalPositionPopup(popupCanvas);

                popup.text = System.Math.Round(value,2).ToString(CultureInfo.InvariantCulture);
                popup.color = color;
                popupCanvas.gameObject.SetActive(true);

                _ = ReturnToPoolCoroutine(popupCanvas);
            }
        }

        private void SetupAdditionalPositionPopup(PopupDamageCanvas popup)
        {
            Vector3 addictionalPosition = Vector3.zero;

            addictionalPosition += _popupOffset;
            addictionalPosition += new Vector3(
                Random.Range(-_randomizerdPosition.x, _randomizerdPosition.x), 
                Random.Range(-_randomizerdPosition.y, _randomizerdPosition.y), 
                Random.Range(-_randomizerdPosition.z, _randomizerdPosition.z)
                );

            popup.offset = addictionalPosition;
            popup.SetPopupPosition();
        }

        private async UniTaskVoid ReturnToPoolCoroutine(PopupDamageCanvas popup)
        {
            await UniTask.WaitForSeconds(_destroyTime, cancellationToken: CancellationToken);
            ReturnToPool(popup);
        }
        
        public void ReturnToPool(PopupDamageCanvas popup)
        {
            popup.target = null;
            popup.gameObject.SetActive(false);
        }
    }
}