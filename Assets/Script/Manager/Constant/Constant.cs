using UnityEngine;

public static class Constant
{
    public static int DEFAULTNUM = 5;
    public static float DEFAULTTIME = DEFAULTNUM;
    public static float ATTACKCD = 1f;
    //生成怪物时间
    public static float ENEMYCD = 10f;
    // 黑夜切换时间
    public static float NightCD = 30f;

    public static Color[] colors = {
        Color.white,
        Color.green,
        Color.yellow,
        new(1f,155f/255f,0f),
        new(1f,63f/255f,0f)
    };

    public static float Upgrade(int level)
    {
        return 1f + level * 0.1f;
    }
}