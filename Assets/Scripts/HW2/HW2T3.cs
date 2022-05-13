using System;
using HW2.T3Files;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace HW2
{
    /*
создайте задачу типа IJobForTransform, которая будет вращать указанные
Transform вокруг своей оси с заданной скоростью.

     */
    public class HW2T3 : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;
        [SerializeField] private float _speed = 1f;
        private TransformAccessArray _transformAccess;
        
        private JobHandle _handle;
        private T3 _job;
        
        private void Start()
        {
            _transformAccess = new TransformAccessArray(_transforms);
            
            _job = new T3
            {
                _speed = _speed
            };
        }
        private void Update()
        {
            _handle = _job.Schedule(_transformAccess);
            _handle.Complete();
        }

        private void OnDestroy()
        {
            if(enabled)
                _transformAccess.Dispose();
        }

        
    }
}