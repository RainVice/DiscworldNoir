using System;
using Effect;
using UnityEngine;

/// <summary>
/// 控制线段的大小
/// </summary>
public class Line : MonoBehaviour
{

    public GameObject circle;
    
    private Vector2 StartPos { get; set; }
    private Vector2 EndPos { get; set; }
    private LayerPosition StartLayerPosition { get; set; }
    private LayerPosition EndLayerPosition { get; set; }
    private float Magnitude { get; set; }
    private float Angle { get; set; }

    private void Awake()
    {
        StartLayerPosition = LayerPosition.Vector3ToLayerPosition(StartPos);
        EndLayerPosition = LayerPosition.Vector3ToLayerPosition(EndPos);
    }


    public void Push(bool direction)
    {
        var start = StartPos;
        var end = EndPos;
        if (!direction)
        {
            start = EndPos;
            end = StartPos;
        }

        var instantiate = Instantiate(circle,start,Quaternion.identity);
        this.CreateEffect().AddEffect(instantiate.transform.SlideTFTo(start,end)).Play(() => Destroy(instantiate));
    }
    
    
    /// <summary>
    /// 画线
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="color"></param>
    public static Line DrawLine(Vector2 startPos, Vector2 endPos)
    {
        // 计算角度和距离
        var offset = endPos - startPos;
        var magnitude = offset.magnitude;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        
        // 设置长度和角度
        var instantiate = Instantiate(GameManager.Instance.LinePrefab, startPos, Quaternion.identity);
        var localScale = instantiate.transform.localScale;
        localScale.x = magnitude;
        instantiate.transform.localScale = localScale;
        var eulerAngles = instantiate.transform.eulerAngles;
        eulerAngles.z = angle;
        instantiate.transform.eulerAngles = eulerAngles;
        var component = instantiate.GetComponent<Line>();
        component.StartPos = startPos;
        component.EndPos = endPos;
        component.Magnitude = magnitude;
        component.Angle = angle;
        return component;
    }
    
    
    
    
}