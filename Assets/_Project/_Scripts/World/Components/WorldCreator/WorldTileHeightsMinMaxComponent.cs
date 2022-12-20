using Unity.Entities;

namespace Assets._Project._Scripts.World.Components.WorldCreator
{
    public struct WorldTileHeightsMinMaxComponent : IComponentData
    {
        public float MinimumTileHeight;
        public float MaximumTileHeight;
    }
}