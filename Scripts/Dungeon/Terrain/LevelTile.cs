using Godot;
using System;

public partial class LevelTile
{
    private Consts.TERRAIN_TYPE terrainType;
    public Consts.TERRAIN_TYPE GetTerrainType()
    {
        return terrainType;
    }
    public void SetTerrainType(Consts.TERRAIN_TYPE value)
    {
        terrainType = value;
        terrainCategory = Funcs.GetTerrainCategory(terrainType);
    }
    public Consts.TERRAIN_CATEGORY terrainCategory { get; set; }
    public LevelObject objectHolder { get; set; }
    public LevelTile()
    {
        terrainType = Consts.TERRAIN_TYPE.NONE;
    }
    public LevelTile(Consts.TERRAIN_TYPE terrainType)
    {
        this.terrainType = terrainType;
    }
}
