using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Player stats")]
    [SerializeField] public float playerHp = 100f;
    [SerializeField] public float playerBaseDamage = 1f;
    [SerializeField] public int playerBaseInventoryWeight = 200;
    
    [SerializeField] public int playerPerception = 1;  // influences hidden traps/doors
    [SerializeField] public int playerLuck = 1;        // influences drops
    [SerializeField] public int playerCharisma = 1;    // influences speed
    [SerializeField] public int playerStrength = 1;    // influences base damage
    [SerializeField] public int playerStealth = 1;     // influences the detection rate
    [SerializeField] public int playerEndurance = 1;   // influences bearing weight

    private void Awake()
    {
        Instance = this;
    }
}
