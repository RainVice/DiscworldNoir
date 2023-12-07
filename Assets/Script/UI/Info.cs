using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public BaseBuild BaseBuild
    {
        get => baseBuild;
        set => baseBuild = value;
    }

    public Text name;
    public Text info;

    private BaseBuild baseBuild;

    public void SetInfo(string name,string info)
    {
        this.name.text = name;
        this.info.text = info;
    }
    
    public void Remove()
    {
        Destroy(baseBuild.gameObject);
    }
    
    public void Upgrade()
    {
        baseBuild.Upgrade();
    }

    public void UpgradeAll()
    {
        var baseBuilds = GameManager.Instance.GetBuilds(baseBuild.GetType());
        foreach (var build in baseBuilds)
        {
            build.Upgrade();
        }
    }
    
    
    
    
}
