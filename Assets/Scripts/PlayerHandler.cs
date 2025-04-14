using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    [Header("Player stats")]
    [SerializeField] public float moveSpeed = 1f;
    
    [Header("Player animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private HorizontalFlip horizontalFlip;
    
    [Header("Serialized for debugging, dont edit")]
    [SerializeField] public float health;
    [SerializeField] public float maxInventoryWeight;

    [SerializeField] public float perception;  // influences hidden traps/doors
    [SerializeField] public float luck;        // influences drops
    [SerializeField] public float charisma;    // influences speed
    [SerializeField] public float strength;    // influences base damage
    [SerializeField] public float stealth;     // influences the detection rate
    [SerializeField] public float endurance;   // influences bearing weight
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        health = GameManager.Instance.playerHp;
        maxInventoryWeight = GameManager.Instance.playerBaseInventoryWeight;  // TODO: make this influenced by endurance
        perception = GameManager.Instance.playerPerception;
        luck = GameManager.Instance.playerLuck;
        charisma = GameManager.Instance.playerCharisma;
        strength = GameManager.Instance.playerStrength;
        stealth = GameManager.Instance.playerStealth;
        endurance = GameManager.Instance.playerEndurance;

        moveSpeed += charisma / 10;
    }

    void Update()
    {
        // get movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        _moveInput = new Vector2(moveX, moveY).normalized;
        
        // handle player animation
        if (moveX == 0 && moveY == 0)
        {
            animator.SetBool(IsMoving, false);
        }
        else
        {
            // flip the asset for left/right directions
            horizontalFlip.Flip(moveX, transform);
            // horizontalFlip.Flip(moveX, hpBar.transform);
            
            // enable the running animation
            animator.SetBool(IsMoving, true);
        }
    }
    
    void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput * moveSpeed;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDamage(float value)
    {
        health -= value;
        // hpBar.ChangeHpBarValue(_originalHealth, health);
        if (health <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Player killed");
        Destroy(gameObject);
    }

    public int GetInventoryLoad()
    {
        var inventoryChildren = transform.Find("Inventory").gameObject;
        int weight = 0;
        for (int i = 0; i < inventoryChildren.transform.childCount; i++)
        {
            var child = inventoryChildren.transform.GetChild(i).GetComponent<ObjectStats>();
            weight += child.weight;
        }
        return weight;
    }
}