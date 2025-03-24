using System;
using UnityEngine;

public class GoldHandler : MonoBehaviour
{
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private float followSpeed = 10f;
    
    private TrackPlayer _tracker;

    private void Start()
    {
        _tracker = gameObject.GetComponentInChildren<TrackPlayer>();
    }

    public void SpawnGold(Vector3 position)
    {
        Instantiate(goldPrefab, position, Quaternion.identity);
        gameObject.layer = LayerMask.NameToLayer("Gold");
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickupRadius"))
        {
            _tracker.SetSpeed(followSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
