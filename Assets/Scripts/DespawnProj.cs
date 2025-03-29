using Unity.Cinemachine;
using UnityEngine;

public class DespawnProj : MonoBehaviour
{
    private BoxCollider2D _box;
    private float _boxWidth;
    private float _boxHeight;

    public GameObject projectile;

    private void Start()
    {
        _box = GameObject.Find("Player/Radius Objects/Proj Despawn Radius").GetComponent<BoxCollider2D>();
        _boxWidth = _box.size.x;
        _boxHeight = _box.size.y;
    }

    private void Update()
    {
        Debug.Log(projectile);
        if (projectile)
        {
            if (IsOutOfBounds(projectile.transform.position))
            { 
                ProjectilePool.Instance.ReturnProjectile(projectile);
            }
        }
    }
    
    public bool IsOutOfBounds(Vector3 objPosition)
    {
        Vector3 boxPos = _box.transform.position;
        return objPosition.x < boxPos.x - _boxWidth || objPosition.x > boxPos.x + _boxWidth ||
               objPosition.y < boxPos.y - _boxHeight || objPosition.y > boxPos.y + _boxHeight;
    }
}