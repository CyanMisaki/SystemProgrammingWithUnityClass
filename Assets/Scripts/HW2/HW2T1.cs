using HW2.T1Files;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


/*
Создайте задачу типа IJob, которая принимает данные в формате
NativeArray<int> и в результате выполнения все значения более десяти делает
равными нулю.
Вызовите выполнение этой задачи из внешнего метода и выведите в консоль
результат.

*/
namespace HW2
{
    public class HW2T1 : MonoBehaviour
    {
    
        private NativeArray<int> _array;
        private JobHandle _handle;
        private T1 _job;
    
        private void Start()
        {
            _array = new NativeArray<int>(10, Allocator.TempJob);
            
            FillArray();
        
            LogMe("Array before Job");

            _job = new T1
            {
                _array = _array
            };

            TransformArray();

            LogMe("Array after Job");
            
            _array.Dispose();
            Debug.Log("Array disposed");
        }

        private void FillArray()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                _array[i] = Random.Range(1, 13);
            }
        }

        private void LogMe(string message)
        {
            Debug.Log(message);
            foreach (var item in _array)
            {
                Debug.Log(item);
            }
        }

        private void TransformArray()
        {
            _handle = _job.Schedule();
            _handle.Complete();
        }
    }
}
