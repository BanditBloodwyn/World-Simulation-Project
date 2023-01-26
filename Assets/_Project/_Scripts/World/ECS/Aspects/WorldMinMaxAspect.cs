using Assets._Project._Scripts.World.ECS.Components.WorldCreator;
using Unity.Entities;

namespace Assets._Project._Scripts.World.ECS.Aspects
{
    public readonly partial struct WorldMinMaxAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<WorldTileHeightsMinMaxComponent> _worldTileHeightsMinMax;

        public float GetInterpolation(float height)
        {
            float minimum = _worldTileHeightsMinMax.ValueRO.MinimumTileHeight;
            float maximum = _worldTileHeightsMinMax.ValueRO.MaximumTileHeight;

            return 100 / (maximum - minimum) * (height - minimum) / 100;
        }
    }
}