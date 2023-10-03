using Godot;
using System;

public partial class LevelObject
{
    public Consts.OBJECT_TYPE objectType { get; set; }
    public LevelObject(Consts.OBJECT_TYPE objectType)
    {
        this.objectType = objectType;
    }
}
