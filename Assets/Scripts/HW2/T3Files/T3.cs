using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace HW2.T3Files
{
    public struct T3 : IJobParallelForTransform
    {
        [ReadOnly] public float _speed;
        
        public void Execute(int index, TransformAccess transform)
        { 
            transform.rotation *= Quaternion.AngleAxis(0.01f*_speed,Vector3.up);
        }
    }
}