using UnityEngine;
/// <summary>
/// 所有障碍物的基类，包括建筑和地形
/// </summary>
public abstract class BaseObstacle  : MonoBehaviour
{

    // 当前座标
    public Vector3Int CurPos
    {
        get => m_curPos;
        set => m_curPos = value;
    }
    // 当前位置
    protected Vector3Int m_curPos = Vector3Int.zero;
    public ObstacleType ObstacleType => m_obstacleType;
    protected ObstacleType m_obstacleType;
    
    /// <summary>
    /// 询问是否需要物质
    /// </summary>
    /// <returns></returns>
    public virtual Resource IsNeed()
    {
        return Resource.None;
    }
}
