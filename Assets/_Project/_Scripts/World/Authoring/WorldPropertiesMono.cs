using Assets._Project._Scripts.World.Data;
using Assets._Project._Scripts.World.Generators.Noise;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets._Project._Scripts.World.Authoring
{
    public class WorldPropertiesMono : MonoBehaviour
    {
        public int worldSize;
        public GameObject worldTilePrefab;

        [TitleGroup("Terrain")]
        public StandardNoiseFilterValues standardNoiseFilter;
        [TitleGroup("Terrain")]
        public RigidNoiseFilterValues rigidNoiseFilter;

        [TitleGroup("VegetationZones")]
        public VegetationZoneHeights[] vegetationZoneHeights;
    }
}