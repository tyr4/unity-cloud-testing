using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [SerializeField] private List<GameObject> projectilePrefabs; // List of different projectiles
    private Dictionary<string, Queue<GameObject>> projectilePools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;

        // Initialize pool for each projectile type
        foreach (GameObject prefab in projectilePrefabs)
        {
            projectilePools[prefab.name] = new Queue<GameObject>();
            PreloadProjectiles(prefab, 10);  // Preload 10 projectiles per type
        }
    }

    private void PreloadProjectiles(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject proj = Instantiate(prefab);
            proj.SetActive(false);
            projectilePools[prefab.name].Enqueue(proj);
        }
    }

    public GameObject GetProjectile(string projectileType)
    {
        if (!projectilePools.ContainsKey(projectileType))
        {
            Debug.LogError($"Projectile type {projectileType} not found in pool!");
            return null;
        }

        if (projectilePools[projectileType].Count == 0)
        {
            // Expand pool if empty
            GameObject newProj = Instantiate(projectilePrefabs.Find(p => p.name == projectileType));
            newProj.SetActive(false);
            projectilePools[projectileType].Enqueue(newProj);
        }

        return projectilePools[projectileType].Dequeue();
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        string projectileType = projectile.name.Replace("(Clone)", "").Trim(); // Remove "(Clone)" from name
        if (projectilePools.ContainsKey(projectileType))
        {
            projectilePools[projectileType].Enqueue(projectile);
        }
        else
        {
            Destroy(projectile); // Destroy if the type was removed or invalid
        }
    }
}
