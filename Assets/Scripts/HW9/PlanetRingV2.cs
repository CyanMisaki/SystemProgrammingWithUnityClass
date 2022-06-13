using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace HW9
{
    public class PlanetRingV2 : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private float minSize = 0.1f;
        [SerializeField] private float maxSize = 0.9f;
    
        [SerializeField] private int _numOfAsteroids=10;
        [SerializeField] private float _minDistance = 2f;


        [SerializeField] private float _rotationSpeed;
        [SerializeField] private int _maxOrbitsMul;
    
    
        private TransformAccessArray _asteroids;
        private Transform[] _transforms;
        private NativeArray<float> _asteroidsData;
        private const float circleRadians = Mathf.PI * 2;
        public struct RotateAsteroids : IJobParallelForTransform
        {
            
            internal float currentAng;
            [ReadOnly]internal Vector3 parentPosition;
            [NativeDisableParallelForRestriction]internal NativeArray<float> asteroidsData;
            [ReadOnly] internal float deltaTime;
            [ReadOnly] internal float rotationSpeed;
            private const float circleRadians = Mathf.PI * 2;
            public void Execute(int index, TransformAccess transform)
            {
                var p = parentPosition;
                
                p.x += Mathf.Sin(asteroidsData[index*3]) * asteroidsData[index*3+1];
                p.z += Mathf.Cos(asteroidsData[index*3]) * asteroidsData[index*3+1];  
               
                transform.position=p;
                
                //смена угла
                asteroidsData[index*3] += circleRadians * deltaTime * asteroidsData[index*3+2];
            }
        }
        
        private void Awake()
        {
            _asteroidsData = new NativeArray<float>(_numOfAsteroids * 3, Allocator.Persistent);
            _transforms = new Transform[_numOfAsteroids];
            
            var asteroidsDataIndexer=0;
            
            for (var j = 0; j < _numOfAsteroids; j++)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
                obj.transform.SetParent(_parent);

                var scale = Random.Range(minSize, maxSize + 0.1f);
                
                obj.transform.localScale = new Vector3(scale,scale,scale);
                obj.transform.localPosition = Random.insideUnitCircle.normalized * (_minDistance + scale * Random.Range(1,_maxOrbitsMul*2));
                
                _transforms[j] = obj.transform;
                
                
                _asteroidsData[asteroidsDataIndexer+1] = (obj.transform.position-_parent.position).magnitude; //расстояние между планетой и астероидом
                _asteroidsData[asteroidsDataIndexer + 2] = _rotationSpeed * Time.deltaTime * (10 / _asteroidsData[asteroidsDataIndexer + 1]); // скорость вращения в зависимости от расстояния от точки
                _asteroidsData[asteroidsDataIndexer] = circleRadians * _asteroidsData[asteroidsDataIndexer+1]* _asteroidsData[asteroidsDataIndexer+2]; //угол
                
                asteroidsDataIndexer += 3;
            }

            _asteroids = new TransformAccessArray(_transforms);
            
        }
        
    

        private void Update()
        {
            var job = new RotateAsteroids()
            {
                parentPosition = _parent.position,
                asteroidsData = _asteroidsData,
                deltaTime = Time.deltaTime,
                rotationSpeed = _rotationSpeed
            };
            
            var handle = job.Schedule(_asteroids);
            handle.Complete();
        }

        private void OnDestroy()
        {
            _asteroids.Dispose();
            _asteroidsData.Dispose();
            _transforms = null;
        }
    }
}