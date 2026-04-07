# SpeedRunners level API
This API is intended for the PC version of the 2016 game *SpeedRunners*. It provides several utilities that allow you to load, create, modify and save levels.

## Loading levels
### From file
Use `Level.ReadFromFile`.
```cs
Level myLevel = Level.ReadFromFile("the/file/path.sr");
```

### Officials
Use the `EOfficial` enum with `Level.ReadOfficial`
```cs
Level metroLevel = Level.ReadOfficial(EOfficial.METRO);
```

### Subscribed
Use the level's title with `Level.ReadSubscribed`. This works for both RWS and subscribed levels. Refer to the method's documentation in case it cannot find the file.
```cs
Level pitfallLevel = Level.ReadSubscribed("Pitfall");
Level grappleCircuitLevel = Level.ReadSubscribed("Grapple Circuit");
```

### Published
Use the level's title with `Level.ReadPublished`. Refer to the method's documentation in case it cannot find the file.
```cs
Level myLevel = Level.ReadPublished("My level");
```

### Local
Use the level's title with `Level.ReadLocal`. Please make sure that Steam is running and you are logged-in. You also need to provide the "Steamworks.NET.dll" and "steam_api.dll" libaries.
```cs
Level myLevel = Level.ReadLocal("My level");
```

## Creating levels
Use the `Level` constructor with the `ETheme` enum.

```cs
Level newLevel = new Level(ETheme.THEME_PARK, 1000/*width*/, 800/*height*/);
```

## Saving levels
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

Every level is made up of a list of actors (`Actor`), a list of tile layers (`TileLayer`) and further header data including the theme, author, title and description.

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