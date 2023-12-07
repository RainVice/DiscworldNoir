
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Enemy Target
    {
        get => target;
        set
        {
            target = value;
            offset = target.transform.position - transform.position;
        }
    }

    public int Attack
    {
        get => attack;
        set => attack = value;
    }
    
    private Enemy target;
    private int attack;
    private Vector3 offset;

    private void FixedUpdate()
    {
        transform.Translate( offset * 0.02f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(nameof(Enemy)))
        {
            var component = other.gameObject.GetComponent<Enemy>();
            component.ChangeHP(-attack);
            // todo 爆炸特效
            Destroy(gameObject);
        }
    }
}