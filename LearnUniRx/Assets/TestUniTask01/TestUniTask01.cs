using System;
using System.Threading;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;


/*
 msdn c# cancel task
 https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/cancel-an-async-task-or-a-list-of-tasks?source=docs
 
 unity issue  task.delay
 https://issuetracker.unity3d.com/issues/taskcanceledexception-is-thrown-when-canceling-a-cancellationtokensource-in-ondestroy


*/
namespace TestUniTask01
{
    public class TestUniTask01 : MonoBehaviour
    {
        private CancellationTokenSource _cts;
        
        private async void Start()
        {
            _cts = new CancellationTokenSource();
            
            Debug.Log("task start");
            await ExecuteTaskAsync(_cts.Token);
            
        }
        
        private static async Task ExecuteTaskAsync(CancellationToken ct)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), ct);  //unity issue
            //await UniTask.Delay(5000, false, PlayerLoopTiming.Update, ct);
            
            Debug.Log("task execute");
        }
        
        public void OnCancelTaskCallback()
        {
            try
            {
                if (_cts.IsCancellationRequested) return;
                
                _cts.Cancel();
                Debug.Log("task cancel");
            }
            catch (Exception e)
            {
                Debug.Log($"exception {e}");
                throw;
            }
        }
    }
}
