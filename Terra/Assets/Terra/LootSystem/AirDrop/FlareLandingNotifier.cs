using System;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class FlareLandingNotifier : MonoBehaviour
    {
        public Action OnLanded;

        private bool _hasLanded = false;
        public bool HasLanded => _hasLanded;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Flare collided with: {collision.gameObject.name}, layer: {collision.gameObject.layer}");

            if (!_hasLanded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Flare landed on ground.");
                _hasLanded = true;
                OnLanded?.Invoke();
            }
        }
    }
}