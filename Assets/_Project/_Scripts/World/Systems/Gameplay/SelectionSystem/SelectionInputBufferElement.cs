using Unity.Entities;
using Unity.Physics;

namespace Assets._Project._Scripts.World.Systems.Gameplay.SelectionSystem
{
    public struct SelectionInputBufferElement : IBufferElementData
    {
        public RaycastInput Value;
    }
}