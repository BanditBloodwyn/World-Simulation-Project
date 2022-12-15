using Unity.Entities;
using Unity.Rendering;

namespace Assets._Project._Scripts.World.Components.WorldTile
{

    [MaterialProperty("_IsWater")]
    public struct BiomeMaterialOverride : IComponentData
    {
        public bool isWater;
    }
}