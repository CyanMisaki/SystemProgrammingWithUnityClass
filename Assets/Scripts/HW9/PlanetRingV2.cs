using System.Collections.Generic;
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
        [SerializeField] private int _maxOrbits;
    
    
        private TransformAccessArray _asteroids;
        private Transform[] _transforms;

        public struct RotateAsteroids : IJobParallelForTransform
        {
            internal float circleRadians;
            internal float currentAng;
            internal Vector3 parentPosition;


            public void Execute(int index, TransformAccess transform)
            {
                var p = parentPosition;
                
                //нужен угол, нужна дистанция от центра
                //p.x += Mathf.Sin(transform.currentAng) * asteroid.dist;
                //p.z += Mathf.Cos(transform.currentAng) * asteroid.dist;
                transform.position=p;

                //нужно изменить угол
                //asteroid.ChangeAngle(asteroid.speed * Time.deltaTime);
            }
        }
        private void Awake()
        {
            for (var j = 0; j < _numOfAsteroids; j++) 
            { 
                var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
                obj.transform.SetParent(_parent);

                var scale = Random.Range(minSize, maxSize + 0.1f);
                
                obj.transform.localScale = new Vector3(scale,scale,scale); 
                obj.transform.localPosition = Random.insideUnitCircle.normalized * (_minDistance + scale * Random.Range(1,_maxOrbits*2));
                
                _transforms[j] = obj.transform;
            }

            _asteroids = new TransformAccessArray(_transforms);
        }
        
    

        private void Update()
        {
            var job = new RotateAsteroids()
            {
                circleRadians = Mathf.PI * 2,
                currentAng = 0,
                parentPosition = _parent.position
            };
            
            var handle = job.Schedule(_asteroids);
            handle.Complete();
        }

        private void OnDestroy()
        {
            _asteroids.Dispose();
        }
    }
}