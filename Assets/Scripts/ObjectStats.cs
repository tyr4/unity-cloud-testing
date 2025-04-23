using System;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] public float damage = 1f;
    [SerializeField] public int weight = 10;
    [SerializeField] public int price = 10;
    
    [Header("Misc Stats")]
    [SerializeField] public int perception;
    [SerializeField] public int luck;
    [SerializeField] public int charisma;
    [SerializeField] public int strength;
    [SerializeField] public int stealth;
    [SerializeField] public int endurance;

    private Transform _inventory;

    private void Start()
    {
        _inventory = GameObject.Find("Player/Inventory").transform;
    }

    public void OnMouseDown()
    {
        Debug.Log("am intrat 2");
        PickupItem();
    }

    public void PickupItem()
    {
        Debug.Log("am intrat fraiere");
        var player = GameObject.Find("Player").GetComponent<PlayerHandler>();
        int _inventoryLoad = player.GetInventoryLoad();
        
        if (_inventoryLoad + weight < player.maxInventoryWeight)
        {
            gameObject.transform.SetParent(_inventory);
            gameObject.SetActive(false);
        }
    }
}
