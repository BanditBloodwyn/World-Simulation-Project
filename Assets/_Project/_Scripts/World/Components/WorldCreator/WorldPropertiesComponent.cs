using Unity.Entities;

namespace Assets._Project._Scripts.World.Components.WorldCreator
{
    public struct WorldPropertiesComponent : IComponentData
    {
        public int WorldSize;
        public Entity WorldTilePrefab;
    }
}