using Godot;
using System;

public static class Preloads
{
	public static readonly Texture2D ATLAS_DOORS = GD.Load<Texture2D>("res://textures/doors.png");
    public static readonly PackedScene MAP_OBJECT_SCENE = GD.Load<PackedScene>("res://scenes/mapObject.tscn");
    public static readonly PackedScene MAP_OBJECT_DOOR_SCENE = GD.Load<PackedScene>("res://scenes/mapObjectDoor.tscn");
    public static readonly PackedScene MAP_CHARACTER_SCENE = GD.Load<PackedScene>("res://scenes/mapCharacter.tscn");
    public static readonly PackedScene MAP_PLAYER_SCENE = GD.Load<PackedScene>("res://scenes/mapPlayer.tscn");
}
