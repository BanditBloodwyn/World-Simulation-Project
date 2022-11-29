using System;
using Assets._Project._Scripts.World.Components;
using Assets._Project._Scripts.World.Generators.Noise;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets._Project._Scripts.World.Aspects
{
    public readonly partial struct WorldAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;

        private readonly RefRO<WorldPropertiesComponent> _worldProperties;
        private readonly RefRW<WorldRandomComponent> _worldRandom;
        private readonly RefRW<HeightGeneratorComponent> _heightGenerator;

        public int WorldSize => _worldProperties.ValueRO.WorldSize;

        public Entity WorldTilePrefab => _worldProperties.ValueRO.WorldTilePrefab;

        public static UniformScaleTransform GetWorldTileTransform(int x, int y, float random)
        {
            return new UniformScaleTransform
            {
                Position = new float3(x, random, y),
                Rotation = new quaternion(),
                Scale = 1f
            };
        }

        public float GetNoisedTileProperty(int x, int y)
        {
            PerlinNoiseEvaluator perlinNoiseEvaluator = new PerlinNoiseEvaluator(
                0,
                _heightGenerator.ValueRO.PerlinNoiseSource);

            return StandardNoiseFilterEvaluator.Evaluate(new float3(x, 0, y), _heightGenerator.ValueRO.StandardNoiseFilterValues, perlinNoiseEvaluator);
        }
    }
}