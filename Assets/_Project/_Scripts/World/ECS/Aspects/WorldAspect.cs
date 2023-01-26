using Assets._Project._Scripts.World.Data;
using Assets._Project._Scripts.World.Data.Enums;
using Assets._Project._Scripts.World.ECS.Components.WorldCreator;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Assets._Project._Scripts.World.ECS.Aspects
{
    public readonly partial struct WorldAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;

        private readonly RefRO<WorldPropertiesComponent> _worldProperties;

        public int WorldSize => _worldProperties.ValueRO.WorldSize;

        public Entity WorldTilePrefab => _worldProperties.ValueRO.WorldTilePrefab;
        
        public NativeArray<VegetationZoneHeights> VegetationZoneHeights => _worldProperties.ValueRO.VegetationZoneHeights;

        public VegetationZones GetVegetationZone(float tileHeight)
        {
            foreach (VegetationZoneHeights vegetationZoneHeight in VegetationZoneHeights)
            {
                if (tileHeight <= vegetationZoneHeight.MaximumHeight / 100)
                    return vegetationZoneHeight.VegetationZone;
            }

            return VegetationZones.Water;
        }
    }
}