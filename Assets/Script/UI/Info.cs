using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
/// <summary>
/// 建筑信息面板
/// </summary>
public class Info : MonoBehaviour
{
    public BaseBuild BaseBuild
    {
        get => baseBuild;
        set => baseBuild = value;
    }

    [FormerlySerializedAs("name")] public Text buildName;
    [FormerlySerializedAs("info")] public Text buildInfo;

    private BaseBuild baseBuild;

    public void SetInfo(BaseBuild baseBuild,string name,string info)
    {
        this.baseBuild = baseBuild;
        this.buildName.text = name;
        this.buildInfo.text = info;
    }
    
    public void Remove()
    {
        baseBuild.Remove();
        gameObject.SetActive(false);
    }
    
    public void Upgrade()
    {
        baseBuild.Upgrade();
        gameObject.SetActive(false);
    }

    public void UpgradeAll()
    {
        gameObject.SetActive(false);
        var baseBuilds = GameManager.Instance.GetBuilds(baseBuild.GetType());
        foreach (var build in baseBuilds)
        {
            build.Upgrade();
        }
    }
    
    
    
    
}
