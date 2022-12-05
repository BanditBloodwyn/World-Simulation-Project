using Assets._Project._Scripts.World.Aspects;
using Assets._Project._Scripts.World.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace Assets._Project._Scripts.World.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnWorldTilesSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WorldPropertiesComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // to make sure this spawning only happens once
            state.Enabled = false;

            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldPropertiesComponent>();
            WorldAspect worldAspect = SystemAPI.GetAspectRW<WorldAspect>(worldPropertiesEntity);


            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

            float minValue = GetMinimumValue(worldAspect);
            float maxValue = GetMaximumValue(worldAspect);

            for (int x = 0; x < worldAspect.WorldSize; x++)
            {
                for (int y = 0; y < worldAspect.WorldSize; y++)
                {
                    Entity newWorldTile = entityCommandBuffer.Instantiate(worldAspect.WorldTilePrefab);

                    float randomTileProperty = worldAspect.GetNoisedTileProperty(x, y);

                    if (randomTileProperty > maxValue)
                        maxValue = randomTileProperty;

                    UniformScaleTransform newWorldTileTransform = WorldAspect.GetWorldTileTransform(
                        x,
                        y,
                        randomTileProperty - minValue);
                    entityCommandBuffer.SetComponent(newWorldTile, new LocalToWorldTransform { Value = newWorldTileTransform });

                    float interpolation = GetInterpolation(randomTileProperty, minValue, maxValue);
                    entityCommandBuffer.SetComponent(
                        newWorldTile,
                        new URPMaterialPropertyBaseColor
                        {
                            Value = new float4(
                                0,
                                interpolation,
                                0,
                                1)
                        });
                }
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }

        private static float GetInterpolation(float randomTileProperty, float minValue, float maxValue)
        {
            return 100 / (maxValue - minValue) * (randomTileProperty - minValue) / 100;
        }

        private static float GetMinimumValue(WorldAspect worldAspect)
        {
            float minValue = float.PositiveInfinity;

            for (int x = 0; x < worldAspect.WorldSize; x++)
            {
                for (int y = 0; y < worldAspect.WorldSize; y++)
                {
                    float randomTileProperty = worldAspect.GetNoisedTileProperty(x, y);

                    if (randomTileProperty < minValue)
                        minValue = randomTileProperty;
                }
            }

            return minValue;
        }

        private static float GetMaximumValue(WorldAspect worldAspect)
        {
            float maxValue = float.NegativeInfinity;

            for (int x = 0; x < worldAspect.WorldSize; x++)
            {
                for (int y = 0; y < worldAspect.WorldSize; y++)
                {
                    float randomTileProperty = worldAspect.GetNoisedTileProperty(x, y);

                    if (randomTileProperty > maxValue)
                        maxValue = randomTileProperty;
                }
            }

            return maxValue;
        }
    }
}