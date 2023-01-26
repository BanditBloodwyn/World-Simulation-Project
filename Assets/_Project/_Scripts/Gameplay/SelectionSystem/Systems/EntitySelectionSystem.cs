using Assets._Project._Scripts.Gameplay.SelectionSystem.Aspects;
using Assets._Project._Scripts.World.ECS.Components.WorldCreator;
using Assets._Project._Scripts.World.ECS.Components.WorldTile;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using WorldAspect = Assets._Project._Scripts.World.ECS.Aspects.WorldAspect;

namespace Assets._Project._Scripts.Gameplay.SelectionSystem.Systems
{
    [BurstCompile]
    public partial struct EntitySelectionSystem : ISystem
    {
        public NativeParallelHashMap<float3, Entity> _tileHashMap;
        public bool _initialised;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WorldTileProperties>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!_initialised) 
                CreateTileHashMap(ref state);

            EntityCommandBuffer entityCommandBuffer = new(Allocator.Temp);

            foreach (DynamicBuffer<SelectionInputBufferElement> dynamicBuffer in SystemAPI.Query<DynamicBuffer<SelectionInputBufferElement>>())
            {
                foreach (SelectionInputBufferElement input in dynamicBuffer)
                {
                    if (SampleAlongRay(input, ref state, out Entity tile))
                    {
                        UnityEngine.Debug.Log("<color=green>Tile found</color>");
                        entityCommandBuffer.SetComponent(tile, new NonUniformScale { Value = new float3(1.1f, 3.1f, 1.1f) });
                    }
                    else
                        UnityEngine.Debug.Log("<color=red>No tile found</color>");
                }

                dynamicBuffer.Clear();
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }

        [BurstCompile]
        private void CreateTileHashMap(ref SystemState state)
        {
            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldPropertiesComponent>();
            WorldAspect worldAspect = SystemAPI.GetAspectRW<WorldAspect>(worldPropertiesEntity);

            _tileHashMap = new NativeParallelHashMap<float3, Entity>(
                worldAspect.WorldSize * worldAspect.WorldSize,
                Allocator.Persistent);

            foreach (SelectableEntityAspect tile in SystemAPI.Query<SelectableEntityAspect>())
            {
                float3 float3 = math.trunc(tile.Position);
                _tileHashMap.Add(float3, tile.Entity);
            }
            _initialised = true;
        }

        [BurstCompile]
        private bool SampleAlongRay(SelectionInputBufferElement input, ref SystemState state, out Entity tile)
        {
            float sampleDistance = 0.1f;
            float3 currentSamplePosition = input.Value.Start;
            float3 rayDirection = ((Vector3)(input.Value.End - input.Value.Start)).normalized;

            while (currentSamplePosition.y > -100)
            {
                float3 roundedSamplePosition = math.trunc(currentSamplePosition);
               
                if(_tileHashMap.TryGetValue(roundedSamplePosition, out tile))
                {
                    tile = FindClosestTileAroundThis(currentSamplePosition, roundedSamplePosition, ref state);
                    return tile != Entity.Null;
                }               
                currentSamplePosition += rayDirection * sampleDistance;
            }

            tile = Entity.Null;

            return false;
        }

        [BurstCompile]
        private Entity FindClosestTileAroundThis(float3 currentSamplePosition, float3 roundedSamplePosition, ref SystemState state)
        {
            NativeParallelHashMap<float3, Entity> map = new NativeParallelHashMap<float3, Entity>(9, Allocator.Temp);

            if(_tileHashMap.TryGetValue(roundedSamplePosition + new int3(1, 0, 1), out Entity tile1)) 
                map.Add(roundedSamplePosition + new int3(1, 0, 1), tile1);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(0, 0, 1), out Entity tile2))
                map.Add(roundedSamplePosition + new int3(0, 0, 1), tile2);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(-1, 0, 1), out Entity tile3))
                map.Add(roundedSamplePosition + new int3(-1, 0, 1), tile3);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(1, 0, 1), out Entity tile4))
                map.Add(roundedSamplePosition + new int3(1, 0, 0), tile4);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(0, 0, 1), out Entity tile5))
                map.Add(roundedSamplePosition + new int3(0, 0, 0), tile5);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(-1, 0, 1), out Entity tile6))
                map.Add(roundedSamplePosition + new int3(-1, 0, 0), tile6);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(1, 0, 1), out Entity tile7))
                map.Add(roundedSamplePosition + new int3(1, 0, -1), tile7);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(0, 0, 1), out Entity tile8))
                map.Add(roundedSamplePosition + new int3(0, 0, -1), tile8);
            if (_tileHashMap.TryGetValue(roundedSamplePosition + new int3(-1, 0, 1), out Entity tile9))
                map.Add(roundedSamplePosition + new int3(-1, 0, -1), tile9);

            Entity closestEntity = Entity.Null;
            float closestDistance = float.PositiveInfinity;

            foreach (KeyValue<float3, Entity> keyValue in map)
            {
                float3 realEntityPosition = SystemAPI.GetComponent<Translation>(keyValue.Value).Value;
                float currentDistance = math.distance(realEntityPosition, currentSamplePosition);

                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestEntity = keyValue.Value;
                }
            }

            map.Dispose();
            return closestEntity;
        }
    }
}