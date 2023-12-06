﻿using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObstacle  : MonoBehaviour
{
    
    

    // 当前座标
    public Vector3Int CurPos
    {
        get => m_curPos;
        set => m_curPos = value;
    }
    // 当前位置
    protected Vector3Int m_curPos;
    public ObstacleType ObstacleType => m_obstacleType;
    protected ObstacleType m_obstacleType;
    
    /// <summary>
    /// 询问是否需要传出物质
    /// </summary>
    /// <returns></returns>
    public virtual Resource IsOut()
    {
        return Resource.None;
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
    /// 询问是否需要物质
    /// </summary>
    /// <returns></returns>
    public virtual Resource IsNeed()
    {
        return Resource.None;
    }
}
