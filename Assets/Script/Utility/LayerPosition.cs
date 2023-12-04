using System;
using UnityEngine;

public struct LayerPosition
{
    private static float Length = 1.05f;
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
    public float Angle
    {
        get
        {
            if (layer == 0) return 0;
            return 360f / layer / 6f * position;
        }
    }

    /// <summary>
    /// 距离
    /// </summary>
    public float Distance => layer * Length;
    /// <summary>
    /// 此层个数
    /// </summary>
    public int Count
    {
        get
        {
            if (layer == 0) return 1;
            return layer * 6;
        }
    }

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
    
    /// <summary>
    /// Vector3 转 LayerPosition
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public static LayerPosition Vector3ToLayerPosition(Vector3 vector3)
    {
        var layer = Mathf.RoundToInt(vector3.magnitude / Length);
        // 计算vertor3角度
        var angle = Mathf.Atan2(vector3.y, vector3.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        var position = Mathf.RoundToInt(angle / (360f / layer / 6f));
        return new LayerPosition(layer, position);
    }
    
    
}