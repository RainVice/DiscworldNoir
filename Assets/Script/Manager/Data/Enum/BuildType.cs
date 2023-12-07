using System;

[Flags]
public enum BuildType
{
    // 攻击
    Attack = 1,

    // 防御
    Defense = 2,

    // 生产
    Production = 4,

    // 运输
    Way = 8,
    
    // 制造
    Maker = 16
}