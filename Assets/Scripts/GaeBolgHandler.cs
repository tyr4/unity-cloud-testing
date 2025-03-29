using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

[BurstCompile]
public struct FindNearestEnemyJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<Vector2> enemyPositions;
    public Vector2 playerPosition;

    public NativeArray<float> distances;  // Stores the distances for comparison

    public void Execute(int index)
    {
        distances[index] = Vector2.Distance(playerPosition, enemyPositions[index]);
    }
}

public class GaeBolgHandler : MonoBehaviour
{
    [SerializeField] private GameObject projPrefab;
    [SerializeField] private float projAngleOffset;
    [SerializeField] private NearestEnemy nearestEnemy;
    [SerializeField] private Vector2 directionOffset;

    private readonly Vector3 _projPositionOffset = new(0, 1.25f, 0);
    private DespawnProj _despawnHandler;

    private void Start()
    {
        nearestEnemy = transform.GetComponentInChildren<NearestEnemy>();
        StartCoroutine(ShootEveryXSeconds());
    }

    private void LaunchProjectile()
    {
        Vector2 direction = nearestEnemy.FindNearestEnemy(transform.root) + directionOffset;
        GameObject projectile = ProjectilePool.Instance.GetProjectile("Gae Bolg Projectile");
        if (!projectile) return; // Exit if the projectile type doesn't exist

        projectile.SetActive(true);
        projectile.transform.position = transform.root.position + _projPositionOffset;
        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + projAngleOffset);

        // Reset Rigidbody Physics
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.simulated = false; // Temporarily disable physics calculations
        rb.linearVelocity = Vector2.zero; // Reset velocity
        rb.angularVelocity = 0f; // Reset angular velocity
//
        // Apply movement using force instead of direct velocity assignment
        rb.simulated = true; // Re-enable physics
        rb.AddForce(direction * GameManager.Instance.gaeBolgLaunchForce, ForceMode2D.Impulse);

        // Set damage
        var clonedWeaponStats = projectile.GetComponent<WeaponDamageOnCollision>();
        clonedWeaponStats.damage = GameManager.Instance.gaeBolgDamage;

        int projectileLayer = projectile.layer;
        int playerLayer = LayerMask.NameToLayer("Player");

        _despawnHandler = projectile.GetComponent<DespawnProj>();
        _despawnHandler.projectile = projectile;

        Physics2D.IgnoreLayerCollision(projectileLayer, playerLayer, true);
        Physics2D.IgnoreLayerCollision(projectileLayer, projectileLayer, true);
    }

    private IEnumerator ShootEveryXSeconds()
    {
        // if (GameObject.FindWithTag("Enemy"))
        // {
            LaunchProjectile();
        // }
        yield return new WaitForSeconds(GameManager.Instance.gaeBolgShootingInterval);
        StartCoroutine(ShootEveryXSeconds());
    }
}
