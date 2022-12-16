using Unity.Entities;
using Unity.Rendering;

namespace Assets._Project._Scripts.World.Components.WorldTile
{

    [MaterialProperty("_TileHeight")]
    public struct VegetationZoneMaterialOverride : IComponentData
    {
        public float Value;
    }
}