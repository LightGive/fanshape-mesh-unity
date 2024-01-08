using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fanshape
{
    [System.Serializable]
    public class FanShapeData
    {
        [field: SerializeField] public float RootRad { get; private set; } = 0.2f;
        [field: SerializeField] public float FanRad { get; private set; } = 1.0f;
        [field: SerializeField, Range(0.0f, 360.0f)] public float Angle { get; private set; } = 90.0f;
        [field: SerializeField, Range(3, 99)] public int PolygonCount { get; private set; } = 20;
        [field: SerializeField, Range(0.0f, 1.0f)] public float Pivot { get; private set; } = 0.5f;
    }
}