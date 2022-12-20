using Assets._Project._Scripts.World.Components.WorldCreator;
using Assets._Project._Scripts.World.Data;
using Assets._Project._Scripts.World.Data.Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets._Project._Scripts.World.Aspects
{
    public readonly partial struct WorldAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;

        private readonly RefRO<WorldPropertiesComponent> _worldProperties;

        public int WorldSize => _worldProperties.ValueRO.WorldSize;

        public Entity WorldTilePrefab => _worldProperties.ValueRO.WorldTilePrefab;
        
        public NativeArray<VegetationZoneHeights> VegetationZoneHeights => _worldProperties.ValueRO.VegetationZoneHeights;

        public static UniformScaleTransform GetWorldTileTransform(float x, float y, float z)
        {
            return new UniformScaleTransform
            {
                Position = new float3(x, y, z),
                Rotation = new quaternion(),
                Scale = 1f
            };
        }

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