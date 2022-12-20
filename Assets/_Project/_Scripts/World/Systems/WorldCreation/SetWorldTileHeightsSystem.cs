using Assets._Project._Scripts.World.Aspects;
using Assets._Project._Scripts.World.Components.WorldCreator;
using Assets._Project._Scripts.World.Generators.Noise;
using Unity.Entities;

namespace Assets._Project._Scripts.World.Systems
{
    public partial struct SetWorldTileHeightsSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            PerlinNoiseEvaluator perlinNoiseEvaluator = new PerlinNoiseEvaluator(0);
            HeightGeneratorComponent heightGenerator = SystemAPI.GetSingleton<HeightGeneratorComponent>();

            foreach (WorldTileAspect tile in SystemAPI.Query<WorldTileAspect>())
                tile.MoveToHeight(perlinNoiseEvaluator, heightGenerator);
        }
    }
}