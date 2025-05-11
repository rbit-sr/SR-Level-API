using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace SRL
{
    /// <summary>
    /// Represents a layer 
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

    public static class Layers
    {
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

        public static IEnumerable<ELayer> AllLayers => Enum.GetValues(typeof(ELayer)).Cast<ELayer>();

        public static string ToName(this ELayer layer)
        {
            return LayerNames[(int)layer];
        }

        public static ELayer FromName(string layerName)
        {
            return (ELayer)Array.IndexOf(LayerNames, layerName);
        }

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

    public class TileLayer
    {
        public static readonly int COL_EMPTY = 0;
        public static readonly int COL_FULL = 1;
        public static readonly int COL_WALL_LEFT = 2;
        public static readonly int COL_GRAPPLE_CEIL = 3;
        public static readonly int COL_WALL_RIGHT = 4;
        public static readonly int COL_CHECKERED = 5;
        public static readonly int COL_SLOPE_UP_LEFT = 6;
        public static readonly int COL_SLOPE_UP_RIGHT = 7;
        public static readonly int COL_STAIRS_UP_LEFT = 8;
        public static readonly int COL_STAIRS_UP_RIGHT = 9;
        public static readonly int COL_CHECKERED_SLOPE_UP_LEFT = 10;
        public static readonly int COL_CHECKERED_SLOPE_UP_RIGHT = 11;
        public static readonly int COL_SLOPE_DOWN_LEFT = 12;
        public static readonly int COL_SLOPE_DOWN_RIGHT = 13;
        public static readonly int COL_CHECKERED_SLOPE_DOWN_LEFT = 14;
        public static readonly int COL_CHECKERED_SLOPE_DOWN_RIGHT = 15;

        public string LayerStr;
        public int[,] Tiles;

        public ELayer Layer
        {
            get => Layers.FromName(LayerStr);
            set => LayerStr = value.ToName();
        }

        public int Width
        {
            get => Tiles.GetLength(0);
            set => Resize(value, Height);
        }

        public int Height
        {
            get => Tiles.GetLength(1);
            set => Resize(Width, value);
        }

        public TileLayer(ELayer layer, int width, int height)
        {
            Layer = layer;
            Tiles = new int[width, height];
        }

        public TileLayer(BinaryReader reader)
        {
            Read(reader);
        }

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

        public void Clear()
        {
            Tiles = new int[Width, Height];
        }

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
