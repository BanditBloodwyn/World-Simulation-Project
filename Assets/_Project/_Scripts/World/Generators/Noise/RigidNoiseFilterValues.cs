using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets._Project._Scripts.World.Generators.Noise
{    
    [Serializable]
    public struct RigidNoiseFilterValues : INoiseParameters
    {
        [SerializeField] private int NumberOfLayers;

        [SerializeField] private float Strength;
        [SerializeField] private float MinValue;
        [SerializeField] private float MaxValue;
        [SerializeField] private float3 Center;

        [SerializeField] private float BaseRoughness;
        [SerializeField] private float Roughness;
        [SerializeField] private float Persistance;
        [SerializeField] private float WeightMultiplier;
       
        public float Evaluate(float3 point, PerlinNoiseEvaluator noiseEvaluator)
        {
            float noiseValue = 0;
            float frequency = BaseRoughness;
            float amplitude = 1;
            float weight = 1;

            for (int i = 0; i < NumberOfLayers; i++)
            {
                float v = 1 - Mathf.Abs(noiseEvaluator.Evaluate(point * frequency + Center));

                v *= v;
                v *= weight;
                weight = Mathf.Clamp01(v * WeightMultiplier);

                noiseValue += v * amplitude;
                frequency *= Roughness;
                amplitude *= Persistance;
            }

            noiseValue *= Strength;

            noiseValue = noiseValue >= MinValue
                ? noiseValue
                : MinValue;

            noiseValue = noiseValue <= MaxValue
                ? noiseValue
                : MaxValue;

            return noiseValue;
        }
    }
}