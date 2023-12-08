using UnityEngine;

public static class Constant
{
    public static float DEFAULTTIME = 5;

    public static Color[] colors = new Color[]
    {
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