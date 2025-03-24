using System;
using UnityEngine;

public class HpBarHandler : MonoBehaviour
{
    private SpriteRenderer _emptyHpSprite;
    private float _newPos;
    
    private void Start()
    {
        _emptyHpSprite = transform.Find("Empty HP").GetComponent<SpriteRenderer>();
        _newPos = transform.Find("Empty HP").GetComponent<Transform>().localPosition.x;
    }
    
    public void ChangeHpBarValue(float maxHealth, float currentHealth)
    {
        float lostPercentage = 1 - currentHealth / maxHealth;
        
        _emptyHpSprite.transform.localScale = new Vector2(lostPercentage, 1);
        _emptyHpSprite.transform.localPosition = new Vector2(_newPos - _newPos * lostPercentage, 0);
    }
}
