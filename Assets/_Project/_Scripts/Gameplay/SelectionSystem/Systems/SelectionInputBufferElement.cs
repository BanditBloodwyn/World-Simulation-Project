using Unity.Entities;
using Unity.Physics;

namespace Assets._Project._Scripts.Gameplay.SelectionSystem.Systems
{
    public struct SelectionInputBufferElement : IBufferElementData
    {
        public RaycastInput Value;
    }
}