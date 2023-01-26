using Assets._Project._Scripts.Core.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets._Project._Scripts.Core.ECS.Aspects
{
    public readonly partial struct SelectableEntityAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;

        private readonly RefRO<SelectableEntityTag> _selectableTag;

        public float3 Position => _transformAspect.Position;
    }
}