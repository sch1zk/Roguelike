using Godot;
using System;

public partial class DungeonWorld : Node
{
    private const int START_LEVEL = 0;
    private const int LEVELS_TOTAL = 2;
    private Dungeon dungeon = new Dungeon();
    private DungeonMap dungeonMap;
    public int currentTurn { get; }
    public override void _Ready()
    {
        dungeonMap = GetNode<DungeonMap>("DungeonMap");
        NewDungeon();
        ShowLevel(START_LEVEL);
    }
    private void NewDungeon()
    {
        GenerateDungeon();
    }
    private void GenerateDungeon()
    {
        dungeon.GenerateDungeon(LEVELS_TOTAL);
    }
    private void ShowLevel(int levelId)
    {
        //if(dungeonMap == null) return;
        //if(levelId < 0) {
        //	dungeonMap?.ClearMap(); 
        //	return;
        //}
        dungeonMap?.SetCurrentLevel(dungeon.GetDungeonLevel(levelId));
    }
}
