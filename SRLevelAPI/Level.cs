using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SRL
{
    /// <summary>
    /// Enumerator for official levels.
    /// </summary>
    public enum EOfficial
    {
        METRO, SS_ROYALE, MANSION, PLAZA, FACTORY, THEME_PARK, POWERPLANT, SILO, LIBRARY, NIGHTCLUB, ZOO, SWIFT_PEAKS, CASINO, FESTIVAL, RESORT, AIRPORT, LABORATORY
    }

    /// <summary>
    /// Provides utility methods for the <c>EOfficial</c> enum.
    /// </summary>
    public static class Officials
    {
        /// <summary>
        /// Filenames for each official (without .xnb extension) sorted by index.
        /// </summary>
        public static readonly string[] OfficialFilenames =
        {
            "metro",
            "ssroyale",
            "mansion",
            "plaza",
            "factory",
            "themepark",
            "powerplant",
            "silo",
            "library",
            "nightclub",
            "zoo",
            "swiftpeaks",
            "casino",
            "festival",
            "resort",
            "airport",
            "laboratory"
        };

        /// <summary>
        /// Lists all officials.
        /// </summary>
        public static IEnumerable<EOfficial> AllOfficials => Enum.GetValues(typeof(EOfficial)).Cast<EOfficial>();

        /// <summary>
        /// Converts an <c>EOfficial</c> enum to the official's corresponding filename (without .xnb extension).
        /// </summary>
        /// <param name="official">The official.</param>
        /// <returns>The official's filename.</returns>
        public static string ToFilename(this EOfficial official)
        {
            return OfficialFilenames[(int)official];
        }

        /// <summary>
        /// Converts an official's filename (without .xnb extension) back to its <c>EOfficial</c> enum.
        /// </summary>
        /// <param name="officialFilename">The official's filename.</param>
        /// <returns></returns>
        public static EOfficial FromFilename(string officialFilename)
        {
            return (EOfficial)Array.IndexOf(OfficialFilenames, officialFilename);
        }

        /// <summary>
        /// Gets the corresponding theme for an official.
        /// </summary>
        /// <param name="official">The official.</param>
        /// <returns>The official's theme.</returns>
        public static ETheme GetTheme(this EOfficial official)
        {
            return (ETheme)official;
        }
    }
    
    /// <summary>
    /// Enumerator for level themes.
    /// </summary>
    public enum ETheme
    {
        METRO, SS_ROYALE, MANSION, PLAZA, FACTORY, THEME_PARK, POWERPLANT, SILO, LIBRARY, NIGHTCLUB, ZOO, SWIFT_PEAKS, CASINO, FESTIVAL, RESORT, AIRPORT, LABORATORY, PROTOTYPE, ALLEY
    }

    /// <summary>
    /// Provides utility methods for the <c>ETheme</c> enum.
    /// </summary>
    public static class Themes
    {
        /// <summary>
        /// Names for each theme as stored in <c>Level.ThemeStr</c> sorted by index.
        /// </summary>
        public static readonly string[] ThemeNames =
        {
            "StageMetro",
            "StageShip",
            "StageMansion",
            "StageCity",
            "StageIndustry",
            "StageThemePark",
            "StagePowerplant",
            "StageSilo",
            "StageUniversity",
            "StageNightclub",
            "StageZoo",
            "StageSki",
            "StageCasino",
            "StageFestival",
            "StageResort",
            "StageAirport",
            "StageBoostacoke",
            "StageVR",
            "StageAlley"
        };

        /// <summary>
        /// Lists all themes.
        /// </summary>
        public static IEnumerable<ETheme> AllThemes => Enum.GetValues(typeof(ETheme)).Cast<ETheme>();

        /// <summary>
        /// Converts an <c>ETheme</c> enum to its corresponding name.
        /// </summary>
        /// <param name="theme">The theme.</param>
        /// <returns>The theme's name.</returns>
        public static string ToName(this ETheme theme)
        {
            return ThemeNames[(int)theme];
        }

        /// <summary>
        /// Converts a theme's name back to its <c>ETheme</c> enum.
        /// </summary>
        /// <param name="themeName">The theme's name.</param>
        /// <returns>The theme.</returns>
        public static ETheme FromName(string themeName)
        {
            return (ETheme)Array.IndexOf(ThemeNames, themeName);
        }

        public static IEnumerable<ELayer> AllLayers(this ETheme theme)
        {
            switch (theme)
            {
                case ETheme.PROTOTYPE:
                    return new[] { ELayer.COLLISION, ELayer.SHADING };
                case ETheme.ALLEY:
                    return new[] { ELayer.COLLISION, ELayer.OVERLAY, ELayer.SHADING, ELayer.BACKGROUND_0, ELayer.BACKGROUND_1 };
                default:
                    return new[] { ELayer.COLLISION, ELayer.SHADING, ELayer.BACKGROUND_0, ELayer.BACKGROUND_1 };
            }
        }
    }

    /// <summary>
    /// Represents a stateless level as created and saved in the level editor.
    /// Values are exactly the same and stored in exactly the same way as in the level file format.
    /// </summary>
    public class Level
    {
        private string path;

        /// <summary>
        /// The path to the level file, in case the level was read from a file.
        /// </summary>
        public string Path => path; 

        private int version;

        /// <summary>
        /// The version of the level file, in case the level was read from a file.
        /// <list type="bullet">
        /// <item>
        /// <description><c>>= 0</c> includes <c>Actors</c>, <c>Layers</c> and <c>Theme</c></description>
        /// </item>
        /// <item>
        /// <description><c>>= 2</c> includes <c>Singleplayer</c></description>
        /// </item>
        /// <item>
        /// <description><c>>= 3</c> includes <c>BombTimer</c></description>
        /// </item>
        /// <item>
        /// <description><c>>= 4</c> includes <c>Author</c></description>
        /// </item>
        /// <item>
        /// <description><c>>= 5</c> includes <c>Name</c> and <c>Description</c></description>
        /// </item>
        /// <item>
        /// <description><c>>= 6</c> includes <c>PublishedFileID</c></description>
        /// </item>
        /// </list>
        /// </summary>
        public int Version => version;

        /// <summary>
        /// Forces a specific version number to be used when writing to a file. On default, version 6 is used. 
        /// See <c>Version</c>.
        /// </summary>
        public int ForceVersion = 6;

        /// <summary>
        /// List of actors. 
        /// Available on any version.
        /// </summary>
        public List<Actor> Actors;

        /// <summary>
        /// List of tile layers. 
        /// Available on any version.
        /// </summary>
        public List<TileLayer> TileLayers;

        /// <summary>
        /// Theme of the level as a string. 
        /// Please use property <c>Theme</c> instead to access and method <c>ChangeTheme</c> to modify the theme.
        /// Available on any version.
        /// </summary>
        public string ThemeStr;

        /// <summary>
        /// Unused bool, it was originally used to determine whether it is a singleplayer or multiplayer level. 
        /// It is <c>true</c> for singleplayer levels. 
        /// In the current game version, this value is always <c>false</c>.
        /// Available from version 2.
        /// </summary>
        public bool Singleplayer;

        /// <summary>
        /// Unused int, it was originally used to determine the bomb timer for singleplayer levels.
        /// In the current game version, this value is always <c>60</c>.
        /// Available from version 3.
        /// </summary>
        public int BombTimer = 60;

        /// <summary>
        /// The auther of the level.
        /// Available from version 4.
        /// </summary>
        public string Author;

        /// <summary>
        /// The name of the level.
        /// Available from version 5.
        /// </summary>
        public string Name;

        /// <summary>
        /// The description of the level.
        /// Available from version 5.
        /// </summary>
        public string Description;

        /// <summary>
        /// The published file ID of the level.
        /// This value is always <c>0</c> for unpublished levels.
        /// Available from version 6.
        /// </summary>
        public ulong PublishedFileID;

        /// <summary>
        /// Property for the theme of the level as an <c>ETheme</c> enum.
        /// </summary>
        public ETheme Theme
        {
            get => Themes.FromName(ThemeStr);
        }

        /// <summary>
        /// Changes the theme of the level.
        /// This will also clear all <c>Deco</c> and <c>EditableSoundEmitter</c> actors and the <c>BACKGROUND_0</c>, <c>BACKGROUND_1</c> and <c>OVERLAY</c> layers.
        /// </summary>
        /// <param name="theme">The new theme as an <c>ETheme</c> enum.</param>
        public void ChangeTheme(ETheme theme)
        {
            Actors.RemoveAll(a => a is Deco || a is EditableSoundEmitter);

            int width = GetTileLayer(ELayer.COLLISION).Width;
            int height = GetTileLayer(ELayer.COLLISION).Height;

            int biggestTileSize = theme.AllLayers().Select(layer => layer.TileSize(theme)).Max();
            if (width % biggestTileSize != 0 || height % biggestTileSize != 0)
            {
                Console.WriteLine("WARNING: Trying to change the theme of a level with a size of tiles that is not a multiple of the biggest tile size of all the layers ({0})!", biggestTileSize);
            }

            TileLayers.RemoveAll(layer => layer.Layer == ELayer.BACKGROUND_0 || layer.Layer == ELayer.BACKGROUND_1 || layer.Layer == ELayer.OVERLAY);
            if (!TileLayers.Any(layer => layer.Layer == ELayer.SHADING))
                TileLayers.Add(new TileLayer(ELayer.SHADING, width, height));

            TileLayers.AddRange(
                theme.AllLayers().
                    Where(layer => layer != ELayer.COLLISION && layer != ELayer.SHADING).
                    Select(layer => new TileLayer(layer, width / layer.TileSize(theme), height / layer.TileSize(theme)))
            );

            ThemeStr = theme.ToName();
        }

        /// <summary>
        /// Reads a level from specified file.
        /// </summary>
        /// <param name="path">The level file.</param>
        /// <returns>The level that was read from the file.</returns>
        public static Level ReadFromFile(string path)
        {
            using (Stream stream = File.OpenRead(path))
            {
                using (BinaryReader reader = new BinaryReader(new GZipStream(stream, CompressionMode.Decompress)))
                {
                    return new Level(reader) { path = path };
                }
            }
        }

        /// <summary>
        /// Reads an official level from file.
        /// </summary>
        /// <param name="official">The official as an enum.</param>
        /// <param name="speedRunnersPath">
        /// The path of your SpeedRunners installation folder.
        /// Any occurrence of "[PROGRAM_FILES_X86]" will be replaced by the user's documents folder path.
        /// </param>
        /// <returns>The official level that was read from file.</returns>
        public static Level ReadOfficial(EOfficial official, string speedRunnersPath = "[PROGRAM_FILES_X86]\\Steam\\steamapps\\common\\SpeedRunners")
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            speedRunnersPath = speedRunnersPath.Replace("[PROGRAM_FILES_X86]", programFilesPath);
            string filename = official.ToFilename();
            filename = speedRunnersPath + "\\Content\\Levels\\Multiplayer\\" + filename + ".xnb";
            return ReadFromFile(filename);
        }

        /// <summary>
        /// Reads a subscribed level from file, which includes RWS.
        /// </summary>
        /// <param name="name">The name of the map.</param>
        /// <param name="savedGamesPath">
        /// The path of your SavedGames folder.
        /// Any occurrence of "[DOCUMENTS]" will be replaced by the user's documents folder path.
        /// </param>
        /// <returns>The first subscribed level that was read from file that matches the specified name. <c>null</c> if no match was found.</returns>
        public static Level ReadSubscribed(string name, string savedGamesPath = "[DOCUMENTS]\\SavedGames")
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            savedGamesPath = savedGamesPath.Replace("[DOCUMENTS]", documentsPath);
            string directory = savedGamesPath + "\\SpeedRunners\\CEngineStorage\\AllPlayers\\Subscribed";
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if (file.Contains("." + name + ".sr"))
                    return ReadFromFile(file);
            }
            return default;
        }

        /// <summary>
        /// Reads a published level from file (published by the currently logged-in Steam user).
        /// </summary>
        /// <param name="name">The name of the map.</param>
        /// <param name="savedGamesPath">
        /// The path of your SavedGames folder.
        /// Any occurrence of "[DOCUMENTS]" will be replaced by the user's documents folder path.
        /// </param>
        /// <returns>The first published level that was read from file that matches the specified name. <c>null</c> if no match was found.</returns>
        public static Level ReadPublished(string name, string savedGamesPath = "[DOCUMENTS]\\SavedGames")
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            savedGamesPath = savedGamesPath.Replace("[DOCUMENTS]", documentsPath);
            string directory = savedGamesPath + "\\SpeedRunners\\CEngineStorage\\AllPlayers\\Published\\";
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if (file.Contains("." + name + ".sr"))
                    return ReadFromFile(file);
            }
            return default;
        }

        private static bool steamInitialized = false;

        private static bool InitSteam()
        {
            if (!steamInitialized)
            {
                if (!Packsize.Test())
                {
                    Console.Error.WriteLine("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
                    return default;
                }
                if (!DllCheck.Test())
                {
                    Console.Error.WriteLine("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
                    return default;
                }
                if (!File.Exists("steam_appid.txt"))
                {
                    File.WriteAllText("steam_appid.txt", "207140"); // required by the Steam API to detect the application as the SpeedRunners game
                }
                if (!SteamAPI.Init())
                {
                    Console.Error.WriteLine("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
                    return default;
                }
                steamInitialized = true;
            }
            return true;
        }

        /// <summary>
        /// Reads a local level from file.
        /// Please make sure that Steam is running and you are logged-in.
        /// You also need to provide the "Steamworks.NET.dll" and "steam_api.dll" libaries.
        /// </summary>
        /// <param name="name">The name of the map.</param>
        /// <returns>The local level that was read from file that matches the specified name. <c>null</c> if SteamAPI could not be initialized.</returns>
        public static Level ReadLocal(string name)
        {
            if (!InitSteam())
                return default;
            if (!name.EndsWith(".sr"))
                name += ".sr";
            int fileSize = SteamRemoteStorage.GetFileSize(name);
            byte[] data = new byte[fileSize];
            SteamRemoteStorage.FileRead(name, data, fileSize);
            using (Stream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(new GZipStream(stream, CompressionMode.Decompress)))
                {
                    return new Level(reader);
                }
            }
        }

        /// <summary>
        /// Creates a new <c>Level</c> from specified theme, tiles width and tiles height.
        /// This will automatically add a <c>PlayerStart</c> actor, two <c>Checkpoint</c> actors closing a loop,
        /// <c>COLLISION</c> and <c>SHADING</c> tile layers of size <c>tilesW</c> and <c>tilesH</c> and
        /// <c>BACKGROUND_0</c> and <c>BACKGROUND_1</c> layers of size <c>tilesW / 2</c> and <c>tilesH / 2</c>.
        /// It's recommended that <c>tilesW</c> and <c>tilesH</c> are both a multiple of the biggest tile size of all the layers.
        /// This biggest tile size is usually 2.
        /// For <c>METRO</c>, <c>MANSION</c>, <c>THEME_PARK</c>, <c>LIBRARY</c> and <c>PROTOTYPE</c> themes it's 1.
        /// For <c>ALLEY</c> theme it's 4.
        /// </summary>
        /// <param name="theme">The theme of the level as an enum.</param>
        /// <param name="tilesW">The width of the level in tiles (16 units).</param>
        /// <param name="tilesH">The height of the level in tiles (16 untis).</param>
        public Level(ETheme theme, int tilesW, int tilesH)
        {
            int biggestTileSize = theme.AllLayers().Select(layer => layer.TileSize(theme)).Max();
            if (tilesW % biggestTileSize != 0 || tilesH % biggestTileSize != 0)
            {
                Console.WriteLine("WARNING: Trying to create a level with a size of tiles that is not a multiple of the biggest tile size of all the layers ({0})!", biggestTileSize);
            }

            version = 6;
            Actors = new List<Actor>(3);
            AddActor<PlayerStart>(0f, 0f);
            Checkpoint cp0 = AddCheckpoint(0f, 0f, predecessors: null, helper: false, startpoint: true);
            Checkpoint cp1 = AddCheckpoint(0f, 0f, predecessors: new[] { cp0 }, helper: false, startpoint: false);
            CheckpointConnect(cp1, cp0);
            TileLayers = theme.AllLayers().Select(layer => new TileLayer(layer, tilesW / layer.TileSize(theme), tilesH / layer.TileSize(theme))).ToList();
            ThemeStr = theme.ToName();
            Name = "";
            Author = "";
            Description = "";
        }

        /// <summary>
        /// Creates a <c>Level</c> that was read from a <c>BinaryReader</c>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Level(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// Reads the level from a <c>BinaryReader</c>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader)
        {
            version = reader.ReadInt32();

            int actorCount = reader.ReadInt32();
            Actors = new List<Actor>(actorCount);
            for (int i = 0; i < actorCount; i++)
            {
                // read the typeStr beforehand to be able to construct the Actor as the corresponding TypedActor subclass
                Actor.PreRead(out Vector2 position, out Vector2 size, out string typeStr, reader);

                Actor actor = TypedActor.CreateNew(typeStr);
                if (actor == null)
                {
                    Console.WriteLine("Unknown actor type \"" + typeStr + "\"!");
                    actor = new Actor();
                }
                actor.ReadAfterPre(position, size, typeStr, reader); // read the rest, providing the already read values

                Actors.Add(actor);
            }

            int tileLayersCount = reader.ReadInt32();
            TileLayers = new List<TileLayer>(tileLayersCount);
            for (int i = 0; i < tileLayersCount; i++)
            {
                TileLayers.Add(new TileLayer(reader));
            }

            ThemeStr = reader.ReadString();
            if (Version >= 2)
            {
                Singleplayer = reader.ReadBoolean();
            }
            if (Singleplayer && Version >= 3)
            {
                BombTimer = reader.ReadInt32();
            }
            if (Version >= 4)
            {
                Author = reader.ReadString();
            }
            if (Version >= 5)
            {
                Name = reader.ReadString();
                Description = reader.ReadString();
            }
            if (Version >= 6)
            {
                PublishedFileID = reader.ReadUInt64();
            }
        }

        /// <summary>
        /// Writes the level to a <c>BinaryWriter</c>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer)
        {
            int version = 6;
            if (ForceVersion >= 0)
                version = ForceVersion;
            writer.Write(version);

            writer.Write(Actors.Count);
            for (int i = 0; i < Actors.Count; i++)
            {
                Actors[i].Write(writer);
            }

            writer.Write(TileLayers.Count);
            for (int i = 0; i < TileLayers.Count; i++)
            {
                TileLayers[i].Write(writer);
            }

            writer.Write(ThemeStr);
            if (version >= 2)
            {
                writer.Write(Singleplayer);
            }
            if (Singleplayer && version >= 3)
            {
                writer.Write(BombTimer);
            }
            if (version >= 4)
            {
                writer.Write(Author);
            }
            if (version >= 5)
            {
                writer.Write(Name);
                writer.Write(Description);
            }
            if (version >= 6)
            {
                writer.Write(PublishedFileID);
            }
        }

        /// <summary>
        /// Writes the level to a file.
        /// </summary>
        /// <param name="path">The file to write the level to.</param>
        public void WriteToFile(string path)
        {
            using (Stream stream = File.OpenWrite(path))
            {
                using (BinaryWriter writer = new BinaryWriter(new GZipStream(stream, CompressionMode.Compress)))
                {
                    Write(writer);
                }
            }
        }

        /// <summary>
        /// Saves the level as a local level.
        /// Please make sure that Steam is running and you are logged-in.
        /// You also need to provide the "Steamworks.NET.dll" and "steam_api.dll" libaries.
        /// </summary>
        /// <param name="name">The name of the level file.</param>
        /// <returns><c>true</c> on success. <c>false</c> if SteamAPI could not be initialized or <c>SteamRemoteStorage.FileWrite</c> failed.</returns>
        public bool WriteLocal(string name)
        {
            if (!InitSteam())
                return false;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(new GZipStream(stream, CompressionMode.Compress)))
                {
                    Write(writer);
                }
                byte[] data = stream.ToArray();
                SteamRemoteStorage.GetQuota(out ulong pnTotalBytes, out ulong puAvailableBytes);
                if (puAvailableBytes < (ulong)data.Length)
                {
                    return false;
                }
                if (!SteamRemoteStorage.FileWrite(name + ".sr", data, data.Length))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Gets all actors of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T">The type to get all actors of.</typeparam>
        /// <returns>All actors of type <c>T</c>.</returns>
        public IEnumerable<T> GetActorsOfType<T>() where T : TypedActor
        {
            return Actors.OfType<T>();
        }

        /// <summary>
        /// Gets the corresponding tile layer from layer <c>layer</c>.
        /// </summary>
        /// <param name="layer">The layer to get the tile layer from as an enum.</param>
        /// <returns>The tile layer from layer <c>layer</c>.</returns>
        public TileLayer GetTileLayer(ELayer layer)
        {
            return GetTileLayer(layer.ToName());
        }

        /// <summary>
        /// Gets the corresponding tile layer from layer <c>layer</c>.
        /// </summary>
        /// <param name="layer">The layer to get the tile layer from as a string.</param>
        /// <returns>The tile layer from layer <c>layer</c> or <c>null</c> if no matching tile layer was found.</returns>
        public TileLayer GetTileLayer(string layer)
        {
            try
            {
                return TileLayers.First(t => t.LayerStr == layer);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Displaces all actors.
        /// </summary>
        /// <param name="displacement">The amount to displace all actors by.</param>
        public void MoveActors(Vector2 displacement)
        {
            foreach (Actor actor in Actors)
            {
                actor.Position += displacement;
            }
        }

        /// <summary>
        /// Displaces all actors and layers.
        /// Actors will be displaced by 16 times the amount of tiles.
        /// </summary>
        /// <param name="tilesX">The amount of tiles (16 units) to displace the level by in x-direction.</param>
        /// <param name="tilesY">The amount of tiles (16 units) to displace the level by in y-direction.</param>
        public void MoveAll(int tilesX, int tilesY)
        {
            int biggestTileSize = Theme.AllLayers().Select(layer => layer.TileSize(Theme)).Max();
            
            if (tilesX % biggestTileSize != 0 || tilesY % biggestTileSize != 0)
            {
                Console.WriteLine("WARNING: Trying to move a level by an amount of tiles that is not a multiple of the biggest tile size of all layers ({0})!", biggestTileSize);
            }

            MoveActors(new Vector2(tilesX * 16f, tilesY * 16f));

            TileLayers.ForEach(layer => layer.Move(tilesX / layer.Layer.TileSize(Theme), tilesY / layer.Layer.TileSize(Theme)));
        }

        /// <summary>
        /// Adds a new actor of type <c>T</c> to the level.
        /// <c>T</c> must be one of the subclasses of <c>TypedActor</c>.
        /// When adding <c>Deco</c>, <c>EditableSoundEmitter</c> or <c>Checkpoint</c> actors, consider using the
        /// <c>AddDeco</c>, <c>AddSoundEmitter</c> or <c>AddCheckpoint</c> methods to ensure the actors are correctly initialized.
        /// </summary>
        /// <typeparam name="T">The type of the actor.</typeparam>
        /// <param name="x">The x-position of the actor in units (1/16 tiles).</param>
        /// <param name="y">The y-position of the actor in units (1/16 tiles).</param>
        /// <returns>The newly added actor.</returns>
        public T AddActor<T>(float x, float y) where T : TypedActor, new()
        {
            if (typeof(T) == typeof(Bookcase) && Theme != ETheme.LIBRARY && Theme != ETheme.MANSION)
            {
                Console.WriteLine("WARNING: Trying to add a Bookcase to a level that does not use theme LIBRARY or MANSION!");
            }
            if (typeof(T) == typeof(DecoLight) && Theme != ETheme.NIGHTCLUB)
            {
                Console.WriteLine("WARNING: Trying to add a DecoLight to a level that does not use theme NIGHTCLUB!");
            }
            if (typeof(T) == typeof(DecoGlow) && Theme != ETheme.NIGHTCLUB)
            {
                Console.WriteLine("WARNING: Trying to add a DecoGlow to a level that does not use theme NIGHTCLUB!");
            }
            if (typeof(T) == typeof(MetroTunnel) && Theme != ETheme.METRO)
            {
                Console.WriteLine("WARNING: Trying to add a MetroTunnel to a level that does not use theme METRO!");
            }
            T actor = new T
            {
                Position = new Vector2(x, y)
            };
            Actors.Add(actor);
            return actor;
        }

        /// <summary>
        /// Adds a new actor of type <c>Deco</c> to the level.
        /// Unlike <c>AddActor&lt;Deco&gt;</c>, this method also initializes the deco object with given graphics.
        /// </summary>
        /// <param name="x">The x-position of the deco actor in units (1/16 tiles).</param>
        /// <param name="y">The y-position of the deco actor in units (1/16 tiles).</param>
        /// <param name="graphic">The graphic to initialize the deco object with.</param>
        /// <returns>The newly added deco actor.</returns>
        public Deco AddDeco(float x, float y, Graphic graphic)
        {
            EBundle bundle = Bundles.FromName(graphic.Bundle);
            if (
                bundle != Bundles.FromTheme(Themes.FromName(ThemeStr)) &&
                bundle <= EBundle.LABORATORY
                )
            {
                Console.WriteLine("WARNING: Trying to add a deco from a bundle that doesn't match the level's bundle!");
            }
            if (
                graphic.Type != EGraphicType.IMAGE &&
                graphic.Type != EGraphicType.ANIMATED_IMAGE &&
                graphic.Type != EGraphicType.SPRITE_IMAGE &&
                graphic.Type != EGraphicType.MULTI_SPRITE_ATLAS &&
                graphic.Type != EGraphicType.SPRITE_ATLAS)
            {
                Console.WriteLine("WARNING: Content type of a deco must either be IMAGE, ANIMATED_IMAGE, SPRITE_IMAGE, MULTI_SPRITE_ATLAS or SPRITE_ATLAS!");
            }
            Deco deco = AddActor<Deco>(x, y);
            deco.SetGraphic(graphic);
            return deco;
        }

        /// <summary>
        /// Adds a new actor of type <c>EditableSoundEmitter</c> to the level.
        /// Unlike <c>AddActor&lt;EditableSoundEmitter&gt;</c>, this method also initializes the deco object with given sound.
        /// </summary>
        /// <param name="x">The x-position of the sound emitter actor in units (1/16 tiles).</param>
        /// <param name="y">The y-position of the sound emitter actor in units (1/16 tiles).</param>
        /// <param name="sound">The sound to initialize the sound emitter object with.</param>
        /// <returns>The newly added sound emitter actor.</returns>
        public EditableSoundEmitter AddSoundEmitter(float x, float y, Sound sound)
        {
            EBundle bundle = Bundles.FromName(sound.Bundle);
            if (
                bundle != Bundles.FromTheme(Themes.FromName(ThemeStr)) &&
                bundle != EBundle.GAME
                )
            {
                Console.WriteLine("WARNING: Trying to add a deco from a bundle that doesn't match the level's bundle!");
            }
            if (sound.Type != ESoundType.SOUND)
            {
                Console.WriteLine("WARNING: Content type of a deco must be SOUND!");
            }
            EditableSoundEmitter soundEmitter = AddActor<EditableSoundEmitter>(x, y);
            soundEmitter.SFX = sound.SoundLabel;
            return soundEmitter;
        }

        /// <summary>
        /// Adds a new actor of type <c>Checkpoint</c> to the level.
        /// Unlike <c>AddActor&lt;Checkpoint&gt;</c>, this method also correctly initializes the checkpoint's ID and
        /// its predecessors' next IDs.
        /// </summary>
        /// <param name="x">The x-position of the checkpoint actor in units (1/16 tiles).</param>
        /// <param name="y">The y-position of the checkpoint actor in units (1/16 tiles).</param>
        /// <param name="predecessors">The predecessor checkpoints.</param>
        /// <param name="helper">Whether the checkpoint should be a helper or not.</param>
        /// <param name="startpoint">Whether the checkpoint should be the startpoint or not.</param>
        /// <returns>The newly added sound emitter actor.</returns>
        public Checkpoint AddCheckpoint(float x, float y, IEnumerable<Checkpoint> predecessors, bool helper = false, bool startpoint = false)
        {
            IEnumerable<Checkpoint> checkpoints = GetActorsOfType<Checkpoint>();
            if (startpoint && checkpoints.Any(c => c.IsStartpoint))
            {
                Console.WriteLine("WARNING: The level already contains a startpoint checkpoint!");
            }
            int maxID = checkpoints.Any() ? checkpoints.Max(c => c.ID) : 0;
            Checkpoint checkpoint = AddActor<Checkpoint>(x, y);
            checkpoint.ID = maxID + 1;
            checkpoint.IsHelper = helper;
            checkpoint.IsStartpoint = startpoint;
            if (predecessors != null)
            {
                foreach (Checkpoint predecessor in predecessors)
                    predecessor.NextIDs = predecessor.NextIDs.Append(checkpoint.ID);
            }
            return checkpoint;
        }

        /// <summary>
        /// Connects two checkpoints by adding a connection from <c>from</c> to <c>to</c>.
        /// </summary>
        /// <param name="from">The predecessor.</param>
        /// <param name="to">The successor.</param>
        public void CheckpointConnect(Checkpoint from, Checkpoint to)
        {
            from.NextIDs = from.NextIDs.Append(to.ID);
        }

        /// <summary>
        /// Renames a trigger ID by applying this change to all triggers and triggerable actors, which are
        /// <c>Trigger</c>, <c>Switch</c>, <c>SwitchBlock</c>, <c>Checkpoint</c>, <c>AIVolume</c>, <c>Deco</c>, <c>EditableSoundEmitter</c>, <c>TriggerSaw</c> and <c>Dove</c>.
        /// </summary>
        /// <param name="oldID">The old trigger ID to rename.</param>
        /// <param name="newID">The new trigger ID.</param>
        public void RenameTriggerID(string oldID, string newID)
        {
            foreach (Actor actor in Actors)
            {
                if (actor is ITriggerable triggerable)
                {
                    if (triggerable.TriggerID == oldID)
                        triggerable.TriggerID = newID;
                }
            }
        }
    }
}
