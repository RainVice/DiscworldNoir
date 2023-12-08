using UnityEngine;

public static class Constant
{
    public static int DEFAULTNUM = 5;
    public static float DEFAULTTIME = DEFAULTNUM;

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