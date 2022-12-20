using Assets._Project._Scripts.World.Aspects;
using Assets._Project._Scripts.World.Components.WorldCreator;
using Assets._Project._Scripts.World.Components.WorldTile;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Assets._Project._Scripts.World.Systems
{
    [UpdateAfter(typeof(SpawnWorldMinMaxSystem))]
    [BurstCompile]
    public partial struct SetWorldTileMaterialsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldTileHeightsMinMaxComponent>();
            WorldMinMaxAspect minMaxAspect = SystemAPI.GetAspectRW<WorldMinMaxAspect>(worldPropertiesEntity);

            EntityCommandBuffer entityCommandBuffer = new(Allocator.Temp);

            foreach (WorldTileAspect tile in SystemAPI.Query<WorldTileAspect>())
            {
                float interpolation = minMaxAspect.GetInterpolation(tile.Height);
                entityCommandBuffer.AddComponent(
                    tile.Entity,
                    new VegetationZoneMaterialOverride { Value = interpolation });
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}