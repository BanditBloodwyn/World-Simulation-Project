using Assets._Project._Scripts.World.Authoring;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Project._Scripts.Camera
{
    public class SECamera : MonoBehaviour
    {
        [SerializeField] private CameraMovementSettings MovementSettings;
        [SerializeField] private CameraRotationSettings RotationSettings;
        [SerializeField] private UnityEvent<float> ShowSpeedEvent;

        private float _rotationX;
        private float _rotationY;

        private Vector3 _currentSpeed;
        private Vector3 _targetPosition;
        private float _currentMaxSpeed;

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(_targetPosition, 1);
        //    Gizmos.DrawLine(transform.position, _targetPosition);
        //}

        private void Start()
        {
            MovementSettings.PositionLimit = FindObjectOfType<WorldPropertiesMono>().worldSize;
            _targetPosition = transform.position;
            _currentMaxSpeed = 20;
        }

        private void Update()
        {
            HandleLookRotation();
            HandleMovementSpeed();
            HandlePosition();
        }

        private void HandleLookRotation()
        {
            Cursor.visible = !Input.GetMouseButton(2);
            Cursor.lockState = Input.GetMouseButton(2) ? CursorLockMode.Locked : CursorLockMode.None;

            if (Input.GetMouseButton(2) == false)
                return;

            float lookHorizontal = Input.GetAxis("Mouse X");
            float lookVertical = -Input.GetAxis("Mouse Y");

            _rotationX += lookVertical * RotationSettings.LookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -RotationSettings.LookXLimit, RotationSettings.LookXLimit);
            _rotationY += lookHorizontal * RotationSettings.LookSpeed;

            transform.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        }

        private void HandleMovementSpeed()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                _currentMaxSpeed += 5;
                if (_currentMaxSpeed > MovementSettings.MaximumSpeed)
                    _currentMaxSpeed = MovementSettings.MaximumSpeed;

                ShowSpeedEvent?.Invoke(_currentMaxSpeed);
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                _currentMaxSpeed -= 5;
                if (_currentMaxSpeed < MovementSettings.MinimalSpeed)
                    _currentMaxSpeed = MovementSettings.MinimalSpeed;

                ShowSpeedEvent?.Invoke(_currentMaxSpeed);
            }
        }

        private void HandlePosition()
        {
            Vector3 translation = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                translation += transform.forward;
            if (Input.GetKey(KeyCode.S))
                translation += transform.forward * -1;
            if (Input.GetKey(KeyCode.D))
                translation += transform.right;
            if (Input.GetKey(KeyCode.A))
                translation += transform.right * -1;

            _targetPosition += _currentMaxSpeed * Time.deltaTime * translation;

            LimitPosition(ref _targetPosition, MovementSettings.PositionLimit);

            transform.position = Vector3.SmoothDamp(
                transform.position,
                _targetPosition,
                ref _currentSpeed,
                MovementSettings.Accelleration * Time.deltaTime);
        }

        private static void LimitPosition(ref Vector3 targetPosition, float limit)
        {
            if (targetPosition.x < 0)
                targetPosition.x = 0;
            if (targetPosition.x > limit)
                targetPosition.x = limit;

            if (targetPosition.y < 0)
                targetPosition.y = 0;

            if (targetPosition.z < 0)
                targetPosition.z = 0;
            if (targetPosition.z > limit)
                targetPosition.z = limit;
        }
    }
}