using Assets._Project._Scripts.World.Components.WorldCreator;
using Assets._Project._Scripts.World.Generators.Noise;
using Unity.Entities;
using UnityEngine;

namespace Assets._Project._Scripts.World.Authoring
{
    public class WorldPropertiesMono : MonoBehaviour
    {
        public int worldSize;
        public GameObject worldTilePrefab;
        public StandardNoiseFilterValues standardNoiseFilter;
        public RigidNoiseFilterValues rigidNoiseFilter;
    }

    public class WorldPropertiesBaker : Baker<WorldPropertiesMono>
    {
        public override void Bake(WorldPropertiesMono authoring)
        {
            AddComponent(new WorldPropertiesComponent
            {
                WorldSize = authoring.worldSize,
                WorldTilePrefab = GetEntity(authoring.worldTilePrefab)
            });

            AddComponent(new HeightGeneratorComponent
            {
                StandardNoiseFilterValues = authoring.standardNoiseFilter,
                RigidNoiseFilterValues = authoring.rigidNoiseFilter,
            });
        }
    }
}