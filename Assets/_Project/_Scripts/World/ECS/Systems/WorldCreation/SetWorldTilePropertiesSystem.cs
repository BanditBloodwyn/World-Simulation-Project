using Assets._Project._Scripts.World.Data.Structs.WorldTile;
using Assets._Project._Scripts.World.ECS.Aspects;
using Assets._Project._Scripts.World.ECS.Components.WorldCreator;
using Assets._Project._Scripts.World.ECS.Components.WorldTile;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Assets._Project._Scripts.World.ECS.Systems
{
    [UpdateAfter(typeof(SetWorldTileMaterialsSystem))]
    [BurstCompile]
    public partial struct SetWorldTilePropertiesSystem : ISystem
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
            EntityCommandBuffer entityCommandBuffer = new(Allocator.Temp);
            
            Entity worldTileHeightsMinMaxEntity = SystemAPI.GetSingletonEntity<WorldTileHeightsMinMaxComponent>();
            WorldMinMaxAspect minMaxAspect = SystemAPI.GetAspectRW<WorldMinMaxAspect>(worldTileHeightsMinMaxEntity);

            Entity worldPropertiesEntity = SystemAPI.GetSingletonEntity<WorldPropertiesComponent>();
            WorldAspect worldAspect = SystemAPI.GetAspectRW<WorldAspect>(worldPropertiesEntity);
            
            foreach (WorldTileAspect tile in SystemAPI.Query<WorldTileAspect>())
            {
                entityCommandBuffer.AddComponent(
                    tile.Entity,
                    new WorldTileProperties()
                    {
                        VegetationZone = worldAspect.GetVegetationZone(minMaxAspect.GetInterpolation(tile.Height)),
                        TerrainValues = new TerrainValues
                        {

                        },
                        FloraValues = new FloraValues
                        {

                        },
                        FaunaValues = new FaunaValues
                        {

                        },
                        PopulationValues = new PopulationValues
                        {

                        }
                    });
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}