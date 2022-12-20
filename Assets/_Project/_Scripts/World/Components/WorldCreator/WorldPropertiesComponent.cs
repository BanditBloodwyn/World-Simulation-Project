using Assets._Project._Scripts.World.Data;
using Unity.Collections;
using Unity.Entities;

namespace Assets._Project._Scripts.World.Components.WorldCreator
{
    public struct WorldPropertiesComponent : IComponentData
    {
        public int WorldSize;
        public Entity WorldTilePrefab;
        public NativeArray<VegetationZoneHeights> VegetationZoneHeights;
    }
}