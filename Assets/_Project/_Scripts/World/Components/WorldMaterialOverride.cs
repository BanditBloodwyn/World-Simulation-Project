using Unity.Entities;
using Unity.Rendering;

namespace Assets._Project._Scripts.World.Components
{
    [MaterialProperty("_TileHeight", -1)]

    public struct WorldMaterialOverride : IComponentData
    {
        public float Value;
    }
}