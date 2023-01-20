using Unity.Assertions;
using Unity.Entities;
using UnityEngine;

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
    }
}
