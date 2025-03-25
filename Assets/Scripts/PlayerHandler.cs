
using System;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    [Header("Player stats")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Player animator")]
    [SerializeField] private Animator animator;

    [SerializeField] private HorizontalFlip horizontalFlip;

    [Header("HP Bar")]
    [SerializeField] private HpBarHandler hpBar;


    // [Header("Weapons and effects")]
    // [SerializeField] private Weapon

    public float health;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _originalHealth;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        health = GameManager.Instance.playerHp;
        _originalHealth = health;
        // Time.timeScale = 0.2f;
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
            horizontalFlip.Flip(moveX, hpBar.transform);
            
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
        hpBar.ChangeHpBarValue(_originalHealth, health);
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
}