using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    [SerializeField] private HorizontalFlip horizontalFlip;
    
    private float _previousX;
    private Rigidbody2D _rb;
    private float _moveSpeed;
    private Transform _player;
    private Transform _object;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }
    
    private void FixedUpdate()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _object = transform.parent;
        
        _previousX = _player.position.x;
        
        // track the player
        Vector2 directionToPlayer = (_player.position - _object.position).normalized;
        _rb.linearVelocity = directionToPlayer * _moveSpeed;

        // flip the asset for left/right directions
        if (horizontalFlip)
        {
            horizontalFlip.Flip(_previousX - _object.position.x, _object);
        }
    }
}