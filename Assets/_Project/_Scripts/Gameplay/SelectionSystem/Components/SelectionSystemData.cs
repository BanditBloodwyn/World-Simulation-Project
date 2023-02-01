using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Assets._Project._Scripts.Gameplay.SelectionSystem.Components
{
    public struct SelectionSystemData : IComponentData
    {
        public NativeArray<Bounds> TilePositions;
    }
}