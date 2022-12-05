using System;
using UnityEngine;

namespace Assets._Project._Scripts.Camera
{
    [Serializable]
    public struct CameraMovementSettings
    {
        [Range(0f, 10f)] public float MinimalSpeed;
        [Range(10f, 100f)] public float MaximumSpeed;
        [Range(0f, 200f)] public float Accelleration;
        public float PositionLimit;

        public void Reset()
        {
            Accelleration = 20;
            MinimalSpeed = 5;
            MaximumSpeed = 100;
        }
    }
}