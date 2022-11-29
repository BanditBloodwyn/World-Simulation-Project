using System;
using Unity.Mathematics;

namespace Assets._Project._Scripts.World.Generators.Noise
{
    public struct StandardNoiseFilterEvaluator
    {
        public static float Evaluate(float3 point, StandardNoiseFilterValues values, PerlinNoiseEvaluator noiseEvaluator)
        {
            float noiseValue = 0;
            float frequency = values.BaseRoughness;
            float amplitude = 1;

            for (int i = 0; i < values.NumberOfLayers; i++)
            {
                float v = noiseEvaluator.Evaluate(point * frequency + values.Center);
                noiseValue += (v + 1) * 0.5f * amplitude;
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