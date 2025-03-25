using System.Collections;
using UnityEngine;

public class GaeBolgHandler : MonoBehaviour
{
    [SerializeField] private GameObject projPrefab;
    [SerializeField] private float projAngleOffset;
    [SerializeField] private NearestEnemy nearestEnemy;
    [SerializeField] private Vector2 directionOffset;

    private DespawnProj _despawnHandler;
    private readonly Vector3 _projPositionOffset = new (0, 1.25f, 0);

    private void Start()
    {
        nearestEnemy = transform.GetComponentInChildren<NearestEnemy>();
        StartCoroutine(ShootEveryXSeconds());
    }

    private void LaunchProjectile()
    {
        // set the projectile direction/rotation/whatever
        Vector2 direction = nearestEnemy.FindNearestEnemy(transform.root).normalized + directionOffset;
        GameObject projectile = Instantiate(projPrefab, transform.root.position + _projPositionOffset, Quaternion.identity);
        
        projectile.layer = LayerMask.NameToLayer("Weapon");
        projectile.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + projAngleOffset);
        
        // despawn the proj when offscreen
        _despawnHandler = projectile.transform.GetComponent<DespawnProj>();
        _despawnHandler.projectile = projectile;
        
        // add collision + speed
        var clonedWeaponStats = projectile.GetComponent<WeaponDamageOnCollision>();
        clonedWeaponStats.damage = GameManager.Instance.gaeBolgDamage;
        
        // add the rigidbody
        Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.mass = 0f;
        rb.linearVelocity = direction * GameManager.Instance.gaeBolgLaunchForce;
        
        // add the collider
        projectile.AddComponent<PolygonCollider2D>();
        
        // ignore collision with the player
        int projectileLayer = projectile.layer;
        int playerLayer = LayerMask.NameToLayer("Player");

        Physics2D.IgnoreLayerCollision(projectileLayer, playerLayer, true);
        Physics2D.IgnoreLayerCollision(projectileLayer, projectileLayer, true);
    }
    
    // shoot every x seconds
    private IEnumerator ShootEveryXSeconds()
    {
        Debug.Log("am intrat fraiere");
        if (GameObject.FindWithTag("Enemy"))
        {
            LaunchProjectile();
        }
        yield return new WaitForSeconds(GameManager.Instance.gaeBolgShootingInterval);
        StartCoroutine(ShootEveryXSeconds());
    }
}
