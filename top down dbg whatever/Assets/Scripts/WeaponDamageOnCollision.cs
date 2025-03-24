using UnityEngine;

public class WeaponDamageOnCollision : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NormalMobHandler normalMob = collision.gameObject.GetComponent<NormalMobHandler>();
        if (normalMob != null)
        {
            normalMob.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
