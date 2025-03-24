using Unity.Mathematics;
using UnityEngine;

public class NearestEnemy : MonoBehaviour
{
    private BoxCollider2D _box;
    private float _minPos = Mathf.Infinity;
    private Vector2 _minPosVector;

    private void Start()
    {
        _box= GameObject.Find("Player/Radius Objects/Nearest Enemy Radius").GetComponent<BoxCollider2D>();
    }

    public Vector2 FindNearestEnemy(Transform player)
    {
        // init values each call
        _minPos = Mathf.Infinity;
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_box.bounds.center, _box.bounds.size, 0);
        foreach (Collider2D obj in colliders)
        {
            if (obj.gameObject.CompareTag("Enemy"))
            {
                Vector2 direction = obj.transform.position - player.position;
                float distance = direction.sqrMagnitude;
                if (_minPos > distance)
                {
                    _minPos = distance;
                    _minPosVector = direction;
                }
            }
        }
        Debug.Log(_minPosVector);
        return _minPosVector;
    }
}
