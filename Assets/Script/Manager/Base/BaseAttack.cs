using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAttack : BaseBuild
{
    // 攻击力
    protected int attack;
    
    protected float attackCD = Constant.DEFAULTTIME;
    protected float attackTimer = 0;
    
    // 子弹
    public GameObject bullet;
    
    protected List<Enemy> enemies = new();

    protected override void Awake()
    {
        base.Awake();
        attack = m_buildData.attack;
        buildType = BuildType.Attack;
        inventory.Add(Resource.Bullet,20);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isPlace)
        {
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer >= Constant.DEFAULTTIME)
            {
                attackTimer %= Constant.DEFAULTTIME;
                Attack();
            }
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    protected void Attack()
    {
        if (GetNum(Resource.Bullet) <= 0) return;
        for (var i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] is null || enemies[i].IsDestroyed())
            {
                enemies.RemoveAt(i);
                i--;
            }
            else
            {
                var instantiate = Instantiate(bullet, transform.position, Quaternion.identity);
                var component = instantiate.GetComponent<Bullet>();
                component.Attack = attack;
                component.Target = enemies[i];
            }
        }
    }
    
}