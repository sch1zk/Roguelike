using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static partial class DungeonGenerator
{
    private static readonly Vector2i ROOM_SIZE_MAX = new Vector2i(6, 6);
    private static readonly Vector2i ROOM_SIZE_MIN = new Vector2i(6, 6);
    private static readonly Vector2i ROOM_GAP = new Vector2i(2, 2);
    private static readonly Vector2i ROOM_MARGINS = new Vector2i(1, -1);

    public static DungeonLevel GenerateDungeonLevel(Dungeon dungeon, int levelId)
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        Random random = new Random();
        int roomsCount = random.Next(4, 7);
        List<Rect2i> rooms = new List<Rect2i>();

        while (rooms.Count < roomsCount)
        {
            int roomSizeX = random.Next(ROOM_SIZE_MIN.x, ROOM_SIZE_MAX.x);
            int roomSizeY = random.Next(ROOM_SIZE_MIN.y, ROOM_SIZE_MAX.y);
            Rect2i room = new Rect2i(
                random.Next(0, Consts.LEVEL_SIZE_MAX.x - roomSizeX - 1),
                random.Next(0, Consts.LEVEL_SIZE_MAX.y - roomSizeY - 1),
                roomSizeX + ROOM_GAP.x,
                roomSizeY + ROOM_GAP.y
                );
            bool isRoomSuitable = true;
            foreach (Rect2i anotherRoom in rooms)
            {
                if (room.Intersects(anotherRoom))
                {
                    isRoomSuitable = false;
                    break;
                }
            }
            if (isRoomSuitable) rooms.Add(room);
        }
        GeneratorAStar aStar = new GeneratorAStar();
        DungeonLevel newDungeonLevel = new DungeonLevel(dungeon);

        for (int x = 0; x < Consts.LEVEL_SIZE_MAX.x; x++)
        {
            for (int y = 0; y < Consts.LEVEL_SIZE_MAX.y; y++)
            {
                newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.DIRT_WALL, x, y);
                aStar.AddPoint(aStar.GetAvailablePointId(), new Vector2i(x, y));
            }
        }
        foreach (long p in aStar.GetPointIds())
        {
            foreach (Vector2i dir in Consts.SIDES)
            {
                Vector2i k = (Vector2i)aStar.GetPointPosition(p) + dir;
                if (k.x < 0 || k.y < 0 || k.x > Consts.LEVEL_SIZE_MAX.x - 1 || k.y > Consts.LEVEL_SIZE_MAX.y - 1) continue;
                long p2 = aStar.GetClosestPoint(k);
                if (p != p2) aStar.ConnectPoints(p, p2);
            }
        }
        List<Vector2i> doorPoints = new List<Vector2i>();
        foreach (Rect2i room in rooms)
        {
            for (int x = room.Position.x + ROOM_MARGINS.x; x < room.End.x + ROOM_MARGINS.y; x++)
            {
                newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.BRICK_WALL, x, room.Position.y + ROOM_MARGINS.x);
                newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.BRICK_WALL, x, room.End.y - 1 + ROOM_MARGINS.y);
                aStar.RemovePoint(aStar.GetClosestPoint(new Vector2(x, room.Position.y + ROOM_MARGINS.x)));
                aStar.RemovePoint(aStar.GetClosestPoint(new Vector2(x, room.End.y - 1 + ROOM_MARGINS.y)));
            }
            for (int y = room.Position.y + ROOM_MARGINS.x; y < room.End.y + ROOM_MARGINS.y; y++)
            {
                newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.BRICK_WALL, room.Position.x + ROOM_MARGINS.x, y);
                newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.BRICK_WALL, room.End.x - 1 + ROOM_MARGINS.y, y);
                aStar.RemovePoint(aStar.GetClosestPoint(new Vector2(room.Position.x + ROOM_MARGINS.x, y)));
                aStar.RemovePoint(aStar.GetClosestPoint(new Vector2(room.End.x - 1 + ROOM_MARGINS.y, y)));
            }

            for (int x = room.Position.x + 1 + ROOM_MARGINS.x; x < room.End.x - 1 + ROOM_MARGINS.y; x++)
            {
                for (int y = room.Position.y + 1 + ROOM_MARGINS.x; y < room.End.y - 1 + ROOM_MARGINS.y; y++)
                {
                    newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.PLATE_FLOOR, x, y);
                    aStar.RemovePoint(aStar.GetClosestPoint(new Vector2(x, y)));
                }
            }

            List<Vector2i> opts = new List<Vector2i>();
            for (int x = room.Position.x + 2 + ROOM_MARGINS.x; x <= room.End.x - 2 + ROOM_MARGINS.y; x++)
            {
                opts.Add(new Vector2i(x, room.Position.y + ROOM_MARGINS.x));
                opts.Add(new Vector2i(x, room.End.y - 1 + ROOM_MARGINS.y));
            }
            for (int y = room.Position.y + 2 + ROOM_MARGINS.x; y <= room.End.y - 2 + ROOM_MARGINS.y; y++)
            {
                opts.Add(new Vector2i(room.Position.x + ROOM_MARGINS.x, y));
                opts.Add(new Vector2i(room.End.x - 1 + ROOM_MARGINS.y, y));
            }
            Vector2i r = opts[random.Next(0, opts.Count)];
            doorPoints.Add(r);
            newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.DIRT_FLOOR, r);
            newDungeonLevel.SetObject(Consts.OBJECT_TYPE.WOODEN_DOOR, r);
        }

        for (int p = 0; p < doorPoints.Count; p++)
        {
            if (p + 1 <= doorPoints.Count - 1)
            {
                long room1 = aStar.GetClosestPoint(doorPoints[p]);
                long room2 = aStar.GetClosestPoint(doorPoints[p + 1]);
                Vector2[] path = aStar.GetPointPath(room1, room2);
                foreach (Vector2 t in path)
                {
                    newDungeonLevel.SetTile(Consts.TERRAIN_TYPE.DIRT_FLOOR, (Vector2i)t);
                }
            }
        }

        Rect2i RoomWithStairsUp = rooms.First();
        Rect2i RoomWithStairsDown = rooms.Last();
        Vector2i s_up = new Vector2i
            (
            RoomWithStairsUp.End.x - 2 + ROOM_MARGINS.y,
            random.Next(RoomWithStairsUp.Position.y + 1 + ROOM_MARGINS.x, RoomWithStairsUp.End.y - 2 + ROOM_MARGINS.y)
            );
        Vector2i s_down = new Vector2i
            (
            RoomWithStairsDown.End.x - 2 + ROOM_MARGINS.y,
            random.Next(RoomWithStairsDown.Position.y + 1 + ROOM_MARGINS.x, RoomWithStairsDown.End.y - 2 + ROOM_MARGINS.y)
            );
        newDungeonLevel.SetLevelStairs(s_up, s_down);
        newDungeonLevel.levelId = levelId;


        watch.Stop();
        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        return newDungeonLevel;
    }
}

public partial class GeneratorAStar : AStar2D
{
    public override double _ComputeCost(long fromId, long toId)
    {
        return Math.Abs(fromId - toId);
    }

    public override double _EstimateCost(long fromId, long toId)
    {
        return Math.Min(0, Math.Abs(fromId - toId) - 1);
    }
}
