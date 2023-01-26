using Assets._Project._Scripts.Gameplay.ECS.Systems.SelectionSystem;
using Unity.Assertions;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using Input = UnityEngine.Input;
using Ray = UnityEngine.Ray;

namespace Assets._Project._Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        private Entity _inputBufferEntity;
        private Unity.Entities.World _entityWorld;

        private void Awake()
        {
            Assert.IsNotNull(_camera);
        }

        private void OnEnable()
        {
            _entityWorld = Unity.Entities.World.DefaultGameObjectInjectionWorld;
        }

        private void OnDisable()
        {
            if (_entityWorld.IsCreated && _entityWorld.EntityManager.Exists(_inputBufferEntity))
                _entityWorld.EntityManager.DestroyEntity(_inputBufferEntity);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0)) 
                HandleMouseClick();
        }

        private void HandleMouseClick()
        {
            if (_entityWorld.IsCreated && !_entityWorld.EntityManager.Exists(_inputBufferEntity))
            {
                _inputBufferEntity = _entityWorld.EntityManager.CreateEntity();
                _entityWorld.EntityManager.AddBuffer<SelectionInputBufferElement>(_inputBufferEntity);
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastInput raycastInput = new RaycastInput
            {
                Start = ray.origin,
                Filter = CollisionFilter.Default,
                End = ray.GetPoint(_camera.farClipPlane)
            };

            _entityWorld.EntityManager
                .GetBuffer<SelectionInputBufferElement>(_inputBufferEntity)
                .Add(new SelectionInputBufferElement { Value = raycastInput });
        }
    }
}
