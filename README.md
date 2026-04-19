# SpeedRunners level API
This API is intended for the PC version of the 2016 game *SpeedRunners*. It provides several utilities to interact with the level file format, allowing you to load, create, modify, save and publish levels. The API tries to be as complete as possible, even allowing you to make changes that the game's level editor is not capable of, though it currently lacks support for Origin levels as they use an entirely different format.

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

### Publish
Use `Level.WriteLocalAndPublish`. Read the documentation for more information. Please make sure that Steam is running and you are logged-in. You also 
need to provide the "Steamworks.NET.dll" and "steam_api.dll" libaries.
```cs
MemoryStream previewData = new MemoryStream();
new FileStream("preview.png", FileMode.Open).CopyTo(previewData);
myLevel.WriteLocalAndPublish(
  "My level",
  previewData.ToArray(),
  "My level",
  "This is my level.",
  ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic,
  new List<string>() { }, res => Console.WriteLine("Done!")
);
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

Actors are represented via the `Actor` class. It is made up of:
- `Position` (`Vector2`)
- `Size` (`Vector2`)
- `TypeStr` (`string`): The actor's type.
- `Fields` (`List<ActorField>`): Further properties specific to the actor's type. These are exactly the properties that you can edit in the level editor by right-clicking the actor.
The `ActorField` class consists of `Key` and `Value` strings.

The API provides specialized child classes of `Actor` corresponding to each actor type. These child classes provide specialized C# properties for each field that are properly typed and automatically handle the key-lookup and string conversion. The full list of child classes includes:

`Checkpoint`, `PlayerStart`, `Pickup`, `BoostSection`, `LethalObstacle`, `SuperBoostVolume`, `Obstacle`, `AIVolume`, `SwitchBlock`, `Switch`, `Trigger`, `EditableSoundEmitter`, `Bubbles`, `FallTile`, `Laser`, `SpawnPoint`, `BoostaCoke`, `Deco`, `TriggerSaw`, `TextDeco`, `Dove`, `Bouncepad`, `Bookcase`, `Leaves`, `DecoLight`, `DecoGlow`, `RocketLauncher`, `MetroTunnel`, `Timer`

You can directly access all actors via the `Level.Actors` field or use the `Level.GetActorsOfType<T>` method to get a filtered and typed list.

```cs
Obstacle obstacle = level.GetActorsOfType<Obstacle>().First();

Console.Write(obstacle.Position.X + " " + obstacle.Position.Y);
obstacle.Position = new Vector2(100f, 300f);
obstacle.ObstacleID = Obstacle.EObstacleID.TRASH_RED; // an example of such a property
```

You can add new actors via `Level.AddActor<T>`. For `Deco`, use `Level.AddDeco`, which allows you to pass a `Graphic` object. For `SoundEmitter`, use `Level.AddSoundEmitter`, which allows you to pass a `Sound` object. For `Checkpoint`, use `Level.AddCheckpoint`, which allows you to pass a list of predecessors and automatically connects them. To connect two specific checkpoints (e.g. for closing the loop), use the `Level.ConnectCheckpoints` method.

All `Graphic` and `Sound` objects are available under the `Bundles` class. A bundle is a collection of resources, including graphics and sounds, that the game can load and unload in batches. To each theme there corresponds a specific bundle, and there are further bundles that are always loaded by the game. The `Bundles` class provides a subclass for each bundle.

```cs
BoostSection boost = level.AddActor<BoostSection>(500f, 800f);
boost.Rotation = BoostSection.ERotation.DEGREE_45;

Deco deco = level.AddDeco(100.0f, 400.0f, Bundles.Library.Pillar_front);
deco.Flipped = true;
deco.AnimationType = Deco.EAnimationType.SPAWNER;
deco.Lifetime = 1.0f;
deco.SpawnInterval = 0.3f;

EditableSoundEmitter soundEmitter = level.AddSoundEmitter(600.0f, 100.0f, Bundles.Library.amb_library_clockworks);
soundEmitter.Volume = 0.8f;

level.Actors.RemoveAll(actor => actor is Checkpoint);
Checkpoint cp0 = level.AddCheckpoint(200.0f, 200.0f, predecessors: null, startpoint: true);
Checkpoint cp1 = level.AddCheckpoint(600.0f, 200.0f, predecessors: new[] { cp0 });
Checkpoint cp2 = level.AddCheckpoint(600.0f, 600.0f, predecessors: new[] { cp1 });
Checkpoint cp3 = level.AddCheckpoint(200.0f, 600.0f, predecessors: new[] { cp2 });
level.ConnectCheckpoints(cp3, cp0);
```

### Tile layers

Tile layers are represented via the `TileLayer` class. It is made up of:
- `LayerStr` (`string`): The layer's name. Use the `Layer` property for access via the `ELayer` enum.
- `Tiles` (`int[,]`): The layer's tiles grid stored in row-major format.
The width and height can be accessed via the `Width` and `Height` properties. The `TileLayer` class provides further utility methods such as `Clear`, `Fill`, `Move` and `Resize` for editing the tile layer and constants for the collision layer tile IDs beginning with `COL_`.

You can directly access all tile layers via the `Level.TileLayers` field or use the `Level.GetTileLayer` method to get a specific tile layer via an `ELayer` enum or the layer's name.

```cs
TileLayer collision = level.GetTileLayer(ELayer.COLLISION);
int width = collision.Width;
int height = collision.Height;
collision.Tiles[20, 30] = TileLayer.COL_GRAPPLE_CEIL;
collision.Fill(50, 60, 100, 150, TileLayer.COL_FULL);
collision.Move(20, -30);
collision.Resize(1500, 1200);
```

### Further utilities

- You can displace all actors via `Level.MoveActors` and displace the entire level via `Level.MoveAll`
- You can rename a trigger ID via `Level.RenameTriggerID`
- You can restore an actor's default values via `TypedActor.SetDefaults`
- You can scale an actor via `TypedActor.Scale`
