using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

/// <summary>
/// 瓦片控制器，控制大部分在瓦片上的事件
/// </summary>
public class TilemapController : MonoBehaviour
{
    // ****************** 引用 ******************
    // 移动目标
    public Transform m_MoveTarget;
    // 绿底瓦片
    // public Tile tileGreen;
    // 正常瓦片
    // public Tile tileNormal;
    // 地图
    private Tilemap m_Tilemap;
    // 网格
    private Grid m_Grid;
    // 地形
    public GameObject[] m_Terrains;

    // ****************** 变量 *****************
    // 鼠标在上一帧的位置
    private Vector3 m_PreMousePos = Vector3.zero;
    // 上一步滑过的坐标
    private Vector3Int m_PreVector3Int = Vector3Int.zero;
    // 选择的预制体
    private GameObject m_CurSelectedObject;
    // 实例化的游戏对象
    private GameObject m_InstanceGameObject;
    // 实例化对象的SpriteRenderer
    private SpriteRenderer m_SpriteRenderer;
    // 摄像机
    private Camera m_Camera;
    public CinemachineVirtualCamera m_Cinemachine;


    private void Awake()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponent<Grid>();
    }

    private void Start()
    {
        m_Camera = Camera.main;
        // 生成地形
        for (var i = 0; i < 500; i++)
        {
            var randomPoint = GenerateRandomPoint();
            var instantiate = Instantiate(
                m_Terrains[i % m_Terrains.Length], 
                m_Grid.CellToWorld(randomPoint),
                Quaternion.identity);
            GameManager.Instance.AddTerrain(randomPoint,instantiate);
        }
    }

    private void Update()
    {
        OnMouseToTileListener();
        OnMouseDownListener();
        OnMouseListener();
        OnMouseScrollListener();
    }
    
    
    /// <summary>
    /// 鼠标滚轮监听
    /// </summary>
    private void OnMouseScrollListener()
    {
        var axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            m_Cinemachine.m_Lens.OrthographicSize += axis * Time.deltaTime * 100f;
            m_Cinemachine.m_Lens.OrthographicSize = Mathf.Clamp(m_Cinemachine.m_Lens.OrthographicSize, 1, 20);
        }
    }

    /// <summary>
    /// 鼠标按住监听
    /// </summary>
    private void OnMouseListener()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            var offset = mousePos - m_PreMousePos;
            m_MoveTarget.position -= offset;
            m_PreMousePos = mousePos;
        }
    }
    
    /// <summary>
    /// 鼠标按下监听
    /// </summary>
    private void OnMouseDownListener()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标坐标
            m_PreMousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            // 鼠标在UI上直接返回
            if (EventSystem.current.IsPointerOverGameObject()) return;
            // 如果没有预选中物体，则直接返回
            if (!m_InstanceGameObject) return;
            // 如果放置的地方有物体则返回
            if (GameManager.Instance.GetBuild(m_PreVector3Int)) return;
            // 判断是否可以放置，如果不可以就直接销毁返回
            if (!m_InstanceGameObject.GetComponent<BaseBuild>().CanPlace())
            {
                Destroy(m_InstanceGameObject);
                Delete();
                return;
            }
            // 如果选中物体则放置
            GameManager.Instance.AddBuild(m_InstanceGameObject);
            GameManager.Instance.AddBuild(m_PreVector3Int, m_InstanceGameObject);
            // 修改透明度
            if (!m_SpriteRenderer) m_SpriteRenderer = m_InstanceGameObject.GetComponent<SpriteRenderer>();
            var color = m_SpriteRenderer.color;
            color.a = 1f;
            m_SpriteRenderer.color = color;
            // 置空
            Delete(!Input.GetKey(KeyCode.LeftControl));
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(m_InstanceGameObject);
            Delete();
        }
    }
    
    /// <summary>
    /// 鼠标在瓦片上事件
    /// </summary>
    private void OnMouseToTileListener()
    {
        // 如果没有预选中物体，则直接返回
        if (!GameManager.Instance.CurSelectedObject) return;
        // 获取坐标
        var mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = m_Tilemap.WorldToCell(mousePos);
        // 如果选择的预制体与 GameManager 的不同，弃用当前的
        if (m_CurSelectedObject!= GameManager.Instance.CurSelectedObject)
        {
            m_CurSelectedObject = GameManager.Instance.CurSelectedObject;
            Destroy(m_InstanceGameObject);
        }
        // 如果选择了，直接实例化游戏对象
        if (!m_InstanceGameObject)
        {
            m_InstanceGameObject = Instantiate(GameManager.Instance.CurSelectedObject, m_Grid.CellToWorld(cellPos),
                Quaternion.identity);
        }
        // 如果坐标改变，则触发事件
        if (m_PreVector3Int != cellPos && m_Tilemap.HasTile(cellPos))
        {
            OnMouseEnterTile(cellPos);
            OnMouseExitTile(m_PreVector3Int);
            m_PreVector3Int = cellPos;
        }
    }

    /// <summary>
    /// 鼠标滑入瓦片事件
    /// </summary>
    /// <param name="cellPos">瓦片的坐标</param>
    private void OnMouseEnterTile(Vector3Int cellPos)
    {
        if (!GameManager.Instance.GetBuild(cellPos)&& !GameManager.Instance.GetTerrain<BaseTerrain>(cellPos))
        {
            // 实例化的预制体的位置
           if (m_InstanceGameObject)
               m_InstanceGameObject.transform.position = m_Grid.CellToWorld(cellPos);
        }
        
    }

    /// <summary>
    /// 鼠标滑出瓦片的事件
    /// </summary>
    /// <param name="cellPos">瓦片的坐标</param>
    private static void OnMouseExitTile(Vector3Int cellPos) { }
    
    /// <summary>
    /// 生成随机坐标
    /// </summary>
    /// <returns></returns>
    private static Vector3Int GenerateRandomPoint()
    {
        Vector3Int randomPoint;
        do
        {
            randomPoint = new Vector3Int( Random.Range(-70,70), Random.Range(-70,70), 0);
        } while (GameManager.Instance.HasTerrain(randomPoint));
        return randomPoint;
    }
    
    
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="isDelete">是否删除预选</param>
    private void Delete(bool isDelete = true)
    {
        m_SpriteRenderer = null;
        m_InstanceGameObject = null;
        if (isDelete)
        {
            GameManager.Instance.CurSelectedObject = null;
        }
    }
    
    /// <summary>
    /// LayerPosition 转 Vector3Int
    /// </summary>
    /// <param name="lp"></param>
    /// <returns></returns>
    public Vector3Int LpToVector3Int(LayerPosition lp)
    {
        return m_Grid.WorldToCell(lp.GetVector3());
    }
    
}