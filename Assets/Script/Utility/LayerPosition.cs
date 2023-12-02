using System;
using UnityEngine;

public struct LayerPosition
{
    
    public int layer;
    public int position;
    
    public LayerPosition(int layer, int position = 0)
    {
        this.layer = layer;
        this.position = position;
    }

    public static LayerPosition operator +(LayerPosition a, LayerPosition b)
    {
        return new LayerPosition(a.layer + b.layer, a.position + b.position);
    }
    
    public static LayerPosition operator -(LayerPosition a, LayerPosition b)
    {
        return new LayerPosition(a.layer - b.layer, a.position - b.position);
    }
    
    public static LayerPosition operator *(LayerPosition a, int b)
    {
        return new LayerPosition(a.layer * b, a.position * b);
    }
    
    public static LayerPosition operator /(LayerPosition a, int b)
    {
        return new LayerPosition(a.layer / b, a.position / b);
    }
    
    public static Vector3 operator +(Vector3 a, LayerPosition b)
    {
        return a + b.GetVector3();
    }
    public static Vector3 operator -(Vector3 a, LayerPosition b)
    {
        return a - b.GetVector3();
    }
    public static Vector3 operator +(LayerPosition a, Vector3 b )
    {
        return a.GetVector3() + b;
    }
    public static Vector3 operator -(LayerPosition a, Vector3 b)
    {
        return a.GetVector3() - b;
    }
    
    
    /// <summary>
    /// 角度
    /// </summary>
    public float Angle => 360f / layer / 6f * position;
    /// <summary>
    /// 距离
    /// </summary>
    public float Distance => layer * 1.05f;
    /// <summary>
    /// 此层个数
    /// </summary>
    public int Count => layer * 6;
    
    /// <summary>
    /// 计算 Vertor3
    /// </summary>
    /// <returns></returns>
    public Vector3 GetVector3()
    {
        return new Vector3(
            Distance * Mathf.Cos(Angle * Mathf.Deg2Rad),
            Distance * Mathf.Sin(Angle * Mathf.Deg2Rad),
            0
        );
    }

    /// <summary>
    /// 扫描周围的地形
    /// </summary>
    /// <param name="curVector3">当前的坐标</param>
    /// <param name="action">坐标处理回调</param>
    public void Scan(Vector3 curVector3, Action<Vector3> action)
    {
        for (var i = 1; i <= layer; i++)
        {
            for (var j = 1; j <= 6 * i; j++)
            {
                var vector3 = curVector3 + new LayerPosition(i, j);
                action.Invoke(vector3);
            }
        }
    }
    
    
}