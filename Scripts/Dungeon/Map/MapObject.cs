using Godot;
using System;

public partial class MapObject : Node2D
{
    private const int ATLAS_COLUMNS = 12;
    private const int ATLAS_ROWS = 10;
    private const int ATLAS_TEXTURE_SIZE_X = 16;
    private const int ATLAS_TEXTURE_SIZE_Y = 32;
    public LevelObject obj { get; set; }
    public override void _Ready()
    {
        if (obj == null)
        {
            QueueFree();
            return;
        }
    }
}
