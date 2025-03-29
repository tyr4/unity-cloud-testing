using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;

public class NearestEnemy : MonoBehaviour
{
    private BoxCollider2D _box;

    private void Start()
    {
        _box = GameObject.Find("Player/Radius Objects/Nearest Enemy Radius").GetComponent<BoxCollider2D>();
    }

    public Vector2 FindNearestEnemy(Transform player)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_box.bounds.center, _box.bounds.size, 0);
        if (colliders.Length == 0) return Vector2.zero;

        // Native Arrays for Job Processing
        NativeArray<Vector2> enemyPositions = new NativeArray<Vector2>(colliders.Length, Allocator.TempJob);
        NativeArray<float> distances = new NativeArray<float>(colliders.Length, Allocator.TempJob);

        int enemyCount = 0;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
            {
                enemyPositions[enemyCount] = (Vector2)colliders[i].transform.position;
                distances[enemyCount] = float.MaxValue;
                enemyCount++;
            }
        }

        if (enemyCount == 0)
        {
            enemyPositions.Dispose();
            distances.Dispose();
            return Vector2.zero;
        }

        // Resizing NativeArrays to actual enemy count
        NativeArray<Vector2> validEnemyPositions = new NativeArray<Vector2>(enemyCount, Allocator.TempJob);
        NativeArray<float> validDistances = new NativeArray<float>(enemyCount, Allocator.TempJob);

        for (int i = 0; i < enemyCount; i++)
        {
            validEnemyPositions[i] = enemyPositions[i];
            validDistances[i] = distances[i];
        }

        // Create and Schedule the Job
        FindNearestEnemyJob job = new FindNearestEnemyJob
        {
            playerPosition = (Vector2)player.position,
            enemyPositions = validEnemyPositions,
            distances = validDistances
        };

        JobHandle jobHandle = job.Schedule(enemyCount, 64);
        jobHandle.Complete();

        // Find the nearest enemy after job completion
        int nearestIndex = 0;
        float minDistance = validDistances[0];

        for (int i = 1; i < validDistances.Length; i++)
        {
            if (validDistances[i] < minDistance)
            {
                minDistance = validDistances[i];
                nearestIndex = i;
            }
        }

        Vector2 nearestEnemyDirection = (validEnemyPositions[nearestIndex] - (Vector2)player.position).normalized;

        // Dispose Native Arrays
        validEnemyPositions.Dispose();
        validDistances.Dispose();
        enemyPositions.Dispose();
        distances.Dispose();

        return nearestEnemyDirection;
    }

    [BurstCompile]
    private struct FindNearestEnemyJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector2> enemyPositions;
        public Vector2 playerPosition;
        public NativeArray<float> distances;

        public void Execute(int index)
        {
            distances[index] = math.distancesq(playerPosition, enemyPositions[index]); // Fast squared distance
        }
    }
}
