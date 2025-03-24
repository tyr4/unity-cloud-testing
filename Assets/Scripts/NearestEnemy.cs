using System;
using UnityEngine;

public class NearestEnemy : MonoBehaviour
{
    
    private BoxCollider2D _box;
    private GameObject _enemy;
    private Vector3 _minPos;

    private void Start()
    {
        _box= GameObject.Find("Player/Radius Objects/Nearest Enemy Radius").GetComponent<BoxCollider2D>();
    }

    public void FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_box.bounds.center, _box.bounds.size, 0);
        Debug.Log(colliders.Length);
        foreach (Collider2D obj in colliders)
        {
            if (obj.gameObject)
            {
                // if (obj.gameObject.layer == "Weapon"))
            }
        }
        
        // return gameObject;
    }
}
