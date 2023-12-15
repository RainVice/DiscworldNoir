
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Enemy Target
    {
        get => target;
        set => target = value;
    }

    public int Attack
    {
        get => attack;
        set => attack = value;
    }
    
    private Enemy target;
    private int attack;
    private Rigidbody2D m_rg2d;

    private void Awake()
    {
        m_rg2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target.IsDestroyed())
        {
            Destroy(gameObject);
            return;
        }
        m_rg2d.velocity = (target.transform.position - transform.position).normalized * 5f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var component = other.gameObject.GetComponent<Enemy>();
            component.ChangeHP(-attack);
            // todo 爆炸特效
            Destroy(gameObject);
        }
    }
}