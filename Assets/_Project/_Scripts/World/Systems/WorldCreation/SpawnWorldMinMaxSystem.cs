using Assets._Project._Scripts.Core.Extentions;
using Assets._Project._Scripts.Core.Types;
using Assets._Project._Scripts.World.Aspects;
using Assets._Project._Scripts.World.Components.WorldCreator;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Assets._Project._Scripts.World.Systems
{
    [UpdateAfter(typeof(SetWorldTileHeightsSystem))]
    [BurstCompile]
    public partial struct SpawnWorldMinMaxSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            SimpleTuple<float, float> minMax = GetMinMaxHeights(ref state);

            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

            Entity worldTileHeightsMinMaxEntity = entityCommandBuffer.CreateEntity();

            entityCommandBuffer.SetName(worldTileHeightsMinMaxEntity, new FixedString64Bytes("WorldMinMax"));
            entityCommandBuffer.AddComponent(
                worldTileHeightsMinMaxEntity,
                new WorldTileHeightsMinMaxComponent
                {
                    MinimumTileHeight = minMax.Value1,
                    MaximumTileHeight = minMax.Value2
                });

            entityCommandBuffer.Playback(state.EntityManager);
        }

        private SimpleTuple<float, float> GetMinMaxHeights(ref SystemState state)
        {
            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldPropertiesComponent>();
            WorldAspect worldAspect = SystemAPI.GetAspectRW<WorldAspect>(worldPropertiesEntity);

            NativeArray<float> heights = new(worldAspect.WorldSize * worldAspect.WorldSize, Allocator.Temp);

            foreach (WorldTileAspect tile in SystemAPI.Query<WorldTileAspect>())
                heights[(int)tile.Z * worldAspect.WorldSize + (int)tile.X] = tile.Height;

            float minValue = heights.BurstMin();
            float maxValue = heights.BurstMax();

            return new SimpleTuple<float, float>(minValue, maxValue);
        }
    }
}