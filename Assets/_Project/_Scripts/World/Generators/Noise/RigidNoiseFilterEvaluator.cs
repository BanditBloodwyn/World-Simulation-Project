using Unity.Mathematics;
using UnityEngine;

namespace Assets._Project._Scripts.World.Generators.Noise
{
    public struct RigidNoiseFilterEvaluator
    {
        public static float Evaluate(float3 point, RigidNoiseFilterValues values, PerlinNoiseEvaluator noiseEvaluator)
        {
            float noiseValue = 0;
            float frequency = values.BaseRoughness;
            float amplitude = 1;
            float weight = 1;

            for (int i = 0; i < values.NumberOfLayers; i++)
            {
                float v = 1 - Mathf.Abs(noiseEvaluator.Evaluate(point * frequency + values.Center));

                v *= v;
                v *= weight;
                weight = Mathf.Clamp01(v * values.WeightMultiplier);

                noiseValue += v * amplitude;
                frequency *= values.Roughness;
                amplitude *= values.Persistance;
            }

            noiseValue *= values.Strength;

            noiseValue = noiseValue >= values.MinValue
                ? noiseValue
                : values.MinValue;

            noiseValue = noiseValue <= values.MaxValue
                ? noiseValue
                : values.MaxValue;

            return noiseValue;
        }
    }
}