using Unity.Entities;

namespace Assets._Project._Scripts.World.Components
{
    public struct WorldPropertiesComponent : IComponentData
    {
        public int WorldSize;
        public Entity WorldTilePrefab;
    }
}