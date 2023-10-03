using Godot;
using System;

public static class Consts
{
    public enum TERRAIN_CATEGORY : byte
    {
        NONE,
        FLOOR,
        WALL
    }
    public enum TERRAIN_TYPE : byte
    {
        NONE,
        WOODEN_FLOOR,
        BRICK_FLOOR,
        PLATE_FLOOR,
        DIRT_FLOOR,
        BRICK_WALL,
        DIRT_WALL
    }
    public enum TERRAIN_ACTION : byte
    {
        NONE
    }
    public enum OBJECT_TYPE : byte
    {
        NONE,
        STAIRS_UP,
        STAIRS_DOWN,
        CHEST,
        WOODEN_DOOR
    }
    public enum OBJECT_ACTION : byte
    {
        NONE,
        MOVE_UP,
        MOVE_DOWN,
        TOGGLE_STATE
    }
    public enum ITEM_TYPE : byte
    {
        NONE
    }
    public static readonly Vector2i TILE_SIZE = new Vector2i(16, 16);
    public static readonly Vector2i LEVEL_SIZE_MAX = new Vector2i(65, 40);
    public static readonly Vector2i[] SIDES = new Vector2i[] { Vector2i.Up, Vector2i.Down, Vector2i.Left, Vector2i.Right };
}
