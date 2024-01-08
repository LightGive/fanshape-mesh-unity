using UnityEngine;

namespace Fanshape
{
    [ExecuteInEditMode]
    public class Fanshape : MonoBehaviour
    {
        [SerializeField] FanShapeData _shapeData = null;
        MeshFilter _meshFilter = null;
        MeshRenderer _meshRenderer = null;
        Mesh _mesh = null;

#if UNITY_EDITOR
        int _prePolygonCount;
#endif
        void Start()
        {
            CreateMesh(true);
        }

#if UNITY_EDITOR
        void Update()
        {
            CreateMesh(_shapeData.PolygonCount != _prePolygonCount);
            _prePolygonCount = _shapeData.PolygonCount;
        }
#endif
        void CreateMesh(bool isVertexCountChanged = true)
        {
            if (_meshFilter == null && !gameObject.TryGetComponent(out _meshFilter))
            {
                _meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            if (_meshRenderer == null && !gameObject.TryGetComponent(out _meshRenderer))
            {
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            if (isVertexCountChanged)
            {
                _mesh = new Mesh
                {
                    vertices = new Vector3[(_shapeData.PolygonCount + 1) * 2],
                    uv = new Vector2[(_shapeData.PolygonCount + 1) * 2],
                    triangles = new int[_shapeData.PolygonCount * 3 * 2]
                };

                var triangles = new int[_mesh.triangles.Length];
                for (var i = 0; i < _shapeData.PolygonCount; i++)
                {
                    triangles[i * 6 + 0] = i;
                    triangles[i * 6 + 1] = i + _mesh.vertices.Length / 2 + 1;
                    triangles[i * 6 + 2] = i + _mesh.vertices.Length / 2;
                    triangles[i * 6 + 3] = i;
                    triangles[i * 6 + 4] = i + 1;
                    triangles[i * 6 + 5] = i + _mesh.vertices.Length / 2 + 1;
                }
                _mesh.triangles = triangles;

                var uv = new Vector2[_mesh.uv.Length];
                for (var i = 0; i < uv.Length; i++)
                {
                    var y = i >= _shapeData.PolygonCount + 1 ? 1 : 0;
                    var lerp = (float)(i % (_shapeData.PolygonCount + 1)) / _shapeData.PolygonCount;
                    uv[i] = new Vector2(1.0f - lerp, y);
                }
                _mesh.uv = uv;
            }

            var vertices = new Vector3[_mesh.vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                var radius = _shapeData.RootRad;
                if (i >= _shapeData.PolygonCount + 1)
                {
                    radius += _shapeData.FanRad;
                }
                var lerp = (float)(i % (_shapeData.PolygonCount + 1)) / _shapeData.PolygonCount;
                var r = _shapeData.Angle * lerp * Mathf.Deg2Rad;
                r += (90.0f - _shapeData.Angle * _shapeData.Pivot) * Mathf.Deg2Rad;
                vertices[i] = new Vector3(Mathf.Cos(r) * radius, 0.0f, Mathf.Sin(r) * radius);
            }
            _mesh.vertices = vertices;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _mesh.RecalculateTangents();
            _meshFilter.sharedMesh = _mesh;
        }
    }
}