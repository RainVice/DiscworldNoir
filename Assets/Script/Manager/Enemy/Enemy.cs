using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int hp = 100;
    
    private void Awake()
    {
        Debug.Log("awake");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("触发了" + other.gameObject.name);
        var baseAttack = other.gameObject.GetComponent<BaseAttack>();
        if (baseAttack is not null)
        {
            baseAttack.AddEnemy(this);
        }
    }
    
    public void ChangeHP(int num = -1)
    {
        hp += num;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}