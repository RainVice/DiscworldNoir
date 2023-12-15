using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 所有建筑的基类
/// </summary>
public abstract class BaseBuild : BaseObstacle
{
    // **************** 属性 *****************
    // 是否放置
    public bool IsPlace
    {
        get => isPlace;
        set => isPlace = value;
    }
    
    // ***************** 引用 ******************
    
    // 血条
    public Slider slider;

    // ***************** 变量 ******************
    // 建筑中文名
    protected string CName;
    // 范围
    protected int distance;
    // 等级
    protected int level;
    // 是否放置
    protected bool isPlace = false;
    // 类型
    protected BuildType buildType;
    // 生命
    protected float hp;
    // 最大生命
    protected int maxHp;
    // 升级价格
    protected int upgradePrice;
    // 价格
    protected int price;
    // 运输速度
    protected float waySpeed;
    // 运输计时
    private float m_wayTimer;
    protected SpriteRenderer spriteRenderer;
    
    
    // 连接的障碍
    protected Dictionary<Type, List<BaseObstacle>> obstacles = new();
    // 库存物资
    protected Dictionary<Resource, int> inventory = new();
    // 数据
    protected BuildData m_buildData;
    // 未放置状态的上一步位置
    protected Vector3 m_lastPos;
    // 线条集合
    protected Dictionary<BaseObstacle,Line> m_lineDic = new();

    protected virtual void Awake()
    {
        m_obstacleType = ObstacleType.Build;
        m_buildData = GameManager.Instance.GetBuildData(GetType().Name);
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_buildData is null) return;
        CName = m_buildData.name;
        distance = m_buildData.distance;
        level = m_buildData.level;
        isPlace = m_buildData.isPlace;
        buildType = m_buildData.buildType;
        hp = m_buildData.hp;
        maxHp = m_buildData.hp;
        upgradePrice = m_buildData.upgradePrice;
        price = m_buildData.price;
        waySpeed = m_buildData.waySpeed;
        slider = GetComponentInChildren<Slider>();
    }

    protected virtual void OnDestroy()
    {
        // 销毁上一次资源
        DestroyPre();
        GameManager.Instance.RemoveNode(this);
    }

    protected virtual void FixedUpdate()
    {
        // 调用移动事件
        OnMove();
        // 判断是否可以调用运输事件
        if (waySpeed!= 0 && isPlace)
        {
            m_wayTimer += Time.fixedDeltaTime;
            if (m_wayTimer >= Constant.DEFAULTTIME / waySpeed)
            {
                m_wayTimer %= Constant.DEFAULTTIME / waySpeed;
                OnWay();
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 接触到敌人扣血
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(Time.fixedDeltaTime);
            hp -= Time.fixedDeltaTime * 5f;
            slider.gameObject.SetActive(true);
            // 血条显示
            slider.value = hp / maxHp;
        }
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // 敌人离开隐藏血条
        if (other.gameObject.CompareTag("Enemy"))
        {
            slider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 展示建筑信息
    /// </summary>
    public virtual void ShowInfo() { }

    /// <summary>
    /// 运输事件
    /// </summary>
    protected virtual void OnWay() { }

    
    /// <summary>
    /// 移动事件
    /// </summary>
    protected virtual void OnMove()
    {
        if (m_lastPos == transform.position) return;
        m_lastPos = transform.position;
        // 扫描
        Scan();
        // 判断是否可以放置
        if (CanPlace())
        {
            var color = Color.white;
            color.a = 0.3f;
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            var color = Color.red;
            color.a = 0.3f;
            GetComponent<SpriteRenderer>().color = color;
        }

    }
    
    
    /// <summary>
    /// 扫描周围的格子
    /// </summary>
    private void Scan()
    {        
        // 销毁上一次资源
        DestroyPre();
        OnScan(out var clean, out var action);
        clean?.Invoke();
        var layerPosition = new LayerPosition(distance);
        GameManager.Instance.Scan(layerPosition, transform.position, obstacle => action?.Invoke(obstacle));
    }

    /// <summary>
    /// 扫描周围的格子
    /// </summary>
    /// <param name="clean">扫描之前的清理</param>
    /// <param name="action">扫描后结果的处理</param>
    protected virtual void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = null;
    }
    
    /// <summary>
    /// 添加线
    /// </summary>
    /// <param name="end"></param>
    protected void AddLine(BaseObstacle end)
    {
        m_lineDic.Add(end,Line.DrawLine(transform.position, end.transform.position));
        if (!obstacles.ContainsKey(end.GetType()))
        {
            obstacles.Add(end.GetType(), new List<BaseObstacle>());
        }
        obstacles[end.GetType()].Add(end);
    }


    /// <summary>
    /// 获取线
    /// </summary>
    /// <param name="obs"></param>
    /// <returns></returns>
    public Line GetLine(BaseObstacle obs)
    {
        return m_lineDic[obs];
    }

    public Dictionary<BaseObstacle,Line> GetLines()
    {
        return m_lineDic;
    }
    
    /// <summary>
    /// 销毁所有线段
    /// </summary>
    protected void DestroyPre()
    {
        foreach (var line in m_lineDic.Where(line => !line.Value.IsDestroyed()))
        {
            Destroy(line.Value.gameObject);
        }
        m_lineDic.Clear();
        obstacles.Clear();
    }


    /// <summary>
    /// 是否可以放置
    /// </summary>
    /// <returns></returns>
    public virtual bool CanPlace()
    {
        return GameManager.Instance.CrystalNum >= price;
    }

    /// <summary>
    /// 更改资源
    /// </summary>
    /// <param name="type"></param>
    public virtual void ChangeNum(Resource resource,int num = 1)
    {
        inventory.TryAdd(resource, 0);
        inventory[resource] += num;
    }
    
    public int GetNum(Resource resource)
    {
        return inventory.TryGetValue(resource, out var num) ? num : 0;
    }
    
    /// <summary>
    /// 询问是否有指定物质
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public virtual bool IsHave(Resource resource)
    {
        return false;
    }

    /// <summary>
    /// 升级
    /// </summary>
    public virtual void Upgrade()
    {
        if (Constant.DEFAULTNUM * level > GameManager.Instance.CrystalNum)
        {
            UIManager.Instance.CreateToast("水晶不足，无法升级");
            return;
        }
        
        if (level >= 5)
        {
            UIManager.Instance.CreateToast("已经升级为最高等级");
            return;
        }
        GameManager.Instance.CrystalNum -= Constant.DEFAULTNUM * level;
        level++;
        spriteRenderer.color = Constant.colors[level - 1];
    }

    /// <summary>
    /// 移除
    /// </summary>
    public virtual void Remove()
    {
        Destroy(gameObject);
    }

}