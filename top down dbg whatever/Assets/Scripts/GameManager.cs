using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public float damageTickInterval = 1f;
    [Header("Player stats")]
    [SerializeField] public float playerHp = 100f;

    [Header("Weapon Stats")]
    [SerializeField] public float gaeBolgDamage = 10f;
    [SerializeField] public float gaeBolgLaunchForce = 10f;

    private void Awake()
    {
        Instance = this;
    }
}
