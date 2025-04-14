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

    public void OnMouseDown()
    {
        Debug.Log("am intrat 2");
        PickupItem();
    }
    
    public void PickupItem()
    {
        Debug.Log("am intrat fraiere");
        var player = GameObject.Find("Player").GetComponent<PlayerHandler>();
        int inventoryLoad = player.GetInventoryLoad();
        
        if (inventoryLoad + weight < player.maxInventoryWeight)
        {
            var inventory = GameObject.Find("Player/Inventory").transform;
            gameObject.transform.SetParent(inventory);
            gameObject.SetActive(false);
        }
    }
}
