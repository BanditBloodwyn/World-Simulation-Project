using Unity.Assertions;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Project._Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputAction _mouseClickInput;
        [SerializeField] private UnityEngine.Camera _camera;

        private Entity _inputBufferEntity;
        private Unity.Entities.World _entityWorld;

        private void Awake()
        {
            Assert.IsNotNull(_mouseClickInput);
            Assert.IsNotNull(_camera);
        }

        private void OnEnable()
        {
            _mouseClickInput.started += MouseClicked;
            _mouseClickInput.Enable();

            _entityWorld = Unity.Entities.World.DefaultGameObjectInjectionWorld;
        }

        private void OnDisable()
        {
            _mouseClickInput.started -= MouseClicked;
            _mouseClickInput.Disable();

            if (_entityWorld.IsCreated && _entityWorld.EntityManager.Exists(_inputBufferEntity))
                _entityWorld.EntityManager.DestroyEntity(_inputBufferEntity);

        }

        private void MouseClicked(InputAction.CallbackContext context)
        {

        }
    }
}
