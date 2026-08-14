using UnityEngine;
using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Extensions
{
    public static class AsyncOperationExtensions
    {
        public static Task GetTask(this AsyncOperation operation)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            void onOperationCompleted(AsyncOperation op)
            {
                op.completed -= onOperationCompleted;

                taskCompletionSource.SetResult(true);
            }

            operation.completed += onOperationCompleted;

            return taskCompletionSource.Task;
        }

        public static Task<TObject> GetTask<TObject>(this ResourceRequest operation) where TObject : UnityEngine.Object
        {
            TaskCompletionSource<TObject> taskCompletionSource = new TaskCompletionSource<TObject>();

            void onOperationCompleted(AsyncOperation op)
            {
                op.completed -= onOperationCompleted;

                taskCompletionSource.SetResult((TObject)((ResourceRequest)op).asset);
            }

            operation.completed += onOperationCompleted;

            return taskCompletionSource.Task;
        }
    }
}
