/*
Реализовать задачу WhatTaskFasterAsync, которая будет принимать в качестве
параметров CancellationToken, а также две задачи в виде переменных типа Task.
Задача должна ожидать выполнения хотя бы одной из задач, останавливать другую
и возвращать результат. Если первая задача выполнена первой, вернуть true, если
вторая — false. Если сработал CancellationToken, также вернуть false. Проверить
работоспособность с помощью задач из Задания 2.
*/

using System;

public class HW1Task2 : MonoBehaviour
{
    private int _frameCounter=0;
    private const int C_FrameBeforeTask2End = 60;

    CancellationTokenSource _cancelTS = new CancellationTokenSource();
    CancellationToken _cancelToken = _cancelTS.Token;

    private void Start()
    {
        var task1 = Task1(_cancelToken);
        var task2 = Task2(_cancelToken);
        Debug.Log(WhatTaskFasterAsync(_cancelToken, task1, task2)).ToString());
    }

    public static Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task task1, Task task2)
    {
        using (var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(ct))
        {
            var linkedCt = linkedCTS.Token;
            var masterTask = await Task.WhenAny(task1,task2);

            var result = (masterTask == task1 && masterTask.Result);
            linkedCTS.Cancel();
            linkedCTS.Dispose();
            return result;
        }
    }


    private async Task<bool> Task1(CancellationToken cancelToken)
    {
        if(cancelToken.IsCancellationRequested()
        {
            Debug.Log("Task 1 cancelled by Cancellation Token");
            return false;
        }
        await Task.Delay(1000);
        Debug.Log("Task1 completed work.");
        return true;
    }

    private async Task<bool> Task2(CancellationToken cancelToken)
    {
        while(_frameCounter<C_FrameBeforeTask2End)
        {
            if(cancelToken.IsCancellationRequested()
            {
                Debug.Log("Task 2 cancelled by Cancellation Token");
                return false;
            }
            _frameCounter++;
            await Task.Yield();
        }
        _frameCounter=0;
        Debug.Log("Task1 completed work.");
        return true;
    }

    private void private void OnDestroy()
    {
        _cancelTS.Cancel();
        _cancelTS.Dispose();
    }

}