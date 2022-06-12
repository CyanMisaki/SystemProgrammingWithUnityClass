using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetRing : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private float minSize = 0.1f;
    [SerializeField] private float maxSize = 0.9f;
    
    [SerializeField] private int _numOfAsteroids=10;
    [SerializeField] private float _minDistance = 2f;


    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _maxOrbits;
    
    
    private List<GameObject> _asteroids;
    
    private void Awake()
    {
        _asteroids = new List<GameObject>();

        for (var i = 1; i < _maxOrbits+1; i++)
        {
            for (var j = 0; j < _numOfAsteroids; j++)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.transform.SetParent(_parent);

                var scale = Random.Range(minSize, maxSize + 0.1f);
                obj.transform.localScale = new Vector3(scale,scale,scale);
                obj.transform.localPosition = Random.insideUnitCircle.normalized * (scale * i*Random.Range(1,_maxOrbits*2)+_minDistance);
                
                _asteroids.Add(obj);
            }
        }
    }

    private void Update()
    {
        foreach (var asteroid in _asteroids)
        { 
            asteroid.transform.RotateAround(gameObject.transform.position,Vector3.back, _rotationSpeed* Time.deltaTime*(10/(asteroid.transform.position-_parent.transform.position).magnitude));
        }
    }

    private void OnDestroy()
    {
        _asteroids.Clear();
    }
}
