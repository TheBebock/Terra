using LootSystem;
using UnityEngine;

namespace LootSystem
{
    public class KeyPressAction : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PerformAction();
            }
        }
        void PerformAction()
        {
            Debug.Log("Dropping item");
            LootBag lootBag = GetComponent<LootBag>();
            if (lootBag != null)
            {
                lootBag.InstantiateLoot(transform.position);
            }
            else
            {
                Debug.LogWarning("LootBag component not found on this object.");
            }
        }
    }
}