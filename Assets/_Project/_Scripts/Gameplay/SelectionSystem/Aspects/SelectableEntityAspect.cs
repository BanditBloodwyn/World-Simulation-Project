using Assets._Project._Scripts.Gameplay.SelectionSystem.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets._Project._Scripts.Gameplay.SelectionSystem.Aspects
{
    public readonly partial struct SelectableEntityAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;

        private readonly RefRO<SelectableEntityTag> _selectableTag;

        public Bounds CreateBounds()
        {
            return new Bounds
            {
                center = _transformAspect.Position + math.up() * 1.45f,
                size = new Vector3(1f, 0.1f, 1f)
            };
        }
    }
}