using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ************** 属性 *****************
    // 单例
    public static UIManager Instance { get; private set; }
    // *************** 引用 *****************
    public Canvas canvas;
    // 吐司
    public GameObject toastPrefab;
    // 提示
    public Tip tip;
    
    // *************** 变量 *****************

    private void Awake()
    {
        Instance = this;
    }

    public void CreateToast(string msg = null)
    {
        var instantiate = Instantiate(toastPrefab, canvas.transform);
        if (msg != null)
        {
            instantiate.GetComponent<Toast>().Show(msg);
        }
        
    }
    
    public void ShowTip(string msg)
    {
        tip.Show(msg);
    }
    
    public void CloseTip()
    {
        tip.Close();
    }
    
}