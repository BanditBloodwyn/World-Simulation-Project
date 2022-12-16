using System.Linq;
using Assets._Project._Scripts.World.Components.WorldCreator;
using Assets._Project._Scripts.World.Data;
using Assets._Project._Scripts.World.Data.Enums;
using Assets._Project._Scripts.World.Generators.Noise;
using Sirenix.OdinInspector;
using Unity.Entities;
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
        [TitleGroup("VegetationZones")]
        public Material worldTileMaterial;
    }

    public class WorldPropertiesBaker : Baker<WorldPropertiesMono>
    {
        public override void Bake(WorldPropertiesMono authoring)
        {
            authoring.worldTilePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetFloat(
                "_KollineHeight",
                authoring.vegetationZoneHeights.First(static height => height.VegetationZone == VegetationZones.Kolline).MaximumHeight / 100);
            authoring.worldTilePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetFloat(
                "_MontaneHeight",
                authoring.vegetationZoneHeights.First(static height => height.VegetationZone == VegetationZones.Montane).MaximumHeight / 100);
            authoring.worldTilePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetFloat(
                "_SubalpineHeight",
                authoring.vegetationZoneHeights.First(static height => height.VegetationZone == VegetationZones.Subalpine).MaximumHeight / 100);
            authoring.worldTilePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetFloat(
                "_Alpine_TreesHeight",
                authoring.vegetationZoneHeights.First(static height => height.VegetationZone == VegetationZones.Alpine_Trees).MaximumHeight / 100);
            authoring.worldTilePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetFloat(
                "_Alpine_BushesHeight",
                authoring.vegetationZoneHeights.First(static height => height.VegetationZone == VegetationZones.Alpine_Bushes).MaximumHeight / 100);
            authoring.worldTilePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetFloat(
                "_SubnivaleHeight",
                authoring.vegetationZoneHeights.First(static height => height.VegetationZone == VegetationZones.Subnivale).MaximumHeight / 100);
            authoring.worldTilePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetFloat(
                "_NivaleHeight",
                authoring.vegetationZoneHeights.First(static height => height.VegetationZone == VegetationZones.Nivale).MaximumHeight / 100);

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