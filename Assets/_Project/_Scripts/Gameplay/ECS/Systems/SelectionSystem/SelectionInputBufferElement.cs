using Unity.Entities;
using Unity.Physics;

namespace Assets._Project._Scripts.Gameplay.ECS.Systems.SelectionSystem
{
    public struct SelectionInputBufferElement : IBufferElementData
    {
        public RaycastInput Value;
    }
}