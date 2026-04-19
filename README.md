# SpeedRunners level API
This API is intended for the PC version of the 2016 game *SpeedRunners*. It provides several utilities to interact with the level file format, allowing you to load, create, modify and save levels. The API tries to be as complete as possible, even allowing you to make changes that the game's level editor is not capable of, though it currently lacks support for Origin levels as they use an entirely different format.

A level is represented via the `Level` class, which consists of some header data, a list of actors and a list of tile layers. Note that *collision tiles* is sometimes used as a unit of distance/size and is made up of 16 "regular" units. A collision tile is the width or height of a black square tile from the collision tile layer. Note that for some background tile layers, a tile has a width and height of 2 or 4 collision tiles (32 or 64 units).

## Loading levels
The `Level` class provides several static methods that allow you to read levels from file.

### From file
Use `Level.ReadFromFile`.
```cs
Level myLevel = Level.ReadFromFile("the/file/path.sr");
```

### Officials
Use the `EOfficial` enum with `Level.ReadOfficial`.
```cs
Level metroLevel = Level.ReadOfficial(EOfficial.METRO);
```

### Subscribed
Use the level's title or ID with `Level.ReadSubscribed`. This works for both RWS and subscribed levels. Refer to the method's documentation in case it cannot find the file.
```cs
Level pitfallLevel = Level.ReadSubscribed("Pitfall");
Level grappleCircuitLevel = Level.ReadSubscribed("Grapple Circuit");
Level terminalLevel = Level.ReadSubscribed(3408574219);
```

### Published
Use the level's title or ID with `Level.ReadPublished`. Refer to the method's documentation in case it cannot find the file.
```cs
Level myLevel = Level.ReadPublished("My level");
Level myLevel2 = Level.ReadPublished(3408574055);
```

### Local
Use the level's title with `Level.ReadLocal`. Please make sure that Steam is running and you are logged-in. You also need to provide the "Steamworks.NET.dll" and "steam_api.dll" libaries.
```cs
Level myLevel = Level.ReadLocal("My level");
```

## Creating levels
Use the `Level` constructor by providing the theme via the `ETheme` enum and width and height values in collision tiles.

```cs
Level newLevel = new Level(ETheme.THEME_PARK, 1000, 800);
```

## Saving levels
The `Level` class provides several (non-static) methods that allow you to write levels to file.

### To file
Use `Level.WriteToFile`.
```cs
myLevel.WriteToFile("the/level/path.sr");
```

### Local
Use `Level.WriteLocal`. Please make sure that Steam is running and you are logged-in. You also need to provide the "Steamworks.NET.dll" and "steam_api.dll" libaries.
```cs
myLevel.WriteLocal("My level");
```

## The level format

Every level is made up of a list of actors of type `Actor`, a list of tile layers of type `TileLayer` and further header data:
- `Version` (`int`): The level format version, in case the level was read from file (`0` to `6`). See the documentation for more information. Use `ForceVersion` to force a specific version upon writing to file.
- `ThemeStr` (`string`): The level's theme as a string. Use the `Theme` property for access via the `ETheme` enum.
- `Singleplayer` (`bool`): Unused, always `false`.
- `BombTimer` (`int`): Unused, always `60`.
- `Author` (`string`): The level author`s name.
- `Title` (`string`): The level's title.
- `Description` (`string`): The level's description.
- `PublishedFileID` (`ulong`): The level's published file ID. The value is always `0` for unpublished levels.

### Actors

You can access the list of all actors via `Level.Actors`. Note that the `Actor` class has a child class for each actor type (see "Actor.cs"). Each of these child classes provides properties that allow you to access and modify every field's value.

```cs
Obstacle obstacle = level.GetActorsOfType<Obstacle>().First();

Console.Write(obstacle.Position.X + " " + obstacle.Position.Y);
obstacle.Position = new Vector2(100f, 300f);
```

You can add new actors via `Level.AddActor<T>`. For `Deco`, use `Level.AddDeco`. For `SoundEmitter`, use `Level.AddSoundEmitter`. For `Checkpoint`, use `Level.AddCheckpoint`.

```cs
BoostSection boost = level.AddActor<BoostSection>(500f, 800f);
boost.Rotation = BoostSection.ERotation.DEGREE_45;

Deco deco = level.AddDeco(100.0f, 400.0f, Bundles.Library.Pillar_front);
deco.Flipped = true;
deco.AnimationType = Deco.EAnimationType.SPAWNER;
deco.Lifetime = 1.0f;
deco.SpawnInterval = 0.3f;

SoundEmitter soundEmitter = level.AddSoundEmitter(600.0f, 100.0f, Bundles.Library.amb_library_clockworks);

level.Actors.RemoveAll(actor => actor is Checkpoint);
Checkpoint cp0 = level.AddCheckpoint(200.0f, 200.0f, predecessors: null, startpoint: true);
Checkpoint cp1 = level.AddCheckpoint(600.0f, 200.0f, predecessors: new[] { cp0 });
Checkpoint cp2 = level.AddCheckpoint(600.0f, 600.0f, predecessors: new[] { cp1 });
Checkpoint cp3 = level.AddCheckpoint(200.0f, 600.0f, predecessors: new[] { cp2 });
level.CheckpointConnect(cp3, cp0);
```

### Tile layers

You can access the list of all tile layers via `Level.TileLayers`. Individual layers can be accessed via `Level.GetTileLayer` and the corresponding `ELayer` enum. Each tile layer contains a 2-dimensional `int`-array `TileLayer.Tiles` representing the grid of all tile IDs.

```cs
TileLayer collision = level.GetTileLayer(ELayer.COLLISION);
int width = collision.Width;
int height = collision.Height;
collision.Tiles[20, 30] = TileLayer.COL_GRAPPLE_CEIL;
collision.Fill(50, 60, 100, 150, TileLayer.COL_FULL);
collision.Move(20, -30);
collision.Resize(1500, 1200);
```
