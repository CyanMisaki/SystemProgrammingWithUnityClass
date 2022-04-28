using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HW2.T2Files
{
    public struct T2 : IJobParallelFor
    {
        internal NativeArray<Vector3> _positions;
        internal NativeArray<Vector3> _velocities;
        internal NativeArray<Vector3> _finalPositions;
        
        
        public void Execute(int index)
        {
            
            _finalPositions[index] = _positions[index] + _velocities[index];
            Debug.Log($"additioning position #{index} with velocity #{index} to final position #{index}");
        }
    }
}