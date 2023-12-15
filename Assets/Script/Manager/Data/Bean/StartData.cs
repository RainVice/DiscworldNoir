
using DB;

public class StartData : BaseData
{
    // 是否启动过
    public bool isStart;
    public StartData()
    {
        
    }

    public StartData(bool isStart)
    {
        this.isStart = isStart;
    }
}