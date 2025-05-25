using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class CrateLanding : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Crate collided with: {collision.gameObject.name}");

            if (collision.gameObject.CompareTag("Flare"))
            {
                Debug.Log("Crate hit flare — destroying flare.");
                Destroy(collision.gameObject);
            }

            Destroy(this);
        }
    }
}