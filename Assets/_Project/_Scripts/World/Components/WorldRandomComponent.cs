using Unity.Entities;
using Unity.Mathematics;

namespace Assets._Project._Scripts.World.Components
{
    public struct WorldRandomComponent : IComponentData
    {
        public Random Value;
    }
}