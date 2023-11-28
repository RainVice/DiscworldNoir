using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    }



    private void OnClickListener()
    {
        btnHome.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = Home );
        BtnWall.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = Wall);
        btnTurret.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = turret);
        btnMining.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = mining);
        btnCion.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = cion);
        btnBullet.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = bullet);
        btnWay.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = way);
        btnAxe.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = axe);
        btnHarvester.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = harvester);
        btnFactory.onClick.AddListener(() => GameManager.Instance.CurSelectedObject = factory);
    }

}