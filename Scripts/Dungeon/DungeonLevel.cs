using Godot;
using System;
using System.Collections.Generic;
//using System.Runtime;

public partial class DungeonLevel
{
    private LevelTile[,] tileMap = new LevelTile[Consts.LEVEL_SIZE_MAX.x, Consts.LEVEL_SIZE_MAX.y];
    public LevelTile[,] GetLevelTiles() { return tileMap; }
    public int levelId { get; set; }
    private readonly Dungeon dungeon;
    public DungeonLevel(Dungeon dungeon)
    {
        InitTileMap();
        this.dungeon = dungeon;
    }
    private void InitTileMap()
    {
        for (int x = 0; x < tileMap.GetLength(0); x++)
        {
            for (int y = 0; y < tileMap.GetLength(1); y++)
            {
                tileMap[x, y] = new LevelTile();
            }
        }
    }
    public void SetTile(Consts.TERRAIN_TYPE tileType, int x, int y)
    {
        tileMap[x, y]?.SetTerrainType(tileType);
    }
    public void SetTile(Consts.TERRAIN_TYPE tileType, Vector2i coordinates)
    {
        tileMap[coordinates.x, coordinates.y]?.SetTerrainType(tileType);
    }
    public void SetObject(Consts.OBJECT_TYPE objectType, int x, int y)
    {
        LevelObject newObject = new LevelObject(objectType);
        tileMap[x, y].objectHolder = newObject;
    }
    public void SetObject(Consts.OBJECT_TYPE objectType, Vector2i coordinates)
    {
        LevelObject newObject = new LevelObject(objectType);
        tileMap[coordinates.x, coordinates.y].objectHolder = newObject;
    }
    public void SetLevelStairs(int upX, int upY, int downX, int downY)
    {
        SetObject(Consts.OBJECT_TYPE.STAIRS_UP, upX, upY);
        SetObject(Consts.OBJECT_TYPE.STAIRS_DOWN, downX, downY);
    }
    public void SetLevelStairs(Vector2i stairsUp, Vector2i stairsDown)
    {
        SetObject(Consts.OBJECT_TYPE.STAIRS_UP, stairsUp.x, stairsUp.y);
        SetObject(Consts.OBJECT_TYPE.STAIRS_DOWN, stairsDown.x, stairsDown.y);
    }
}
