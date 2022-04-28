using Unity.Collections;
using Unity.Jobs;

namespace HW2.T1Files
{
    public struct T1: IJob
    {
        internal NativeArray<int> _array;
        public void Execute()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                if (_array[i] > 10)
                    _array[i] = 0;
            }
        }
    }
}