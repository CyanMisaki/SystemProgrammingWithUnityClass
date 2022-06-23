﻿using System;
using UnityEngine;

namespace HW10
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class Star : MonoBehaviour
    {
        private Mesh _mesh;
        
        [SerializeField] private ColorPoint _center;
        [SerializeField, NonReorderable] private ColorPoints _points;
        [SerializeField] private int _frequency = 1;

        public int Frequency => _frequency;
        public ColorPoints Points => _points;

        private Vector3[] _vertices;
        private Color[] _colors;

        private int[] _triangles;

        private void Start()
        {
            UpdateMesh();
        }
        public void UpdateMesh()
        {
            GetComponent<MeshFilter>().sharedMesh = _mesh = new Mesh();
            _mesh.name = "Star Mesh";
            _mesh.hideFlags = HideFlags.HideAndDontSave;
            if (_frequency < 1)
            {
                _frequency = 1;
            }
            
            _points.Points ??= Array.Empty<ColorPoint>();
            var numberOfPoints = _frequency * _points.Points.Length;
            _vertices = new Vector3[numberOfPoints + 1];
            _colors = new Color[numberOfPoints + 1];
            _triangles = new int[numberOfPoints * 3];
            if (numberOfPoints >= 3)
            {
                _vertices[0] = _center.Position;
                _colors[0] = _center.Color;
                var angle = -360f / numberOfPoints;
                for (int repetitions = 0, v = 1, t = 1; repetitions < _frequency;
                     repetitions++)
                {
                    for (var p = 0; p < _points.Points.Length; p++, v++, t += 3)
                    {
                        _vertices[v] = Quaternion.Euler(0f, 0f, angle * (v - 1)) *
                                       _points.Points[p].Position;
                        _colors[v] = _points.Points[p].Color;
                        _triangles[t] = v;
                        _triangles[t + 1] = v + 1;
                    }
                }
                _triangles[_triangles.Length - 1] = 1;
            }
            _mesh.vertices = _vertices;
            _mesh.colors = _colors;
            _mesh.triangles = _triangles;
        }

        private void Reset()
        {
            UpdateMesh();
        }

    }
}