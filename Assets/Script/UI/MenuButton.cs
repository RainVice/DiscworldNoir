
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 底部菜单按钮
/// </summary>
public class MenuButton : MonoBehaviour
{
    
    // ************* 引用
    // 主页
    public Button btnHome;
    public GameObject Home;
    // 城墙
    public Button BtnWall;
    public GameObject Wall;
    // 炮台
    public Button btnTurret;
    public GameObject turret;
    // 铁矿采集器
    public Button btnMining;
    public GameObject mining;
    // 金矿采集器
    public Button btnCion;
    public GameObject cion;
    // 炮弹工厂
    public Button btnBullet;
    public GameObject bullet;
    // 运输线路
    public Button btnWay;
    public GameObject way;
    // 砍树机器（斧头）
    public Button btnAxe;
    public GameObject axe;
    // 木材加工厂
    public Button btnHarvester;
    public GameObject harvester;
    // 铁矿加工厂
    public Button btnFactory;
    public GameObject factory;

    public void Start()
    {
        OnClickListener();
        AddEnterListener();
    }



    private void OnClickListener()
    {
        btnHome.onClick.AddListener(() => {
            var homeBuilds = GameManager.Instance.GetBuilds(typeof(HomeBuild));
            if (homeBuilds.Count == 1)
            {
                UIManager.Instance.CreateToast();
                return;
            }
            GameManager.Instance.CurSelectedObject = Home;
        });
        BtnWall.onClick.AddListener(BuyBuild(Wall));
        btnTurret.onClick.AddListener(BuyBuild(turret));
        btnMining.onClick.AddListener(BuyBuild(mining));
        btnCion.onClick.AddListener(BuyBuild(cion));
        btnBullet.onClick.AddListener(BuyBuild(bullet));
        btnWay.onClick.AddListener(BuyBuild(way));
        btnAxe.onClick.AddListener(BuyBuild(axe));
        btnHarvester.onClick.AddListener(BuyBuild(harvester));
        btnFactory.onClick.AddListener(BuyBuild(factory));
    }
    
    // 购买物资
    private UnityAction BuyBuild(GameObject go)
    {
        return () =>
        {
            var homeBuilds = GameManager.Instance.GetBuilds(typeof(HomeBuild));
            if (homeBuilds.Count < 1)
            {
                UIManager.Instance.CreateToast("必须先放置大本营");
                return;
            }
            var buildDataByFuzzy = GameManager.Instance.GetBuildDataByFuzzy(go.name);
            if (buildDataByFuzzy.price <= GameManager.Instance.CrystalNum)
            {
                GameManager.Instance.CurSelectedObject = go;
            }
            else
            {
                UIManager.Instance.CreateToast("水晶数量不足");
            }
        };
    }

    private void AddEnterListener()
    {
        // 鼠标移入事件
        var entryEvent = new EventTrigger.TriggerEvent();
        entryEvent.AddListener(OnPointerEnter);
        var entry = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter, callback = entryEvent };
        // 鼠标移出事件
        var exitEvent = new EventTrigger.TriggerEvent();
        exitEvent.AddListener(OnPointerExit);
        var exit = new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit, callback = exitEvent };
        
        btnHome.GetComponent<EventTrigger>().triggers.Add(entry);
        BtnWall.GetComponent<EventTrigger>().triggers.Add(entry);
        btnTurret.GetComponent<EventTrigger>().triggers.Add(entry);
        btnMining.GetComponent<EventTrigger>().triggers.Add(entry);
        btnCion.GetComponent<EventTrigger>().triggers.Add(entry);
        btnBullet.GetComponent<EventTrigger>().triggers.Add(entry);
        btnWay.GetComponent<EventTrigger>().triggers.Add(entry);
        btnAxe.GetComponent<EventTrigger>().triggers.Add(entry);
        btnHarvester.GetComponent<EventTrigger>().triggers.Add(entry);
        btnFactory.GetComponent<EventTrigger>().triggers.Add(entry);
        
        btnHome.GetComponent<EventTrigger>().triggers.Add(exit);
        BtnWall.GetComponent<EventTrigger>().triggers.Add(exit);
        btnTurret.GetComponent<EventTrigger>().triggers.Add(exit);
        btnMining.GetComponent<EventTrigger>().triggers.Add(exit);
        btnCion.GetComponent<EventTrigger>().triggers.Add(exit);
        btnBullet.GetComponent<EventTrigger>().triggers.Add(exit);
        btnWay.GetComponent<EventTrigger>().triggers.Add(exit);
        btnAxe.GetComponent<EventTrigger>().triggers.Add(exit);
        btnHarvester.GetComponent<EventTrigger>().triggers.Add(exit);
        btnFactory.GetComponent<EventTrigger>().triggers.Add(exit);
        
    }


    private static void OnPointerEnter(BaseEventData eventData)
    {
        if (eventData is not PointerEventData pointerEventData) return;
        var go = pointerEventData.pointerCurrentRaycast.gameObject;
        var data = GameManager.Instance.GetBuildDataByFuzzy(go.name);
        if (data is not null)
        {
            UIManager.Instance.ShowTip(data.ToString());
        }
    }

    private static void OnPointerExit(BaseEventData eventData)
    {
        UIManager.Instance.CloseTip();
    }
}