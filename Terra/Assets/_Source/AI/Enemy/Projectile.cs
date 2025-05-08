using UnityEngine;
using Terra.Interfaces;

/// <summary>
/// Represents a projectile that deals damage on contact and self-destructs after a set time.
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 1f;       // Amount of damage the projectile inflicts
    [SerializeField] private float lifetime = 5f;     // Time in seconds before the projectile is automatically destroyed

    /// <summary>
    /// Called on the first frame the projectile exists.
    /// Starts a timer to destroy the projectile after its lifetime expires.
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Called when the projectile collides with another trigger collider.
    /// If the object implements IDamagable, apply damage and destroy the projectile.
    /// </summary>
    /// <param name="other">The collider that this projectile has entered.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamagable>(out var target))
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}