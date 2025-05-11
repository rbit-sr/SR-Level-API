using System;
using System.Linq;

namespace SRL
{
    public class Test
    {
        public static void Main()
        {
            Level metroLevel = Level.ReadOfficial(EOfficial.METRO);

            int width = metroLevel.GetTileLayer(ELayer.COLLISION).Width;
            int height = metroLevel.GetTileLayer(ELayer.COLLISION).Width;

            Obstacle box = metroLevel.GetActorsOfType<Obstacle>().First();
            box.Position = new Vector2(100.0f, 300.0f);

            BoostSection boost = metroLevel.AddActor<BoostSection>(500.0f, 800.0f);
            boost.Rotation = BoostSection.ERotation.DEGREE_45;

            metroLevel.Actors.RemoveAll(actor => actor is SuperBoostVolume);

            metroLevel.WriteLocal("Metro modified");

            Level newLevel = new Level(ETheme.LIBRARY, tilesW: 1000, tilesH: 800);

            TileLayer collision = newLevel.GetTileLayer(ELayer.COLLISION);
            collision.Fill(tilesX: 100, tilesY: 50, tilesW: 300, tilesH: 200, TileLayer.COL_FULL);

            Deco deco = newLevel.AddDeco(100.0f, 400.0f, Bundles.Library.Pillar_front);
            deco.Flipped = true;
            deco.AnimationType = Deco.EAnimationType.SPAWNER;
            deco.Lifetime = 1.0f;
            deco.SpawnInterval = 0.3f;

            newLevel.AddActor<Bookcase>(500.0f, 100.0f);

            newLevel.AddSoundEmitter(600.0f, 100.0f, Bundles.Library.amb_library_clockworks);

            newLevel.Actors.RemoveAll(actor => actor is Checkpoint);
            Checkpoint cp0 = newLevel.AddCheckpoint(200.0f, 200.0f, predecessors: null, startpoint: true);
            Checkpoint cp1 = newLevel.AddCheckpoint(600.0f, 200.0f, predecessors: new[] { cp0 });
            Checkpoint cp2 = newLevel.AddCheckpoint(600.0f, 600.0f, predecessors: new[] { cp1 });
            Checkpoint cp3 = newLevel.AddCheckpoint(200.0f, 600.0f, predecessors: new[] { cp2 });
            newLevel.CheckpointConnect(cp3, cp0);

            newLevel.WriteLocal("new level");

            Console.Read();
        }
    }
}
