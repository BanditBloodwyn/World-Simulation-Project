using Unity.Entities;
using Unity.Rendering;

namespace Assets._Project._Scripts.World.Components.WorldTile
{
    [MaterialProperty("_TileHeight")]

    public struct WorldMaterialOverride : IComponentData
    {
        public float Value;
    }
}