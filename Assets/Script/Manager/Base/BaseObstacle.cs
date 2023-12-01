using UnityEngine;

public abstract class BaseObstacle  : MonoBehaviour
{
    public ObstacleType ObstacleType => m_obstacleType;
    protected ObstacleType m_obstacleType;
}

public enum ObstacleType
{
    // 建筑
    Build = 1,
    // 地形
    Terrain = 2
}