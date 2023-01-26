using Assets._Project._Scripts.Core.ECS.Aspects;
using Unity.Entities;

namespace Assets._Project._Scripts.World.Systems.Gameplay.SelectionSystem
{
    public partial struct HighlightSelectionsSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SelectableEntityAspect>();
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {

        }
    }
}