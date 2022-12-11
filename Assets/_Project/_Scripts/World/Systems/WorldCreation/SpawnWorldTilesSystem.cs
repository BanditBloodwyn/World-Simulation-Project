using System;
using System.Linq;
using Assets._Project._Scripts.World.Aspects;
using Assets._Project._Scripts.World.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets._Project._Scripts.World.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnWorldTilesSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WorldPropertiesComponent>();
        }

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            // to make sure this spawning only happens once
            state.Enabled = false;

            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldPropertiesComponent>();
            WorldAspect worldAspect = SystemAPI.GetAspectRW<WorldAspect>(worldPropertiesEntity);


            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

            NativeArray<TileHeight> heights = new NativeArray<TileHeight>(worldAspect.WorldSize * worldAspect.WorldSize, Allocator.Temp);
            GetTileHeights(ref worldAspect, ref heights);

            float minValue = heights.Min(static height => height.Height);
            float maxValue = heights.Max(static height => height.Height);

            foreach (TileHeight tileHeight in heights)
            {
                Entity newWorldTile = entityCommandBuffer.Instantiate(worldAspect.WorldTilePrefab);

                UniformScaleTransform newWorldTileTransform = WorldAspect.GetWorldTileTransform(
                    tileHeight.Coordinates.x,
                    tileHeight.Coordinates.y,
                    tileHeight.Height);
                entityCommandBuffer.SetComponent(newWorldTile, new LocalToWorldTransform { Value = newWorldTileTransform });

                float interpolation = GetInterpolation(tileHeight.Height, minValue, maxValue);
                entityCommandBuffer.AddComponent(
                    newWorldTile,
                    new WorldMaterialOverride { Value = interpolation });
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }

        private static void GetTileHeights(ref WorldAspect worldAspect, ref NativeArray<TileHeight> heights)
        {
            for (int x = 0; x < worldAspect.WorldSize; x++)
            {
                for (int y = 0; y < worldAspect.WorldSize; y++)
                {
                    float randomTileProperty = worldAspect.GetNoisedTileProperty(x, y);

                    heights[y * worldAspect.WorldSize + x] = (new TileHeight { Coordinates = new int2(x, y), Height = randomTileProperty });
                }
            }
        }

        private static float GetInterpolation(float randomTileProperty, float minValue, float maxValue)
        {
            return 100 / (maxValue - minValue) * (randomTileProperty - minValue) / 100;
        }
    }
}