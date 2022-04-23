/*Реализовать две задачи: Task1 и Task2. В качестве параметров задачи должны
принимать CancellationToken. Первая задача должна ожидать одну секунду, а после
выводить в консоль сообщение о своём завершении. Вторая задача должна
ожидать 60 кадров, а после — выводить сообщение в консоль*/

using System;

public class HW1Task2 : MonoBehaviour
{
    private int _frameCounter=0;
    private const int C_FrameBeforeTask2End = 60;

    CancellationTokenSource _cancelTS = new CancellationTokenSource();
    CancellationToken _cancelToken = _cancelTS.Token;

    private void Start()
    {
        Task task1 = Task.Run(()=> Task1(_cancelToken));
        Task task2 = Task.Run(()=> Task2(_cancelToken));
    }




    private async void Task1(CancellationToken cancelToken)
    {
        if(cancelToken.IsCancellationRequested()
        {
            Debug.Log("Task 1 cancelled by Cancellation Token");
        }
        await Task.Delay(1000);
        Debug.Log("Task1 completed work.");
    }

    private async void Task2(CancellationToken cancelToken)
    {
        while(_frameCounter<C_FrameBeforeTask2End)
        {
            if(cancelToken.IsCancellationRequested()
            {
                Debug.Log("Task 2 cancelled by Cancellation Token");
            }
            _frameCounter++;
            await Task.Yield();
        }
        _frameCounter=0;
        Debug.Log("Task1 completed work.");
    }

    private void private void OnDestroy()
    {
        _cancelTS.Cancel();
        _cancelTS.Dispose();
    }

}