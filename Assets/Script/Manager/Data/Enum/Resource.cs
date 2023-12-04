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
    
    //所有
    All = 2048
}