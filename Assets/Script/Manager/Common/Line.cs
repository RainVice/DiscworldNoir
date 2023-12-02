using UnityEngine;

/// <summary>
/// 控制线段的大小
/// </summary>
public class Line : MonoBehaviour
{
    private Vector2 m_StartPos;
    private Vector2 m_EndPos;
    private float m_Magnitude;
    private float m_Angle;
    
    public void InitLine(Vector2 startPos, Vector2 endPos)
    {
        var offset = startPos - endPos;
        m_StartPos = startPos;
        m_EndPos = endPos;
        m_Magnitude = offset.magnitude;
        m_Angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
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
        return instantiate.GetComponent<Line>();
    }
    
    
    
    
}