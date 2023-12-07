using System;

[Flags]
public enum Resource : uint
{
    None = 0,
    //水晶
    Crystal = 1,
    //铁
    Iron = 2,
    //树
    Tree = 4,
    // 弹药
    Bullet = 8,
    // 铁锭
    Ingot = 16,
    // 木材
    Wood = 32,
    
    
    //所有
    All = Crystal | Iron | Tree | Bullet,
}