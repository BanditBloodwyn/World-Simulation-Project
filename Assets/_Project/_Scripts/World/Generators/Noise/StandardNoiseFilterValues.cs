using System;
using Unity.Mathematics;

namespace Assets._Project._Scripts.World.Generators.Noise
{
    [Serializable]
    public struct StandardNoiseFilterValues
    {
        public int NumberOfLayers;

        public float Strength;
        public float MinValue;
        public float MaxValue;
        public float3 Center;

        public float BaseRoughness;
        public float Roughness;
        public float Persistance;
    }
}