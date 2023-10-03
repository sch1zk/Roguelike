using Godot;
using System;
using System.Collections.Generic;



public partial class DungeonMap : TileMap
{
    enum LAYERS : byte
    {
        TILES,
        WALLS,
        OBJECTS,
        DOORS1,
        DOORS2
    }
    enum WALL_SIDES : byte
    {
        NONE, // 0b0000
        UP, // 0b1000
        DOWN, // 0b0100
        UP_DOWN, // 0b1100
        LEFT, // 0b0010
        UP_LEFT, // 0b1010
        DOWN_LEFT, // 0b0110
        UP_DOWN_LEFT, // 0b1110
        RIGHT, // 0b0001
        UP_RIGHT, // 0b1001
        DOWN_RIGHT, // 0b0101
        UP_DOWN_RIGHT, // 0b1101
        LEFT_RIGHT, // 0b0011
        UP_LEFT_RIGHT, // 0b1011
        DOWN_LEFT_RIGHT, // 0b0111
        UP_DOWN_LEFT_RIGHT // 0b1111
    }
    private static int GetWallSideFromVector2iList(List<Vector2i> sides)
    {
        int wallId = 0;
        byte wallBit = 0b0;
        foreach (Vector2i side in sides)
        {
            switch (side)
            {
                case Vector2i(0, -1):
                    wallId += (int)WALL_SIDES.UP;
                    wallBit += 0b1000;
                    break;
                case Vector2i(0, 1):
                    wallId += (int)WALL_SIDES.DOWN;
                    wallBit += 0b100;
                    break;
                case Vector2i(-1, 0):
                    wallId += (int)WALL_SIDES.LEFT;
                    wallBit += 0b10;
                    break;
                case Vector2i(1, 0):
                    wallId += (int)WALL_SIDES.RIGHT;
                    wallBit += 0b1;
                    break;
            }
        }
        Console.WriteLine(wallBit);
        return wallId;
    }
    private static readonly Vector2i[] ATLAS_TERRAIN = new Vector2i[]
    {
        new Vector2i() { x = 0, y = 0 },
        new Vector2i() { x = 1, y = 0 },
        new Vector2i() { x = 2, y = 0 },
        new Vector2i() { x = 3, y = 0 },
        new Vector2i() { x = 4, y = 0 },
        new Vector2i() { x = 0, y = 1 },
        new Vector2i() { x = 1, y = 1 }
    };
    private static readonly Vector2i[] ATLAS_OBJECTS = new Vector2i[]
    {
        new Vector2i() { x = 0, y = 0 },
        new Vector2i() { x = 1, y = 0 },
        new Vector2i() { x = 2, y = 1 },
        new Vector2i() { x = 3, y = 1 },
        new Vector2i() { x = 0, y = 0 }
    };
    private static readonly Vector2i[] ATLAS_WALL_SIDES = new Vector2i[]
    {
        new Vector2i() { x = 0, y = 0 },
        new Vector2i() { x = 6, y = 1 },
        new Vector2i() { x = 1, y = 0 },
        new Vector2i() { x = 1, y = 3 },
        new Vector2i() { x = 1, y = 1 },
        new Vector2i() { x = 4, y = 1 },
        new Vector2i() { x = 4, y = 3 },
        new Vector2i() { x = 0, y = 3 },
        new Vector2i() { x = 2, y = 1 },
        new Vector2i() { x = 5, y = 1 },
        new Vector2i() { x = 5, y = 3 },
        new Vector2i() { x = 2, y = 3 },
        new Vector2i() { x = 0, y = 1 },
        new Vector2i() { x = 3, y = 1 },
        new Vector2i() { x = 3, y = 3 },
        new Vector2i() { x = 6, y = 3 },
    };
    private DungeonLevel currentLevel;
    public void SetCurrentLevel(DungeonLevel dungeonLevel)
    {
        currentLevel = dungeonLevel;
        if (currentLevel != null) { CreateCurrentLevel(); }
    }
    public override void _Ready()
    {
    }
    public override void _Process(double delta)
    {
    }
    public void ClearMap()
    {
        Clear();
    }
    private void CreateCurrentLevel()
    {
        ClearMap();
        LevelTile[,] levelTiles = currentLevel.GetLevelTiles();
        for (int x = 0; x < levelTiles.GetLength(0); x++)
        {
            for (int y = 0; y < levelTiles.GetLength(1); y++)
            {
                SetTile(levelTiles[x, y], x, y);
            }
        }
    }
    private void SetTile(LevelTile tile, int x, int y)
    {
        UpdateTileTerrain(tile, x, y);
		if (tile.objectHolder != null)
		{
			UpdateTileObject(tile.objectHolder, x, y);
		}
    }
    private void UpdateTileTerrain(LevelTile tile, int x, int y)
    {
        Vector2i coordinates = new Vector2i(x, y);

        if (tile.terrainCategory is Consts.TERRAIN_CATEGORY.WALL)
        {
            if (tile.GetTerrainType() is Consts.TERRAIN_TYPE.BRICK_WALL)
            {
                Console.WriteLine("BRUH!");
            }
            List<Vector2i> sides = new List<Vector2i>();
            LevelTile[,] levelTiles = currentLevel.GetLevelTiles();
            foreach (Vector2i side in Consts.SIDES)
            {
                if (
                    x + side.x < 0 ||
                    y + side.y < 0 ||
                    x + side.x >= levelTiles.GetLength(0) ||
                    y + side.y >= levelTiles.GetLength(1)
                ) continue;
                LevelTile sideTile = levelTiles[x + side.x, y + side.y];
                Consts.TERRAIN_CATEGORY tileCategory = sideTile.terrainCategory;
                if (tileCategory is Consts.TERRAIN_CATEGORY.FLOOR) sides.Add(side);
            }
            int wallId = GetWallSideFromVector2iList(sides);
            Vector2i atlasCoordinates = ATLAS_WALL_SIDES[wallId];
            SetCell((int)LAYERS.TILES, coordinates, 0, atlasCoordinates);
            SetCell((int)LAYERS.WALLS, coordinates, 1, ATLAS_TERRAIN[(int)tile.GetTerrainType()]);
            return;
        }
        SetCell((int)LAYERS.TILES, coordinates, 1, ATLAS_TERRAIN[(int)tile.GetTerrainType()]);
    }
	private void UpdateTileObject(LevelObject obj, int x, int y)
	{
		Vector2i coordinates = new Vector2i(x, y);
        MapObject newMapObject = new MapObject();
        SetCell((int)LAYERS.OBJECTS, coordinates, 2, ATLAS_OBJECTS[(int)obj.objectType]);
        newMapObject = Preloads.MAP_OBJECT_SCENE.Instantiate<MapObject>();
        newMapObject.obj = obj;
        newMapObject.Position = MapToLocal(coordinates);
        Node2D objectsHolder = GetNode<Node2D>("Objects");
        objectsHolder.AddChild(newMapObject);
	}
}
