/*
Реализовать задачу WhatTaskFasterAsync, которая будет принимать в качестве
параметров CancellationToken, а также две задачи в виде переменных типа Task.
Задача должна ожидать выполнения хотя бы одной из задач, останавливать другую
и возвращать результат. Если первая задача выполнена первой, вернуть true, если
вторая — false. Если сработал CancellationToken, также вернуть false. Проверить
работоспособность с помощью задач из Задания 2.
*/

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class HW1T3 : MonoBehaviour
{
    private int _frameCounter;
    [SerializeField]private int _framesBeforeTask2End = 6000;

    private CancellationTokenSource _cancelTS;
    
    private void Start()
    {
        _cancelTS = new CancellationTokenSource();
        StartAsyncOperations();
        
    }


    private async void StartAsyncOperations()
    {
        var result = await WhatTaskFasterAsync(_cancelTS.Token, Task1(_cancelTS.Token), Task2(_cancelTS.Token));
        _cancelTS.Cancel();
        Debug.Log(result);
    }
    
    public static async Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task task1, Task task2)
    {
        var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var masterTask = await Task.WhenAny(task1, task2);
        if (!linkedCTS.IsCancellationRequested)
        {
            linkedCTS.Dispose();
            return (masterTask == task1);
        }
        linkedCTS.Dispose();    
        return false;
    }


    private async Task Task1(CancellationToken cancelToken)
    {
        await Task.Delay(1000);
        if(cancelToken.IsCancellationRequested)
        { 
            Debug.Log("Task 1 cancelled by Cancellation Token");
            return;
        }
        Debug.Log("Task1 completed work.");
    }

    private async Task Task2(CancellationToken cancelToken)
    {
        while(_frameCounter<_framesBeforeTask2End)
        {
            if(cancelToken.IsCancellationRequested)
            {
                Debug.Log("Task 2 cancelled by Cancellation Token");
                return;
            }
            _frameCounter++;
            await Task.Yield();
        }
        _frameCounter=0;
        Debug.Log("Task2 completed work.");
    }

    private void OnDestroy()
    {
        _cancelTS?.Cancel();
        _cancelTS?.Dispose();
    }

}