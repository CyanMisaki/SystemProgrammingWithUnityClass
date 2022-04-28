using System;
using HW2.T2Files;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HW2
{
    /*
Cоздайте задачу типа IJobParallelFor, которая будет принимать данные в
виде двух контейнеров: Positions и Velocities — типа NativeArray<Vector3>. Также
создайте массив FinalPositions типа NativeArray<Vector3>.
Сделайте так, чтобы в результате выполнения задачи в элементы массива
FinalPositions были записаны суммы соответствующих элементов массивов Positions
и Velocities.
Вызовите выполнение созданной задачи из внешнего метода и выведите в консоль
результат.
     */
    public class HW2T2 : MonoBehaviour
    {
        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _finalPositions;

        private JobHandle _handle;
        private T2 _job;


        private void Start()
        {
            _positions = new NativeArray<Vector3>(5, Allocator.Persistent);
            _velocities = new NativeArray<Vector3>(5, Allocator.Persistent);
            _finalPositions = new NativeArray<Vector3>(5, Allocator.Persistent);

            _job = new T2
            {
                _positions = _positions,
                _velocities = _velocities,
                _finalPositions = _finalPositions
            };

            DoWork();
            Debug.Log("Job completed");
            _positions.Dispose();
            _velocities.Dispose();
            _finalPositions.Dispose();
            Debug.Log("Arrays disposed");
        }

        

        private void DoWork()
        {
            Debug.Log("Job started");
            _handle = _job.Schedule(5,5);
            _handle.Complete();
        }

        
    }
}