using UnityEngine;
using System.Collections;

public class NormalMobHandler : MonoBehaviour
{
    [SerializeField] public float currentHealth = 100f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private TrackPlayer playerTracker;
    [SerializeField] private GoldHandler goldHandler;
    
    private Rigidbody2D _rb;
    private PlayerHandler _player;
    private bool _isDamaging;
    
    public float damageValue = 5f;
    private float _maxHealth;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _maxHealth = currentHealth;
        playerTracker.SetSpeed(moveSpeed);
        _maxHealth = currentHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_player == null)
        {
            _player = collision.gameObject.GetComponent<PlayerHandler>();

            if (_player != null && !_isDamaging)
            {
                StartCoroutine(DamageInterval());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHandler>())
        {
            _player = null;
            _isDamaging = false;
        }
    }

    private IEnumerator DamageInterval()
    {
        _isDamaging = true;

        while (_player) // Only damage while the player is still colliding
        {
            _player.TakeDamage(damageValue);
            yield return new WaitForSeconds(GameManager.Instance.damageTickInterval);
        }

        _isDamaging = false; // Stop coroutine when player is gone
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            goldHandler.SpawnGold(transform.position);
            gameObject.SetActive(false);
        }
    }
}