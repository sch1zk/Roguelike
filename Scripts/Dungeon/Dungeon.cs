using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Dungeon
{
    List<DungeonLevel> dungeonLevels = new List<DungeonLevel>();

    public Dungeon()
    {
    }
    public void GenerateDungeon(int dungeonDepth)
    {
        dungeonLevels.Clear();
        for (int levelId = 0; levelId < dungeonDepth; levelId++)
        {
            DungeonLevel newDungeonLevel = DungeonGenerator.GenerateDungeonLevel(this, levelId);
            dungeonLevels.Add(newDungeonLevel);
        }
    }
    public DungeonLevel GetDungeonLevel(int levelId)
    {
        return dungeonLevels.ElementAtOrDefault(levelId);
    }

    private static void MoveCharacterToOtherLevel(Character character, EventArgs eventArgs)
    {

    }
}
