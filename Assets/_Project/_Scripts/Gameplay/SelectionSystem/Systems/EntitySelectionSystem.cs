using Assets._Project._Scripts.Gameplay.SelectionSystem.Aspects;
using Assets._Project._Scripts.Gameplay.SelectionSystem.Components;
using Assets._Project._Scripts.World.ECS.Aspects;
using Assets._Project._Scripts.World.ECS.Components.WorldCreator;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets._Project._Scripts.Gameplay.SelectionSystem.Systems
{
    [BurstCompile]
    public partial struct EntitySelectionSystem : ISystem
    {
        public bool _initialised;
        private NativeParallelHashMap<Bounds, Entity> _tileHashMap;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SelectableEntityTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!_initialised)
            {
                CreateSelectionSystemData(ref state);
            }

            EntityCommandBuffer ecb = new(Allocator.Temp);

            foreach (DynamicBuffer<SelectionInputBufferElement> dynamicBuffer in SystemAPI.Query<DynamicBuffer<SelectionInputBufferElement>>())
            {
                foreach (SelectionInputBufferElement input in dynamicBuffer)
                    SampleAlongRay(input, ecb, ref state);

                dynamicBuffer.Clear();
            }

            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        private void CreateSelectionSystemData(ref SystemState state)
        {
            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldPropertiesComponent>();
            WorldAspect worldAspect = SystemAPI.GetAspectRW<WorldAspect>(worldPropertiesEntity);

            _tileHashMap = new NativeParallelHashMap<Bounds, Entity>(
                worldAspect.WorldSize * worldAspect.WorldSize,
                Allocator.Persistent);

            foreach (SelectableEntityAspect tile in SystemAPI.Query<SelectableEntityAspect>()) 
                _tileHashMap.Add(
                    tile.CreateBounds(),
                    tile.Entity);
           
            state.EntityManager.AddComponent<SelectionSystemData>(state.SystemHandle);
          
            SelectionSystemData selectionSystemData = new SelectionSystemData { TilePositions = _tileHashMap.GetKeyArray(Allocator.Persistent) };
            SystemAPI.SetComponent(state.SystemHandle, selectionSystemData);

            _initialised = true;
        }

        [BurstCompile]
        private void SampleAlongRay(SelectionInputBufferElement input, EntityCommandBuffer ecb, ref SystemState state)
        {
            const float sampleDistance = 0.1f;
            float3 currentSamplePosition = input.Value.Start;
            float3 rayDirection = ((Vector3)(input.Value.End - input.Value.Start)).normalized;

            while (currentSamplePosition.y > 0)
            {
                foreach (KeyValue<Bounds, Entity> keyValue in _tileHashMap)
                {
                    if (keyValue.Key.Contains(currentSamplePosition))
                    {
                        ecb.SetComponent(
                            keyValue.Value,
                            new Translation { Value = keyValue.Key.center + (Vector3)math.up() });
                    }
                }

                currentSamplePosition += rayDirection * sampleDistance;
            }
        }
    }
}