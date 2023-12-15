using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 具有攻击功能的物品基类
/// </summary>
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
        inventory.Add(Resource.Bullet,30);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isPlace)
        {
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer >= Constant.ATTACKCD)
            {
                attackTimer %= Constant.ATTACKCD;
                Attack();
            }
        }
    }

    /// <summary>
    /// 添加敌人
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    
    /// <summary>
    ///     移除敌人
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    /// <summary>
    /// 攻击事件
    /// </summary>
    protected void Attack()
    {
        if (GetNum(Resource.Bullet) <= 0) return;
        if (enemies.Count == 0) return;
        if (enemies[0].IsDestroyed()) enemies.RemoveAt(0);
        if (enemies.Count == 0) return;
        
        var instantiate = Instantiate(bullet, transform.position, Quaternion.identity);
        var component = instantiate.GetComponent<Bullet>();
        component.Attack = attack;
        component.Target = enemies[0];
        ChangeNum(Resource.Bullet,-1);
        

    }
    
}