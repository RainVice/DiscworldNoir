using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObstacle  : MonoBehaviour
{
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
    /// 询问是否需要物质
    /// </summary>
    /// <returns></returns>
    public virtual Resource IsNeed()
    {
        return Resource.None;
    }
}
