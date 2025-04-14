using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float floatHeight = 0.01f;    // Height of the floating effect
    public float floatSpeed = 1.75f;       // Speed of the floating effect
    public float smoothTime = 0.15f;     // Smoothing factor

    private Vector3 _originalPosition;
    private float _currentY;

    void Update()
    {
        _originalPosition = transform.root.position;
        _currentY = _originalPosition.y;
        
        // Use Mathf.Sin for the basic oscillation, but apply a smooth lerp for the movement
        float targetY = _originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Smoothly interpolate between _currentY and the targetY using Lerp
        _currentY = Mathf.Lerp(_currentY, targetY, Time.deltaTime / smoothTime);

        // Apply the new Y value to the position, keeping X and Z constant
        transform.root.position = new Vector3(_originalPosition.x, _currentY, _originalPosition.z);
    }
}