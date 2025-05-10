using System;
using UnityEngine;

/// <summary>
/// Factory responsible for creating and initializing bullets.
/// </summary>
public class BulletFactory : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    /// <summary>
    /// Instantiates a projectile prefab and initializes it with the given data and direction.
    /// </summary>
    public Projectile CreateBullet(BulletData data, Vector3 position, Vector3 direction)
    {
        if (projectilePrefab == null)
            throw new InvalidOperationException("BulletFactory projectilePrefab is not assigned.");

        var go = Instantiate(projectilePrefab, position, Quaternion.identity);
        var proj = go.GetComponent<Projectile>();
        if (proj == null)
        {
            Destroy(go);
            throw new InvalidOperationException("Projectile prefab does not contain a Projectile component.");
        }

        proj.Initialize(data, direction);
        return proj;
    }
}