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

    public void SetInfo(BaseBuild baseBuild,string name,string info)
    {
        this.baseBuild = baseBuild;
        this.name.text = name;
        this.info.text = info;
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
