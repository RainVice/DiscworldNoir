using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 敌人
/// </summary>
public class Enemy : MonoBehaviour
{
    private float hp = 40f;
    private Rigidbody2D m_rg2d;
    public Slider m_slider;

    private void Awake()
    {
        m_rg2d = GetComponent<Rigidbody2D>();
        hp *= 1f + GameManager.Instance.Day;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsNight)
        {
            Destroy(gameObject);
            return;
        }
        if (GameManager.Instance.Home.IsDestroyed())
        {
            Destroy(gameObject);
            return;
        }
        m_rg2d.velocity = (GameManager.Instance.Home.transform.position - transform.position).normalized * 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var baseAttack = other.gameObject.GetComponent<BaseAttack>();
        baseAttack?.AddEnemy(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var baseAttack = other.gameObject.GetComponent<BaseAttack>();
        baseAttack?.RemoveEnemy(this);
    }

    public void ChangeHP(int num = -1)
    {
        hp += num;
        m_slider.gameObject.SetActive(true);
        m_slider.value = hp / 40f;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}