using Assets._Project._Scripts.World.ECS.Components.WorldCreator;
using Assets._Project._Scripts.World.ECS.Components.WorldTile;
using Assets._Project._Scripts.World.Generators.Noise;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets._Project._Scripts.World.ECS.Aspects
{
    public readonly partial struct WorldTileAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transform;
        private readonly RefRO<WorldTileHeightComponent> _tileHeight;

        public float X => _transform.LocalPosition.x;
        public float Height => _transform.LocalPosition.y;
        public float Z => _transform.LocalPosition.z;

        public float3 Position => _transform.Position;

        public void MoveToHeight(PerlinNoiseEvaluator perlinNoiseEvaluator, HeightGeneratorComponent heightGenerator)
        {
            float3 position = _transform.LocalPosition;

            float height = GetNoisedTileProperty(position.x, position.z, perlinNoiseEvaluator, heightGenerator);
            position.y = height;
            _transform.LocalPosition = position;
        }

        private static float GetNoisedTileProperty(float x, float y, PerlinNoiseEvaluator perlinNoiseEvaluator, HeightGeneratorComponent heightGenerator)
        {
            float standardNoisedValues = heightGenerator.StandardNoiseFilterValues.Evaluate(new float3(x, 0, y), perlinNoiseEvaluator);
            float rigidNoisedValues = heightGenerator.RigidNoiseFilterValues.Evaluate(new float3(x, 0, y), perlinNoiseEvaluator) * standardNoisedValues;
            return standardNoisedValues + rigidNoisedValues;
        }
    }
}