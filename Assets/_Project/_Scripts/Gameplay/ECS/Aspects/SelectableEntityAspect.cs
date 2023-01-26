using Assets._Project._Scripts.Gameplay.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets._Project._Scripts.Gameplay.ECS.Aspects
{
    public readonly partial struct SelectableEntityAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;

        private readonly RefRO<SelectableEntityTag> _selectableTag;

        public float3 Position => _transformAspect.Position;
    }
}