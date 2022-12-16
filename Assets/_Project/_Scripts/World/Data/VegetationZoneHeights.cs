using System;
using Assets._Project._Scripts.World.Data.Enums;

namespace Assets._Project._Scripts.World.Data
{
    [Serializable]
    public struct VegetationZoneHeights
    {
        public VegetationZones VegetationZone;
        public float MaximumHeight;
    }
}