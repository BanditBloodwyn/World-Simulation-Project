using System.Linq;
using Assets._Project._Scripts.Core.Extentions;
using Assets._Project._Scripts.Core.Types;
using Assets._Project._Scripts.World.Aspects;
using Assets._Project._Scripts.World.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Assets._Project._Scripts.World.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SetWorldTileHeightsSystem))]
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

            SimpleTuple<float, float> minMax = GetMinMaxHeights(ref state);

            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

            foreach (WorldTileAspect tile in SystemAPI.Query<WorldTileAspect>())
            {
                float interpolation = GetInterpolation(tile.Y, minMax.Value1, minMax.Value2);
                entityCommandBuffer.AddComponent(
                    tile.Entity,
                    new WorldMaterialOverride { Value = interpolation });
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }

        [BurstCompile]
        private SimpleTuple<float, float> GetMinMaxHeights(ref SystemState state)
        {
            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldPropertiesComponent>();
            WorldAspect worldAspect = SystemAPI.GetAspectRW<WorldAspect>(worldPropertiesEntity);

            NativeArray<float> heights = new NativeArray<float>(worldAspect.WorldSize * worldAspect.WorldSize, Allocator.Temp);

            foreach (WorldTileAspect tile in SystemAPI.Query<WorldTileAspect>())
                heights[(int)tile.Z * worldAspect.WorldSize + (int)tile.X] = tile.Y;

            float minValue = heights.BurstMin();
            float maxValue = heights.BurstMax();

            return new SimpleTuple<float, float>(minValue, maxValue);
        }

        private static float GetInterpolation(float randomTileProperty, float minValue, float maxValue)
        {
            return 100 / (maxValue - minValue) * (randomTileProperty - minValue) / 100;
        }
    }
}