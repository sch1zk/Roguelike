using Godot;
using System;

public partial class Funcs
{
    public static Consts.TERRAIN_CATEGORY GetTerrainCategory(Consts.TERRAIN_TYPE terrainType)
    {
        switch (terrainType)
        {
            // Floors
            case Consts.TERRAIN_TYPE.WOODEN_FLOOR:
                return Consts.TERRAIN_CATEGORY.FLOOR;
            case Consts.TERRAIN_TYPE.BRICK_FLOOR:
                return Consts.TERRAIN_CATEGORY.FLOOR;
            case Consts.TERRAIN_TYPE.PLATE_FLOOR:
                return Consts.TERRAIN_CATEGORY.FLOOR;
            case Consts.TERRAIN_TYPE.DIRT_FLOOR:
                return Consts.TERRAIN_CATEGORY.FLOOR;
            // Walls
            case Consts.TERRAIN_TYPE.BRICK_WALL:
                return Consts.TERRAIN_CATEGORY.WALL;
            case Consts.TERRAIN_TYPE.DIRT_WALL:
                return Consts.TERRAIN_CATEGORY.WALL;
            // Nothing
            default:
                return Consts.TERRAIN_CATEGORY.NONE;
        }
    }
}
