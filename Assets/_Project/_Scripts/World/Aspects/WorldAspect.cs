using Assets._Project._Scripts.World.Components;
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
        private readonly RefRW<HeightGeneratorComponent> _heightGenerator;

        public int WorldSize => _worldProperties.ValueRO.WorldSize;

        public Entity WorldTilePrefab => _worldProperties.ValueRO.WorldTilePrefab;
        
        public static UniformScaleTransform GetWorldTileTransform(float x, float y, float z)
        {
            return new UniformScaleTransform
            {
                Position = new float3(x, y, z),
                Rotation = new quaternion(),
                Scale = 1f
            };
        }
    }
}