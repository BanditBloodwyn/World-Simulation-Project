using System;
using UnityEngine;

namespace Assets._Project._Scripts.Camera
{
    [Serializable]
    public struct CameraRotationSettings
    {
        [Range(0.5f, 10)] public float LookSpeed;
        [Range(10, 120)] public float LookXLimit;

        public void Reset()
        {
            LookSpeed = 2.0f;
            LookXLimit = 80.0f;
        }
    }
}