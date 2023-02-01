using Assets._Project._Scripts.Gameplay.SelectionSystem.Systems;
using Assets._Project._Scripts.Gameplay.SelectionSystem.Components;
using Unity.Entities;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{
    private void Awake()
	{
		
	}

	private void Start()
	{

    }

    private void Update()
	{
		
	}

	private void OnDrawGizmos()
    {
        try
        {
            World entityWorld = World.DefaultGameObjectInjectionWorld;
            SystemHandle selectionSystemHandle = entityWorld.GetExistingSystem<EntitySelectionSystem>();
            SelectionSystemData selectionSystemData = entityWorld.EntityManager.GetComponentData<SelectionSystemData>(selectionSystemHandle);
        
            if(selectionSystemData.TilePositions.Length == 0)
                return;

            foreach (Bounds bounds in selectionSystemData.TilePositions)
                Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        catch
        {
            // ignored
        }
    }
}
