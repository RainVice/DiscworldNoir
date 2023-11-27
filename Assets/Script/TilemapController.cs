using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    public Tile tileGreen;
    public Tile tileNormal;

    private Tilemap m_tilemap;
    private Vector3Int m_preVector3Int = Vector3Int.zero;


    private void Start()
    {
        m_tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        OnMouseToTileListener();
        
        
        
    }

    private void OnMouseToTileListener()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = m_tilemap.WorldToCell(mousePos);
        if (m_preVector3Int != cellPos && m_tilemap.HasTile(cellPos))
        {
            OnMouseEnterTile(cellPos);
            OnMouseExitTile(m_preVector3Int);
            m_preVector3Int = cellPos;
        }
    }

    private void OnMouseEnterTile(Vector3Int cellPos)
    {
        m_tilemap.SetTile(cellPos, tileGreen);
    }

    private void OnMouseExitTile(Vector3Int cellPos)
    {
        m_tilemap.SetTile(cellPos, tileNormal);
    }
    

    
}