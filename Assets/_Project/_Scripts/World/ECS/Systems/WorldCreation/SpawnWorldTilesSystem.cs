﻿using Assets._Project._Scripts.Gameplay.SelectionSystem.Components;
using Assets._Project._Scripts.World.ECS.Aspects;
using Assets._Project._Scripts.World.ECS.Components.WorldCreator;
using Assets._Project._Scripts.World.ECS.Components.WorldTile;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets._Project._Scripts.World.ECS.Systems
{
    [BurstCompile]
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

            EntityCommandBuffer entityCommandBuffer = new(Allocator.Temp);

            for (int x = 0; x < worldAspect.WorldSize; x++)
            {
                for (int z = 0; z < worldAspect.WorldSize; z++)
                {
                    Entity newWorldTile = entityCommandBuffer.Instantiate(worldAspect.WorldTilePrefab);

                    entityCommandBuffer.AddComponent(
                        newWorldTile,
                        new WorldTileHeightComponent());
                    entityCommandBuffer.SetComponent(
                        newWorldTile,
                        new Translation { Value = new float3(x, 0, z) });
                    entityCommandBuffer.AddComponent(
                        newWorldTile,
                        new NonUniformScale { Value = new float3(1, 3, 1) });
                    entityCommandBuffer.AddComponent(
                        newWorldTile,
                        new SelectableEntityTag());
                }
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}