using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    // ****************** 引用 ******************
    // 绿底瓦片
    public Tile tileGreen;
    // 正常瓦片
    public Tile tileNormal;
    // 地图
    private Tilemap m_tilemap;
    // 网格
    private Grid m_Grid;

    // ****************** 变量 *****************
    // 上一步滑过的坐标
    private Vector3Int m_PreVector3Int = Vector3Int.zero;


    private void Awake()
    {
        m_tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponent<Grid>();
    }

    private void Update()
    {
        OnMouseToTileListener();
    }

    /// <summary>
    /// 鼠标在瓦片上事件
    /// </summary>
    private void OnMouseToTileListener()
    {
        // 如果没有预选中物体，则直接返回
        if (GameManager.Instance.CurSelectedObject == null) return;
        // 获取坐标
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = m_tilemap.WorldToCell(mousePos);
        // 如果坐标改变，则触发事件
        if (m_PreVector3Int != cellPos && m_tilemap.HasTile(cellPos))
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
        // 实例化的预制体的位置
        if (GameManager.Instance.CurSelectedObject != null)
            GameManager.Instance.CurSelectedObject.transform.position = m_Grid.CellToWorld(cellPos);
    }

    /// <summary>
    /// 鼠标滑出瓦片的事件
    /// </summary>
    /// <param name="cellPos">瓦片的坐标</param>
    private void OnMouseExitTile(Vector3Int cellPos) { }
}