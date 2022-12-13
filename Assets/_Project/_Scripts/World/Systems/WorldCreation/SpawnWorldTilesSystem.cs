using Assets._Project._Scripts.World.Aspects;
using Assets._Project._Scripts.World.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Assets._Project._Scripts.World.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
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


            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

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
                        new LocalToWorldTransform { Value = WorldAspect.GetWorldTileTransform(x, 0, z) });
                }
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}