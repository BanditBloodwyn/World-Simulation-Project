using Assets._Project._Scripts.World.Components.WorldCreator;
using Assets._Project._Scripts.World.Generators.Noise;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets._Project._Scripts.World.Aspects
{
    public readonly partial struct WorldTileAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transform;

        public float X => _transform.LocalToWorld.Position.x;
        public float Y => _transform.LocalToWorld.Position.y;
        public float Z => _transform.LocalToWorld.Position.z;

        public void MoveToHeight(PerlinNoiseEvaluator perlinNoiseEvaluator, HeightGeneratorComponent heightGenerator)
        {
            UniformScaleTransform localToWorldTransform = _transform.LocalToWorld;

            float height = GetNoisedTileProperty(localToWorldTransform.Position.x, localToWorldTransform.Position.z, perlinNoiseEvaluator, heightGenerator);
            localToWorldTransform.Position.y = height;
            _transform.LocalToWorld = localToWorldTransform;
        }

        private static float GetNoisedTileProperty(float x, float y, PerlinNoiseEvaluator perlinNoiseEvaluator, HeightGeneratorComponent heightGenerator)
        {
            float standardNoisedValues = heightGenerator.StandardNoiseFilterValues.Evaluate(new float3(x, 0, y), perlinNoiseEvaluator);
            float rigidNoisedValues = heightGenerator.RigidNoiseFilterValues.Evaluate(new float3(x, 0, y), perlinNoiseEvaluator) * standardNoisedValues;
            return standardNoisedValues + rigidNoisedValues;
        }
    }
}