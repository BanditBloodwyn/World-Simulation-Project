﻿using Assets._Project._Scripts.World.Generators.Noise;
using Unity.Collections;
using Unity.Entities;

namespace Assets._Project._Scripts.World.Components
{
    public struct HeightGeneratorComponent : IComponentData
    {
        public StandardNoiseFilterValues StandardNoiseFilterValues;
        public NativeArray<int> PerlinNoiseSource;
    }
}