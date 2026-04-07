using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace SRL
{
    /// <summary>
    /// Layer enumerator.
    /// </summary>
    public enum ELayer
    {
        DEFAULT_UI_LAYER,
        VERSION_NUMBER_LAYER,
        POPUP_LAYER,
        TOP_UI_LAYER,
        USER_UI_LAYER,
        CURSOR_UI_LAYER,
        BACKGROUND_LAYER_0,
        BACKGROUND_LAYER_1,
        BACKGROUND_LAYER_2,
        BACKGROUND_LAYER_3,
        BACKGROUND_LAYER_4,
        BACKGROUND_LAYER_5,
        NON_PARALLAXING_BACK_LAYER,
        PARALLAX_LAYER_0_800,
        PARALLAX_LAYER_0_825,
        PARALLAX_LAYER_0_850,
        PARALLAX_LAYER_0_875,
        PARALLAX_LAYER_0_900,
        PARALLAX_LAYER_0_925,
        PARALLAX_LAYER_0_950,
        PARALLAX_LAYER_0_975,
        BACK_OBJECT_LAYER,
        BACKGROUND_0,
        BACKGROUND_1,
        MIDDLE_OBJECT_LAYER,
        SHADING,
        OVERLAY,
        GAMEPLAY_OBJECTS_LAYER,
        COLLISION,
        OBJECT_LAYER,
        TRAIL_BEHIND_REMOTE_PLAYERS_LAYER,
        REMOTE_PLAYERS_LAYER,
        TRAIL_IN_FRONT_OF_REMOTE_PLAYERS_LAYER,
        TRAIL_BEHIND_LOCAL_PLAYERS_LAYER,
        LOCAL_PLAYERS_LAYER,
        TRAIL_IN_FRONT_OF_LOCAL_PLAYERS_LAYER,
        TILE_PREVIEW,
        TEMP
    }

    /// <summary>
    /// Provides utility methods for the <c>ELayer</c> enum.
    /// </summary>
    public static class Layers
    {
        /// <summary>
        /// Names for each layer as stored in <c>TileLayer.LayerStr</c> sorted by index.
        /// </summary>
        public static readonly string[] LayerNames =
        {
            "DefaultUILayer",
            "VersionNumberLayer",
            "PopupLayer",
            "TopUILayer",
            "UserUILayer",
            "CursorUILayer",
            "BackgroundLayer0",
            "BackgroundLayer1",
            "BackgroundLayer2",
            "BackgroundLayer3",
            "BackgroundLayer4",
            "BackgroundLayer5",
            "NonParallaxingBackLayer",
            "ParallaxLayer: 0.800",
            "ParallaxLayer: 0.825",
            "ParallaxLayer: 0.850",
            "ParallaxLayer: 0.875",
            "ParallaxLayer: 0.900",
            "ParallaxLayer: 0.925",
            "ParallaxLayer: 0.950",
            "ParallaxLayer: 0.975",
            "BackObjectLayer",
            "Background 0",
            "Background 1",
            "MiddleObjectLayer",
            "Shading",
            "Overlay",
            "GameplayObjectsLayer",
            "Collision",
            "ObjectLayer",
            "TrailBehindRemotePlayersLayer",
            "RemotePlayersLayer",
            "TrailInFrontOfRemotePlayersLayer",
            "TrailBehindLocalPlayersLayer",
            "LocalPlayersLayer",
            "TrailInFrontOfLocalPlayersLayer",
            "tilePreview",
            "temp"
        };

        /// <summary>
        /// Lists all layers.
        /// </summary>
        public static IEnumerable<ELayer> AllLayers => Enum.GetValues(typeof(ELayer)).Cast<ELayer>();

        /// <summary>
        /// Converts an <c>ELayer</c> enum to its corresponding name.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>The layer's name.</returns>
        public static string ToName(this ELayer layer)
        {
            return LayerNames[(int)layer];
        }

        /// <summary>
        /// Converts a layer's name back to its corresponding <c>ELayer</c> enum.
        /// </summary>
        /// <param name="layerName">The layer's name.</param>
        /// <returns>The layer.</returns>
        public static ELayer FromName(string layerName)
        {
            return (ELayer)Array.IndexOf(LayerNames, layerName);
        }

        /// <summary>
        /// Determines whether the given layer is a tile layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>True if the given layer is a tile layer.</returns>
        public static bool IsTileLayer(this ELayer layer)
        {
            return
                layer == ELayer.COLLISION ||
                layer == ELayer.SHADING ||
                layer == ELayer.OVERLAY ||
                layer == ELayer.BACKGROUND_0 ||
                layer == ELayer.BACKGROUND_1;
        }

        /// <summary>
        /// Gets the tile size for a given tile layer with given theme.
        /// Multiplying the result by 16 yields the size in units.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="theme">The theme.</param>
        /// <returns>The corresponding tile size. 0 if the given layer is not a tile layer.</returns>
        public static int TileSize(this ELayer layer, ETheme theme)
        {
            if (layer == ELayer.COLLISION || layer == ELayer.SHADING)
                return 1;
            if (layer == ELayer.OVERLAY)
                return 4;
            if (layer == ELayer.BACKGROUND_0 || layer == ELayer.BACKGROUND_1)
            {
                switch (theme)
                {
                    case ETheme.SS_ROYALE:
                    case ETheme.PLAZA:
                    case ETheme.FACTORY:
                    case ETheme.POWERPLANT:
                    case ETheme.SILO:
                    case ETheme.NIGHTCLUB:
                    case ETheme.ZOO:
                    case ETheme.SWIFT_PEAKS:
                    case ETheme.CASINO:
                    case ETheme.FESTIVAL:
                    case ETheme.RESORT:
                    case ETheme.AIRPORT:
                    case ETheme.LABORATORY:
                        return 2;
                    case ETheme.METRO:
                    case ETheme.MANSION:
                    case ETheme.THEME_PARK:
                    case ETheme.LIBRARY:
                        return 1;
                }
            }
            return 0;
        }
    }

    /// <summary>
    /// Represents a tile layer as part of a level.
    /// Values are exactly the same and stored in exactly the same way as in the level file format.
    /// </summary>
    public class TileLayer
    {
        /// <summary>
        /// empty collision tile
        /// </summary>
        public static readonly int COL_EMPTY = 0;

        /// <summary>
        /// full collision tile
        /// </summary>
        public static readonly int COL_FULL = 1;

        /// <summary>
        /// wall collision tile, dots on the left
        /// </summary>
        public static readonly int COL_WALL_LEFT = 2;

        /// <summary>
        /// grapple ceiling collision tile
        /// </summary>
        public static readonly int COL_GRAPPLE_CEIL = 3;

        /// <summary>
        /// wall collision tile, dots on the right
        /// </summary>
        public static readonly int COL_WALL_RIGHT = 4;

        /// <summary>
        /// checkered collision tile
        /// </summary>
        public static readonly int COL_CHECKERED = 5;

        /// <summary>
        /// floor slope collision tile, upper tip on the right
        /// </summary>
        public static readonly int COL_FLOOR_SLOPE_RIGHT = 6;

        /// <summary>
        /// floor slope collision tile, upper tip on the left
        /// </summary>
        public static readonly int COL_FLOOR_SLOPE_LEFT = 7;

        /// <summary>
        /// stairs collision tile, upper tip on the right
        /// </summary>
        public static readonly int COL_STAIRS_RIGHT = 8;

        /// <summary>
        /// stairs collision tile, upper tip on the left
        /// </summary>
        public static readonly int COL_STAIRS_LEFT = 9;

        /// <summary>
        /// checkered floor collision tile, upper tip on the right
        /// </summary>
        public static readonly int COL_CHECKERED_FLOOR_SLOPE_RIGHT = 10;

        /// <summary>
        /// checkered floor collision tile, upper tip on the left
        /// </summary>
        public static readonly int COL_CHECKERED_FLOOR_SLOPE_LEFT = 11;

        /// <summary>
        /// ceiling slope collision tile, lower tip on the right
        /// </summary>
        public static readonly int COL_CEIL_SLOPE_RIGHT = 12;

        /// <summary>
        /// ceiling slope collision tile, lower tip on the left
        /// </summary>
        public static readonly int COL_CEIL_SLOPE_LEFT = 13;

        /// <summary>
        /// checkered ceiling slope collision tile, lower tip on the right
        /// </summary>
        public static readonly int COL_CHECKERED_CEIL_SLOPE_RIGHT = 14;

        /// <summary>
        /// checkered ceiling slope collision tile, lower tip on the left
        /// </summary>
        public static readonly int COL_CHECKERED_CEIL_SLOPE_LEFT = 15;

        /// <summary>
        /// The layer name.
        /// </summary>
        public string LayerStr;

        /// <summary>
        /// The tile grid as a 2-dimensional array.
        /// </summary>
        public int[,] Tiles;

        /// <summary>
        /// Property for the layer as an <c>ELayer</c> enum.
        /// </summary>
        public ELayer Layer
        {
            get => Layers.FromName(LayerStr);
            set => LayerStr = value.ToName();
        }

        /// <summary>
        /// The tile layer's width in tiles.
        /// </summary>
        public int Width
        {
            get => Tiles.GetLength(0);
            set => Resize(value, Height);
        }

        /// <summary>
        /// The tile layer's height in tiles.
        /// </summary>
        public int Height
        {
            get => Tiles.GetLength(1);
            set => Resize(Width, value);
        }

        /// <summary>
        /// Constructs a new <c>TileLayer</c> from specified layer enum and width and height in tiles.
        /// </summary>
        /// <param name="layer">The layer enum.</param>
        /// <param name="width">The width in tiles.</param>
        /// <param name="height">The height in tiles.</param>
        public TileLayer(ELayer layer, int width, int height)
        {
            Layer = layer;
            Tiles = new int[width, height];
        }

        /// <summary>
        /// Constructs a <c>TileLayer</c> that is read from a <c>BinaryReader</c>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public TileLayer(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// Reads the tile layer from a <c>BinaryReader</c>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader)
        {
            LayerStr = reader.ReadString();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            Tiles = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tiles[x, y] = reader.ReadInt32();
                }
            }
        }

        /// <summary>
        /// Writes the tile layer to a <c>BinaryWriter</c>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(LayerStr);
            int width = Width;
            int height = Height;
            writer.Write(width);
            writer.Write(height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    writer.Write(Tiles[x, y]);
                }
            }
        }

        /// <summary>
        /// Clears the tile layer by setting every tile to <c>0</c>.
        /// </summary>
        public void Clear()
        {
            Tiles = new int[Width, Height];
        }

        /// <summary>
        /// Fills a region of the tile layer with the specified tile ID.
        /// </summary>
        /// <param name="tilesX">The region's x-position.</param>
        /// <param name="tilesY">The region's y-position.</param>
        /// <param name="tilesW">The region's width.</param>
        /// <param name="tilesH">The region's height.</param>
        /// <param name="tileID">The tile ID to fill the region with.</param>
        public void Fill(int tilesX, int tilesY, int tilesW, int tilesH, int tileID)
        {
            for (int x = Math.Max(tilesX, 0); x < Math.Min(tilesX + tilesW, Width); x++)
            {
                for (int y = Math.Max(tilesY, 0); y < Math.Min(tilesY + tilesH, Width); y++)
                {
                    Tiles[x, y] = tileID;
                }
            }
        }

        /// <summary>
        /// Displaces the tile layer.
        /// This operation does not change the tile layer's size and empty regions are filled by <c>0</c>.
        /// </summary>
        /// <param name="tilesX">The horizontal displacement.</param>
        /// <param name="tilesY">The vertical displacement.</param>
        public void Move(int tilesX, int tilesY)
        {
            int shift = tilesY + tilesX * Height;
            if (shift > 0)
            {
                Array.Copy(Tiles, 0, Tiles, shift, Width * Height - shift);
            }
            else
            {
                Array.Copy(Tiles, -shift, Tiles, 0, Width * Height + shift);
            }
            if (tilesX > 0)
            {
                Fill(0, 0, tilesX, Height, 0);
            }
            else if (tilesX < 0)
            {
                Fill(Width - 1 + tilesX, 0, -tilesX, Height, 0);
            }
            if (tilesY > 0)
            {
                Fill(0, 0, Width, tilesY, 0);
            }
            else if (tilesY < 0)
            {
                Fill(0, Height - 1 + tilesY, Width, -tilesY, 0);
            }
        }

        /// <summary>
        /// Resizes the tile layer.
        /// Empty regions are filled by <c>0</c>.
        /// </summary>
        /// <param name="tilesW">The new width.</param>
        /// <param name="tilesH">The new height.</param>
        public void Resize(int tilesW, int tilesH)
        {
            int[,] newTiles = new int[tilesW, tilesH];
            int minW = Math.Min(Width, tilesW);
            int minH = Math.Min(Height, tilesH);

            if (tilesH == Height)
            {
                Array.Copy(Tiles, 0, newTiles, 0, minW * Height);
            }
            else
            {
                for (int x = 0; x < minW; x++)
                    Array.Copy(Tiles, x * Height, newTiles, x * tilesH, minH);
            }
            Tiles = newTiles;
        }
    }
}
